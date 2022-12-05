namespace PointCloudRenderer.Data.Converters;

public interface IScalarConverter
{
	public static abstract float? Convert(string? text, IFormatProvider? fp);
}
