using PointCloudRenderer.Data;
using OpenTK.Graphics.OpenGL;
using PointCloudRenderer.APP.Helpers;
using OpenTK.Mathematics;

namespace PointCloudRenderer.APP.Scenes;

public class BasicPointCloudScene : IScene
{
	private int vboPoints, len = 0;
	private float zoom = 0.0f, orbitX, orbitY;
	private Shader shader = null!;

	public float Width { get; set; } = 1;
	public float Height { get; set; } = 1;

	public void Load(PointCloud cloud)
	{
		len = cloud.Points.Length;

		GL.Enable(EnableCap.ProgramPointSize);
		GL.Enable(EnableCap.DepthTest);

		vboPoints = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, vboPoints);
		GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * len, cloud.Points, BufferUsageHint.StaticDraw);

		shader = new Shader("BasicPointCloud");
		shader.Use();

		var position = shader.GetAttribLocation("aPosition");
		var color = shader.GetAttribLocation("aColor");

		GL.VertexAttribPointer(position, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 0);
		GL.EnableVertexAttribArray(position);
		
		GL.VertexAttribPointer(color, 4, VertexAttribPointerType.Float, false, 7 * sizeof(float), 3 * sizeof(float));
		GL.EnableVertexAttribArray(color);

		GL.PointSize(3);
	}

	public void Render()
	{
		GL.Viewport(0, 0, (int)Width, (int)Height);

		GL.ClearColor(0.7f, 0.7f, 0.7f, 1);
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		var view = Matrix4.CreateTranslation(0.0f, 0.0f, -zoom);
		shader.SetUniform("view", view);

		var model = Matrix4.Identity *
			Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), orbitX) *
			Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), orbitY);
		shader.SetUniform("model", model);

		var proj = Matrix4.CreatePerspectiveFieldOfView(MathF.PI / 2, Width / Height, 0.01f, 100.0f);
		shader.SetUniform("projection", proj);

		GL.BindVertexArray(vboPoints);
		GL.DrawArrays(PrimitiveType.Points, 0, len);

		GL.Finish();
	}

	public void SetPointSize(float size)
	{
		GL.PointSize(size);
	}

	public void Zoom(int delta)
	{
		zoom = Math.Max(0, zoom - (float)delta / 10000);
	}

	public void SetOrbitAngleX(float angle)
	{
		orbitX = angle;
	}

	public void SetOrbitAngleY(float angle)
	{
		orbitY = angle;
	}
}
