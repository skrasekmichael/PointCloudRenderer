using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace PointCloudRenderer.APP.Helpers.Models;

public class LineModel
{
	private readonly Shader shader;
	private readonly int vao;
	private readonly float[] vertices;

	public Vector4 Color { get; set; }

	public LineModel(Vector3 start, Vector3 end)
	{
		vertices = new float[]
		{
			start.X, start.Y, start.Z,
			end.X, end.Y, end.Z,
		};

		vao = GL.GenVertexArray();
		var vbo = GL.GenBuffer();

		GL.BindVertexArray(vao);

		GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
		GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StreamDraw);

		shader = new Shader("Line", "Line");
		shader.Use();

		var position = shader.GetAttribLocation("aPosition");
		GL.VertexAttribPointer(position, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
		GL.EnableVertexAttribArray(position);
	}

	public void Render(Matrix4 mvp)
	{
		shader.SetUniform("mvp", mvp);
		shader.SetUniform("color", Color);

		GL.BindVertexArray(vao);
		GL.DrawArrays(PrimitiveType.Lines, 0, 2);
	}
}
