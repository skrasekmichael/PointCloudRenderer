using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PointCloudRenderer.APP.Helpers;
using PointCloudRenderer.Data;
using PointCloudRenderer.Data.Converters;
using PointCloudRenderer.Data.Enums;
using PointCloudRenderer.Data.Parser;
using System.Collections.ObjectModel;
using System.Windows;

namespace PointCloudRenderer.APP.ViewModels;

public sealed partial class LoadPointCloudWindowViewModel : BaseViewModel
{
	[ObservableProperty]
	private bool isLoading = false;

	[ObservableProperty]
	private Range lineRange;

	public PointCloud? Cloud { get; private set; }
	public LineFormatOptions Options { get; } = new();
	public ObservableCollection<PointCloudReader.Line> Lines { get; } = new();
	public ObservableCollection<IScalarColumn> ScalarTypes { get; } = new();
	public NamedObject<Type>[] DataTypes { get; } = {
		new(typeof(FloatScalar), "Float"),
		new(typeof(IntScalar), "Int")
	};

	private PointCloudReader? cloudReader;

	public async Task LoadAsync(string path)
	{
		await Task.Run(() => cloudReader = new PointCloudReader(path));
		await LoadScalarConfiguration();
	}

	private async Task LoadScalarConfiguration()
	{
		var scalars = await Task.Run(() =>
		{
			LineRange = cloudReader!.GetRange(5);

			var count = cloudReader.GetNumberOfScalars(LineRange, Options).Max();
			var scalars = cloudReader.GetScalars(count, LineRange, Options);
			return scalars;
		});

		int start = (int)ScalarName.X;

		ScalarTypes.Clear();
		ScalarTypes.Add(new LineIndexes()
		{
			Lines = Enumerable.Range(LineRange.Start.Value, LineRange.End.Value - LineRange.Start.Value)
		});

		foreach (var scalar in scalars)
		{
			ScalarTypes.Add(new ValueType()
			{
				Name = (ScalarName)start,
				DataType = DataTypes.First(),
				Scalars = scalar.Select(x => x.Data).ToArray()
			});

			start = Math.Min(start + 1, (int)ScalarName.Other);
		}

		Lines.Clear();
		foreach (var line in cloudReader!.GetLines(LineRange))
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
					if (t is ValueType type)
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

	[RelayCommand]
	public async Task ChangedSeparator() => await LoadScalarConfiguration();

	public interface IScalarColumn { }

	public sealed record ValueType : IScalarColumn
	{
		public ScalarName Name { get; set; }
		public required NamedObject<Type> DataType { get; set; }
		public string[]? Scalars { get; init; }
	}

	public sealed record LineIndexes : IScalarColumn
	{
		public required IEnumerable<int> Lines { get; init; }
	}
}
