using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using PointCloudRenderer.APP.Extensions;
using System.IO;
using System.Reflection;
using System.Text;

namespace PointCloudRenderer.APP.Helpers;

public sealed class Shader : IDisposable
{
	public int ProgramHandle { get; }

	private readonly Dictionary<string, int> uniformLocations = new();
	private readonly Assembly assembly;

	public Shader(string vertexShaderName, string fragmentShaderName)
	{
		assembly = typeof(Shader).Assembly;
		ProgramHandle = CompileProgram(vertexShaderName, fragmentShaderName);
	}

	public Shader(string vertexShaderName, string geometryShaderName, string fragmentShaderName)
	{
		assembly = typeof(Shader).Assembly;
		ProgramHandle = CompileProgram(
			vertexShaderName,
			fragmentShaderName,
			null,
			null,
			geometryShaderName
		);
	}

	private int CompileProgram(
		string vertexShaderName,
		string fragmentShaderName,
		string? tessControlShaderName = null,
		string? tessEvaluationShaderName = null,
		string? geometryShaderName = null)
	{
		var shaders = new List<int>();

		void addShader(string? shaderName, ShaderType type)
		{
			if (shaderName is not null)
			{
				var source = ReadSource(type.Path(shaderName));
				shaders.Add(CompileShader(source, type));
			}
		}

		addShader(vertexShaderName, ShaderType.VertexShader);
		addShader(fragmentShaderName, ShaderType.FragmentShader);
		addShader(tessControlShaderName, ShaderType.TessControlShader);
		addShader(tessEvaluationShaderName, ShaderType.TessEvaluationShader);
		addShader(geometryShaderName, ShaderType.GeometryShader);

		var handle = GL.CreateProgram();
		shaders.ForEach(x => GL.AttachShader(handle, x));
		GL.LinkProgram(handle);

		var infoLog = GL.GetProgramInfoLog(handle);
		if (infoLog != string.Empty)
			throw new Exception(infoLog);

		shaders.ForEach(x => {
			GL.DetachShader(handle, x);
			GL.DeleteShader(x);
		});

		GL.GetProgram(handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

		for (var i = 0; i < numberOfUniforms; i++)
		{
			var key = GL.GetActiveUniform(handle, i, out _, out _);
			var location = GL.GetUniformLocation(handle, key);
			uniformLocations.Add(key, location);
		}

		return handle;
	}

	private int CompileShader(string source, ShaderType type)
	{
		var shader = GL.CreateShader(type);
		GL.ShaderSource(shader, source);

		GL.CompileShader(shader);

		var infoLog = GL.GetShaderInfoLog(shader);
		if (infoLog != string.Empty)
			throw new Exception(infoLog);

		return shader;
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

	public void SetUniform(string name, Vector4 value)
	{
		Use();
		GL.Uniform4(GetUniformLocation(name), value);
	}

	public void Dispose()
	{
		GL.DeleteProgram(ProgramHandle);
		GC.SuppressFinalize(this);
	}

	public void Use() => GL.UseProgram(ProgramHandle);

	public int GetAttribLocation(string attribName) => GL.GetAttribLocation(ProgramHandle, attribName);

	public int GetUniformLocation(string uniformName) => uniformLocations[uniformName];
}
