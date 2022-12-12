using CommunityToolkit.Mvvm.ComponentModel;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using PointCloudRenderer.APP.Helpers.Models;
using PointCloudRenderer.Data;

namespace PointCloudRenderer.APP.Scenes;

public abstract partial class BaseScene : ObservableObject, IDisposable
{
	private const float FOVY = MathF.PI / 2;

	protected AxisLinesModel? AxisLinesModel { get; private set; }
	protected AxisCirclesModel? AxisCirclesModel { get; private set; }

	[ObservableProperty]
	private float zoomLevel = 2;

	[ObservableProperty]
	public float axisSize = 0.5f;

	[ObservableProperty]
	public float orbitAngleX;

	[ObservableProperty]
	public float orbitAngleY;

	[ObservableProperty]
	public bool displayAxis = false;

	[ObservableProperty]
	public bool displayCircleAxis = false;

	public float Width { get; set; } = 1;
	public float Height { get; set; } = 1;

	public void Load(PointCloud cloud)
	{
		GL.Enable(EnableCap.DepthTest);

		AxisLinesModel?.Dispose();
		AxisLinesModel = new();

		AxisCirclesModel?.Dispose();
		AxisCirclesModel = new();

		LoadCloud(cloud);
	}

	protected abstract void LoadCloud(PointCloud cloud);

	public void Render()
	{
		GL.ClearColor(0.7f, 0.7f, 0.7f, 1);
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		var proj = Matrix4.CreatePerspectiveFieldOfView(FOVY, Width / Height, 0.001f, 150.0f);
		var model =
			Matrix4.CreateFromAxisAngle(new(0, 1, 0), OrbitAngleX) *
			Matrix4.CreateFromAxisAngle(new(1, 0, 0), OrbitAngleY);

		RenderCloud(model, proj);

		var mvpAxis = model * Matrix4.CreateTranslation(0, 0, -1 / AxisSize) * proj;

		if (DisplayAxis)
			AxisLinesModel?.Render(mvpAxis);

		if (DisplayCircleAxis)
			AxisCirclesModel?.Render(mvpAxis);

		GL.Finish();
	}

	protected abstract void RenderCloud(Matrix4 model, Matrix4 proj);

	public virtual void Dispose()
	{
		AxisLinesModel?.Dispose();
		AxisCirclesModel?.Dispose();
	}
}
