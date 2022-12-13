using PointCloudRenderer.APP.Scenes;

namespace PointCloudRenderer.APP.ViewModels;

public sealed class SimplePointCloudSceneViewModel : BaseSceneViewModel
{
	public override string Name => "Simple Rendering";

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
}
