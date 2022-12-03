using PointCloudRenderer.Data.Enums;

namespace PointCloudRenderer.Data;

public sealed class LineFormatOptions
{
	public Separator Separator { get; init; } = Separator.Space;
	public string[] Comments { get; init; } = { "\\#", "\\/\\/" };
}
