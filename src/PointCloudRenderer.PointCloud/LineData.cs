using OpenTK.Mathematics;

namespace PointCloudRenderer.Data;

public sealed record LineData
{
	public required Vector3 Point { get; init; }
	public required Vector4 Color { get; init; }
}
