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

	private float _angleX = 0;
	public float OrbitAngleX
	{
		get => _angleX;
		set
		{
			_angleX = value;
			SetCamera();
		}
	}

	private float _angleY = 0;
	public float OrbitAngleY
	{
		get => _angleY;
		set
		{
			_angleY = value;
			SetCamera();
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

	public void Zoom(int delta)
	{
		ZoomLevel = Math.Max(0, ZoomLevel - (float)delta / 10000);
	}

	public MainWindowViewModel(SimplePointCloudScene scene)
	{
		Scene = scene;
	}

	public void LoadCloud(string path)
	{
		var builder = new LineParserBuilder(new());

		builder.AddPoint<IValueConverter.Float>();
		//builder.AddColor<IValueConverter.Float>();

		var cloud = PointCloud.FromFile(path, builder.Build());

		Scene.Load(cloud);
	}

	private void SetCamera()
	{
		var x = OrbitAngleX * MathF.PI / 180.0f;
		var y = OrbitAngleY * MathF.PI / 180.0f;

		Scene.OrbitAngleX = x;
		Scene.OrbitAngleY = y;

		OnPropertyChanged(nameof(OrbitAngleX));
		OnPropertyChanged(nameof(OrbitAngleY));
	}
}
