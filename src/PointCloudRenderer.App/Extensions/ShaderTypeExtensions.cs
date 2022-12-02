using OpenTK.Graphics.OpenGL;

namespace PointCloudRenderer.APP.Extensions;

public static class ShaderTypeExtensions
{
	public static string Path(this ShaderType type, string shaderName)
	{
		return type switch
		{
			ShaderType.VertexShader => $"VertexShaders.{shaderName}",
			ShaderType.FragmentShader => $"FragmentShaders.{shaderName}",
			ShaderType.GeometryShader => $"GeometryShaders.{shaderName}",
			_ => throw new NotImplementedException(type.ToString())
		};
	}
}
