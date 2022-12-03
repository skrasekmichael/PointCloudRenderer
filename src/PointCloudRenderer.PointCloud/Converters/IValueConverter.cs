namespace PointCloudRenderer.Data.Converters;

public interface IScalarConverter
{
	public static abstract float? Convert(string? text, IFormatProvider? fp);
}

public sealed class FloatScalar : IScalarConverter
{
	public static float? Convert(string? text, IFormatProvider? fp)
	{
		if (float.TryParse(text, fp, out var value))
			return value;
		return null;
	}
}
