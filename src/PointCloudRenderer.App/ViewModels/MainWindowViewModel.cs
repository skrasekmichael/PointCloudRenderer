using PointCloudRenderer.APP.Scenes;
using PointCloudRenderer.Data;

namespace PointCloudRenderer.APP.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
	public SimplePointCloudScene Scene { get; }


	private float _pointSize = 0;
	public float PointSize
	{
		get => _pointSize;
		set
		{
			_pointSize = value;
			Scene.SetPointSize(value);
		}
	}

	public float ZoomLevel
	{
		get => Scene.ZoomLevel;
		set
		{
			Scene.ZoomLevel = value;
			OnPropertyChanged(nameof(ZoomLevel));
		}
	}

	public MainWindowViewModel(SimplePointCloudScene scene)
	{
		Scene = scene;
	}

	public void Zoom(int delta)
	{
		ZoomLevel = Math.Max(0, ZoomLevel - (float)delta / 10000);
	}

	public void LoadCloud(string path)
	{
		var builder = new LineParserBuilder(new());

		builder.AddPoint<IValueConverter.Float>();
		//builder.AddColor<IValueConverter.Float>();

		var cloud = PointCloud.FromFile(path, builder.Build());

		Scene.Load(cloud);
		PointSize = 3;
	}
}
