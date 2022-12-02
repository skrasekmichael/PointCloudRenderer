using PointCloudRenderer.Data;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using PointCloudRenderer.APP.Helpers.Models;

namespace PointCloudRenderer.APP.Scenes;

public class SimplePointCloudScene : IScene
{
	private const float FOVY = MathF.PI / 2;

	private SimplePointCloudModel pointCloudModel = null!;
	private AxisLinesModel axis = null!;
	private AxisCirclesModel circleAxis = null!;

	public float ZoomLevel { get; set; } = 2;
	public float Width { get; set; } = 1;
	public float Height { get; set; } = 1;
	public float OrbitAngleX { get; set; }
	public float OrbitAngleY { get; set; }
	public bool DisplayAxis { get; set; } = true;
	public bool DisplayCircleAxis { get; set; } = true;

	public void Load(PointCloud cloud)
	{
		GL.Enable(EnableCap.DepthTest);
		
		pointCloudModel = new SimplePointCloudModel(cloud);
		axis = new();
		circleAxis = new();
	}

	public void Render()
	{
		GL.ClearColor(0.7f, 0.7f, 0.7f, 1);
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		var mvp =
			Matrix4.CreateFromAxisAngle(new(0, 1, 0), OrbitAngleX) *
			Matrix4.CreateFromAxisAngle(new(1, 0, 0), OrbitAngleY) *
			Matrix4.CreateTranslation(0, 0, -ZoomLevel) *
			Matrix4.CreatePerspectiveFieldOfView(FOVY, Width / Height, 0.001f, 150.0f);

		pointCloudModel.Render(mvp);

		if (DisplayAxis)
			axis.Render(mvp);

		if (DisplayCircleAxis)
			circleAxis.Render(mvp);

		GL.Finish();
	}

	public void SetPointSize(float size)
	{
		GL.PointSize(size);
	}

	public void Zoom(int delta)
	{
		ZoomLevel = Math.Max(0, ZoomLevel - (float)delta / 10000);
	}
}
