using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace PointCloudRenderer.APP.Helpers.Models;

public class CircleModel
{
	private static readonly uint[] indices =
	{
		0, 1, 2, 0, 2, 3
	};

	private readonly float[] vertices;
	private readonly Shader shader;
	private readonly int vao;

	public float Size { get; set; } = 1;

	public Vector4 Color { get; set; } = new(1, 0, 0, 1);

	public CircleModel(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
	{
		vertices = new float[]
		{
			p1.X, p1.Y, p1.Z, 0, 1,
			p2.X, p2.Y, p2.Z, 1, 1,
			p3.X, p3.Y, p3.Z, 1, 0,
			p4.X, p4.Y, p4.Z, 0, 0,
		};

		vao = GL.GenVertexArray();
		GL.BindVertexArray(vao);

		var vbo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
		GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

		var ebo = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
		GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

		shader = new Shader("Circle", "Circle");
		shader.Use();

		var position = shader.GetAttribLocation("aPosition");
		GL.VertexAttribPointer(position, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
		GL.EnableVertexAttribArray(position);

		var coords = shader.GetAttribLocation("aCoords");
		GL.VertexAttribPointer(coords, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
		GL.EnableVertexAttribArray(coords);
	}

	public void Render(Matrix4 mvp)
	{
		shader.Use();
		shader.SetUniform("mvp", mvp);
		shader.SetUniform("color", Color);

		GL.BindVertexArray(vao);
		GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
	}
}
