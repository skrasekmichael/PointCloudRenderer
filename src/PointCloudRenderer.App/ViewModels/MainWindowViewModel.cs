using PointCloudRenderer.APP.Scenes;
using PointCloudRenderer.Data;

namespace PointCloudRenderer.APP.ViewModels;

public class MainWindowViewModel
{
	public BasicPointCloudScene Scene { get; }

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

	private float _orbitAngleX = 0;
	public float OrbitAngleX
	{
		get => _orbitAngleX;
		set
		{
			_orbitAngleX = value;
			Scene.SetOrbitAngleX(value * MathF.PI * 2 / 360.0f);
		}
	}

	private float _orbitAngleY = 0;
	public float OrbitAngleY
	{
		get => _orbitAngleY;
		set
		{
			_orbitAngleY = value;
			Scene.SetOrbitAngleY(value * MathF.PI * 2 / 360.0f);
		}
	}

	public MainWindowViewModel(BasicPointCloudScene scene)
	{
		Scene = scene;
	}

	public void LoadCloud(string path)
	{
		var builder = new LineParserBuilder(new());

		builder.AddPoint<IValueConverter.Float>();

		var cloud = PointCloud.FromFile(path, builder.Build());

		Scene.Load(cloud);
	}
}
