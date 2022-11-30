using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.IO;
using System.Reflection;
using System.Text;

namespace PointCloudRenderer.APP.Helpers;

public class Shader : IDisposable
{
	public int ProgramHandle;

	private readonly Dictionary<string, int> uniformLocations = new();
	private readonly Assembly assembly;

	public Shader(string shaderName)
	{
		assembly = typeof(Shader).Assembly;

		var vertexShaderSource = ReadSource($"VertexShaders.{shaderName}");
		var fragmentShaderSource = ReadSource($"FragmentShaders.{shaderName}");

		var vertexShader = GL.CreateShader(ShaderType.VertexShader);
		GL.ShaderSource(vertexShader, vertexShaderSource);

		var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
		GL.ShaderSource(fragmentShader, fragmentShaderSource);

		GL.CompileShader(vertexShader);

		string infoLogVert = GL.GetShaderInfoLog(vertexShader);
		if (infoLogVert != string.Empty)
			throw new Exception(infoLogVert);

		GL.CompileShader(fragmentShader);

		string infoLogFrag = GL.GetShaderInfoLog(fragmentShader);
		if (infoLogFrag != string.Empty)
			throw new Exception(infoLogFrag);

		ProgramHandle = GL.CreateProgram();
		GL.AttachShader(ProgramHandle, vertexShader);
		GL.AttachShader(ProgramHandle, fragmentShader);
		GL.LinkProgram(ProgramHandle);

		GL.DetachShader(ProgramHandle, vertexShader);
		GL.DetachShader(ProgramHandle, fragmentShader);
		GL.DeleteShader(vertexShader);
		GL.DeleteShader(fragmentShader);

		GL.GetProgram(ProgramHandle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

		for (var i = 0; i < numberOfUniforms; i++)
		{
			var key = GL.GetActiveUniform(ProgramHandle, i, out _, out _);
			var location = GL.GetUniformLocation(ProgramHandle, key);
			uniformLocations.Add(key, location);
		}
	}

	private string ReadSource(string shader)
	{
		var stream = assembly.GetManifestResourceStream(shader);
		if (stream is null)
			throw new ArgumentException($"missing {shader}");

		string source;
		using (var reader = new StreamReader(stream, Encoding.UTF8))
		{
			source = reader.ReadToEnd();
		}

		return source;
	}

	public void SetUniform(string name, Matrix4 value)
	{
		Use();
		GL.UniformMatrix4(GetUniformLocation(name), true, ref value);
	}

	public void SetUniform(string name, float value)
	{
		Use();
		GL.Uniform1(GetUniformLocation(name), value);
	}

	~Shader()
	{
		GL.DeleteProgram(ProgramHandle);
	}

	public void Dispose()
	{
		GL.DeleteProgram(ProgramHandle);
		GC.SuppressFinalize(this);
	}

	public void Use()
	{
		GL.UseProgram(ProgramHandle);
	}

	public int GetAttribLocation(string attribName)
	{
		return GL.GetAttribLocation(ProgramHandle, attribName);
	}

	public int GetUniformLocation(string uniformName)
	{
		return uniformLocations[uniformName];
	}
}
