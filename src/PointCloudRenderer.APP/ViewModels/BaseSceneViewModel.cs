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
	public void Zoom(int delta) => Scene.ZoomLevel = Math.Max(0, Scene.ZoomLevel - delta * 0.0001f);

	[RelayCommand]
	public void StartRotating()
	{
		SX = MouseX;
		SY = MouseY;

		SetAxisVisibility(true);
		IsRotating = true;
	}

	[RelayCommand]
	public void Rotate()
	{
		if (IsRotating)
		{
			var dx = (float)(MouseX - SX) * 0.01f;
			var dy = (float)(MouseY - SY) * 0.01f;

			SX = MouseX;
			SY = MouseY;

			Scene.OrbitAngleX = (Scene.OrbitAngleX + dx) % MathF.Tau + MathF.Tau;
			Scene.OrbitAngleY = (Scene.OrbitAngleY + dy) % MathF.Tau + MathF.Tau;
		}
	}

	[RelayCommand]
	public void SetCanvasWidth(double width) => Scene.Width = (float)width;

	[RelayCommand]
	public void SetCanvasHeight(double height) => Scene.Height = (float)height;

	public virtual void LoadCloud(PointCloud cloud) => Scene.Load(cloud);
}
