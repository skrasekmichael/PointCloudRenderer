using PointCloudRenderer.Data.Enums;

namespace PointCloudRenderer.Data.Extensions;

public static class SeparatorExtensions
{
	private static readonly string[] separator2char = new string[4] { " ", "\\t", ",", ";" };
	public static string ToStringSeparator(this Separator separator) => separator2char[(int)separator];
}
