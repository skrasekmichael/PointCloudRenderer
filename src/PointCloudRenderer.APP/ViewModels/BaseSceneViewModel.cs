using CommunityToolkit.Mvvm.Input;
using PointCloudRenderer.APP.Scenes;
using PointCloudRenderer.Data;

namespace PointCloudRenderer.APP.ViewModels;

public partial class BaseSceneViewModel : BaseViewModel
{
	public bool IsRotating { get; set; } = false;
	public double MouseX { get; set; }
	public double MouseY { get; set; }
	public double SX { get; set; }
	public double SY { get; set; }
	public BaseScene Scene { get; }
	public virtual string Name { get; } = "Base Scene";

	public BaseSceneViewModel(BaseScene scene)
	{
		Scene = scene;
	}

	[RelayCommand]
	public void SetAxisVisibility(bool isVisible)
	{
		Scene.DisplayAxis = isVisible;
	}

	[RelayCommand]
	public void Zoom(int delta) => Scene.ZoomLevel = Math.Clamp(Scene.ZoomLevel - delta * 0.0001f, 0, 5);

	[RelayCommand]
	public void StartRotating()
	{
		SX = MouseX;
		SY = MouseY;

		IsRotating = true;
		SetAxisVisibility(true);
		Scene.StartRotaring();
	}

	[RelayCommand]
	public void Rotate()
	{
		if (IsRotating)
			Scene.Rotate(SX, SY, MouseX, MouseY);
	}

	[RelayCommand]
	public void StopRotating()
	{
		if (IsRotating)
		{
			IsRotating = false;
			Scene.StopRotating();
			SetAxisVisibility(false);
		}
	}

	[RelayCommand]
	public void SetCanvasWidth(double width) => Scene.Width = (float)width;

	[RelayCommand]
	public void SetCanvasHeight(double height) => Scene.Height = (float)height;

	public virtual void LoadCloud(PointCloud cloud) => Scene.Load(cloud);
}
