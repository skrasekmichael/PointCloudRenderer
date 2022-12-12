using CommunityToolkit.Mvvm.Input;
using PointCloudRenderer.APP.Scenes;
using PointCloudRenderer.Data;

namespace PointCloudRenderer.APP.ViewModels;

public partial class BaseSceneViewModel : BaseViewModel
{
	public BaseScene Scene { get; }

	public BaseSceneViewModel(BaseScene scene)
	{
		Scene = scene;
	}

	[RelayCommand]
	public void SetAxisVisibility(bool isVisible)
	{
		Scene.DisplayAxis = isVisible;
		Scene.DisplayCircleAxis = isVisible;
	}

	public void Zoom(int delta)
	{
		Scene.ZoomLevel = Math.Max(0, Scene.ZoomLevel - delta * 0.0001f);
	}

	public virtual void LoadCloud(PointCloud cloud)
	{
	}
}
