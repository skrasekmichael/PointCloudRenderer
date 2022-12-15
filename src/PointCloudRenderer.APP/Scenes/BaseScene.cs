using CommunityToolkit.Mvvm.ComponentModel;
using PointCloudRenderer.APP.Helpers.Models;
using PointCloudRenderer.Data;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace PointCloudRenderer.APP.Scenes;

public abstract partial class BaseScene : ObservableObject, IDisposable
{
	private const float FOVY = MathF.PI / 2;
	private static readonly Matrix4 cameraOffset = Matrix4.CreateTranslation(0, 0, -1);

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

	private float _width = 1;
	public float Width
	{
		get => _width;
		set
		{
			_width = value;
			aspect = _width / _height;
		}
	}

	private float _height = 1;
	public float Height
	{
		get => _height;
		set
		{
			_height = value;
			aspect = _width / _height;
		}
	}

	private float aspect = 1.0f;

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

		var proj = Matrix4.CreatePerspectiveFieldOfView(FOVY, aspect, 0.001f, 150.0f);
		var model =
			Matrix4.CreateFromAxisAngle(new(0, 1, 0), OrbitAngleX) *
			Matrix4.CreateFromAxisAngle(new(1, 0, 0), OrbitAngleY);

		RenderCloud(model, proj);

		var mvpAxis = model * Matrix4.CreateScale(AxisSize, AxisSize, AxisSize) * cameraOffset * proj;

		if (DisplayAxis)
		{
			AxisLinesModel?.Render(mvpAxis);
			AxisCirclesModel?.Render(mvpAxis);
		}

		GL.Finish();
	}

	protected abstract void RenderCloud(Matrix4 model, Matrix4 proj);

	public virtual void Dispose()
	{
		AxisLinesModel?.Dispose();
		AxisCirclesModel?.Dispose();
	}
}
