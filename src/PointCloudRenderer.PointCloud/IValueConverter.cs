namespace PointCloudRenderer.Data;

public interface IValueConverter
{
	public static abstract float? Convert(string? text, IFormatProvider? fp);

	public class Float : IValueConverter
	{
		public static float? Convert(string? text, IFormatProvider? fp)
		{
			if (float.TryParse(text, fp, out var value))
				return value;
			return null;
		}
	}
}
