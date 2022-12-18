using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PointCloudRenderer.APP.Helpers;
using PointCloudRenderer.Data;
using PointCloudRenderer.Data.Converters;
using PointCloudRenderer.Data.Enums;
using PointCloudRenderer.Data.Parser;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;

namespace PointCloudRenderer.APP.ViewModels;

public sealed partial class LoadPointCloudWindowViewModel : BaseViewModel
{
	[ObservableProperty]
	private bool isLoading = false;

	[ObservableProperty]
	private int lineOffset = 0;

	[ObservableProperty]
	private string filePath = string.Empty;

	public PointCloud? Cloud { get; private set; }
	public LineFormatOptions Options { get; } = new();
	public ObservableCollection<PointCloudReader.Line> Lines { get; } = new();
	public ObservableCollection<ISegmentColumn> ScalarTypes { get; } = new();
	public NamedObject<Type>[] DataTypes { get; } = {
		new(typeof(FloatScalar), "Float"),
		new(typeof(IntScalar), "Int")
	};

	private PointCloudReader? cloudReader;

	public async Task LoadAsync(string path)
	{
		filePath = Path.GetFullPath(path);
		await Task.Run(() => cloudReader = new PointCloudReader(path));
		await LoadScalarConfigurationAsync();
	}

	[RelayCommand]
	private async Task LoadScalarConfigurationAsync()
	{
		var (lineRange, scalars) = await Task.Run(() =>
		{
			var lineRange = cloudReader!.GetRange(LineOffset, 5);
			var count = cloudReader.GetNumberOfScalars(lineRange, Options).Max();
			var scalars = cloudReader.GetScalars(count, lineRange, Options);
			return (lineRange, scalars);
		});

		int start = (int)ScalarName.X;

		ScalarTypes.Clear();
		ScalarTypes.Add(new LineIndexes()
		{
			Lines = Enumerable.Range(lineRange.Start.Value, lineRange.End.Value - lineRange.Start.Value)
		});

		foreach (var scalar in scalars)
		{
			ScalarTypes.Add(new SegmentColumn()
			{
				Name = (ScalarName)start,
				DataType = DataTypes.First(),
				Scalars = scalar.Select(x => x.Data)
			});

			start = Math.Min(start + 1, (int)ScalarName.Other);
		}

		Lines.Clear();
		foreach (var line in cloudReader!.GetLines(lineRange))
			Lines.Add(line);
	}

	[RelayCommand]
	public async Task LoadCloudAsync(Window? window)
	{
		var close = await Task.Run(() =>
		{
			IsLoading = true;
			var builder = new LineParserBuilder(Options);

			try
			{
				foreach (var t in ScalarTypes)
				{
					if (t is SegmentColumn type)
					{
						if (type.DataType.Object == typeof(FloatScalar))
							builder.AddScalar<FloatScalar>(type.Name);
						else if (type.DataType.Object == typeof(IntScalar))
							builder.AddScalar<IntScalar>(type.Name);
					}
				}

				(Cloud, _) = cloudReader!.ParsePointCloud(builder.Build());
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			return false;
		});

		if (close)
		{
			window!.DialogResult = true;
			window.Close();
		}

		IsLoading = false;
	}

	[RelayCommand]
	public void Cancel(Window? window) => window?.Close();

	public interface ISegmentColumn { }

	public sealed record SegmentColumn : ISegmentColumn
	{
		public ScalarName Name { get; set; }
		public required NamedObject<Type> DataType { get; set; }
		public required IEnumerable<string> Scalars { get; init; }
	}

	public sealed record LineIndexes : ISegmentColumn
	{
		public required IEnumerable<int> Lines { get; init; }
	}
}
