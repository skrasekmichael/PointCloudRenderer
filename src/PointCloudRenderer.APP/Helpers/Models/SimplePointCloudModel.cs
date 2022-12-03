using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using PointCloudRenderer.Data;

namespace PointCloudRenderer.APP.Helpers.Models;

public class SimplePointCloudModel
{
	private readonly Matrix4 model;
	private readonly Shader shader;
	private readonly uint[] indices;
	private readonly int vao;

	public SimplePointCloudModel(PointCloud cloud)
	{
		indices = Enumerable.Range(0, cloud.Points.Length).Select(x => (uint)x).ToArray();
		model = Matrix4.CreateTranslation(-cloud.Center);

		vao = GL.GenVertexArray();
		GL.BindVertexArray(vao);

		var vbo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
		GL.BufferData(BufferTarget.ArrayBuffer, cloud.Points.Length * sizeof(float), cloud.Points, BufferUsageHint.StaticDraw);

		var ebo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
		GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

		shader = new Shader("SimplePointCloud", "SimplePointCloud", "SimplePointCloud");
		shader.Use();

		var position = shader.GetAttribLocation("aPosition");
		var color = shader.GetAttribLocation("aColor");

		GL.VertexAttribPointer(position, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 0);
		GL.EnableVertexAttribArray(position);

		GL.VertexAttribPointer(color, 4, VertexAttribPointerType.Float, false, 7 * sizeof(float), 3 * sizeof(float));
		GL.EnableVertexAttribArray(color);
	}

	public void Render(Matrix4 mvp, Matrix4 proj)
	{
		shader.Use();
		shader.SetUniform("mvp", model * mvp);
		shader.SetUniform("proj", proj);

		GL.BindVertexArray(vao);
		GL.DrawElements(PrimitiveType.Points, indices.Length, DrawElementsType.UnsignedInt, 0);
	}

	public void SetPointSize(float size)
	{
		shader.SetUniform("scale", size);
	}
}
