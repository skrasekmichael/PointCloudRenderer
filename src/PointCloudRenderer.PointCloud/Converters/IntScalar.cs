namespace PointCloudRenderer.Data.Converters;

public sealed class IntScalar : IScalarConverter
{
	public static float? Convert(string? text, IFormatProvider? fp)
	{
		if (int.TryParse(text, fp, out var value))
			return value;
		return null;
	}
}
