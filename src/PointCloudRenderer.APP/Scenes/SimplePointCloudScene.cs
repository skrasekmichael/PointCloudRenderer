using PointCloudRenderer.Data;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using PointCloudRenderer.APP.Helpers.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PointCloudRenderer.APP.Scenes;

public sealed partial class SimplePointCloudScene : BaseScene
{
	private const float FOVY = MathF.PI / 2;

	private SimplePointCloudModel? pointCloudModel = null;
	private AxisLinesModel axis = null!;
	private AxisCirclesModel circleAxis = null!;

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

	public override void Load(PointCloud cloud)
	{
		GL.Enable(EnableCap.DepthTest);

		Dispose();

		pointCloudModel = new SimplePointCloudModel(cloud);
		axis = new();
		circleAxis = new();
	}

	public override void Render()
	{
		GL.ClearColor(0.7f, 0.7f, 0.7f, 1);
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		if (pointCloudModel is not null)
		{
			var proj = Matrix4.CreatePerspectiveFieldOfView(FOVY, Width / Height, 0.001f, 150.0f);
			var model =
				Matrix4.CreateFromAxisAngle(new(0, 1, 0), OrbitAngleX) *
				Matrix4.CreateFromAxisAngle(new(1, 0, 0), OrbitAngleY);

			var mvpCloud = model * Matrix4.CreateTranslation(0, 0, -ZoomLevel) * proj;
			pointCloudModel.Render(mvpCloud, proj);

			var mvpAxis = model * Matrix4.CreateTranslation(0, 0, -1 / AxisSize) * proj;

			if (DisplayAxis)
				axis.Render(mvpAxis);

			if (DisplayCircleAxis)
				circleAxis.Render(mvpAxis);
		}

		GL.Finish();
	}

	public void SetPointSize(float size)
	{
		pointCloudModel?.SetPointSize(size);
	}

	public override void Dispose()
	{
		pointCloudModel?.Dispose();
		axis?.Dispose();
		circleAxis?.Dispose();
	}
}
