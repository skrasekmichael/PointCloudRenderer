namespace PointCloudRenderer.Data.Converters;

public sealed class IntNormScalar : IScalarConverter
{
	public static float? Convert(string? text, IFormatProvider? fp)
	{
		if (int.TryParse(text, fp, out var value))
			return (float)value / int.MaxValue;
		return null;
	}
}
