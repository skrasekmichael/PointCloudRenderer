using PointCloudRenderer.APP.Scenes;
using PointCloudRenderer.Data;

namespace PointCloudRenderer.APP.ViewModels;

public sealed class SimplePointCloudSceneViewModel : BaseSceneViewModel
{
	private float _pointSize = 0.07f;
	public float PointSize
	{
		get => _pointSize;
		set
		{
			_pointSize = value;
			(Scene as SimplePointCloudScene)?.SetPointSize(value);
			OnPropertyChanged();
		}
	}

	public SimplePointCloudSceneViewModel(SimplePointCloudScene scene) : base(scene)
	{

	}

	public override void LoadCloud(PointCloud cloud)
	{
		Scene.Load(cloud);
	}
}
