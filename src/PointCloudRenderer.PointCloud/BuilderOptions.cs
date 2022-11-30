using PointCloudRenderer.Data.Enums;

namespace PointCloudRenderer.Data;

public sealed class BuilderOptions
{
	public Separator Separator { get; init; } = Separator.Space;
	public string[] Comments { get; init; } = { "\\#", "\\/\\/" };
}
