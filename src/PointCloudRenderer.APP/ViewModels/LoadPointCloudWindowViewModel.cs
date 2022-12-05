using CommunityToolkit.Mvvm.Input;
using PointCloudRenderer.APP.Helpers;
using PointCloudRenderer.Data;
using PointCloudRenderer.Data.Converters;
using PointCloudRenderer.Data.Enums;
using PointCloudRenderer.Data.Parser;
using System.Collections.ObjectModel;
using System.Windows;

namespace PointCloudRenderer.APP.ViewModels;

public class LoadPointCloudWindowViewModel : BaseViewModel
{
	public RelayCommand<Window?> LoadCommand { get; }
	public RelayCommand<Window?> CancelCommand { get; }

	public PointCloud? Cloud { get; private set; }
	public LineFormatOptions Options { get; } = new();
	public ObservableCollection<string> Lines { get; } = new();
	public ObservableCollection<ValueType> ScalarTypes { get; } = new();
	public NamedType[] DataTypes { get; } = {
		new(typeof(FloatScalar), "Float"),
		new(typeof(IntScalar), "Int")
	};

	private PointCloudReader? cloudReader;

	public LoadPointCloudWindowViewModel()
	{
		LoadCommand = new RelayCommand<Window?>(LoadCloud);
		CancelCommand = new RelayCommand<Window?>(win => win!.Close());
	}

	public void Load(string path)
	{
		cloudReader = new PointCloudReader(path);
		var range = new Range(1, 5);

		var count = cloudReader.GetNumberOfScalars(range, Options).Max();

		var scalars = cloudReader.GetScalars(count, range, Options);
		int start = (int)ScalarName.X;

		ScalarTypes.Clear();
		foreach (var scalar in scalars)
		{
			ScalarTypes.Add(new()
			{
				Name = (ScalarName)start,
				DataType = DataTypes.First(),
				Scalars = scalar.Select(x => x.Scalar).ToArray()
			});

			start = Math.Min(start + 1, (int)ScalarName.Other);
		}

		Lines.Clear();
		foreach (var line in cloudReader.GetLines(range))
		{
			Lines.Add(line);
		}
	}

	public void LoadCloud(Window? window)
	{
		var builder = new LineParserBuilder(Options);

		foreach (var type in ScalarTypes)
		{
			if (type.DataType.Type == typeof(FloatScalar))
				builder.AddScalar<FloatScalar>(type.Name);
			else if (type.DataType.Type == typeof(IntScalar))
				builder.AddScalar<IntScalar>(type.Name);
		}

		try
		{
			(Cloud, _) = cloudReader!.ParsePointCloud(builder.Build());

			window!.DialogResult = true;
			window.Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public sealed record ValueType
	{
		public ScalarName Name { get; set; }
		public required NamedType DataType { get; set; }
		public string[]? Scalars { get; init; }
	}
}
