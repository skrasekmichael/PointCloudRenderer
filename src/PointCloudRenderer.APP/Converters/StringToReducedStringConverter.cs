using System.Globalization;
using System.Windows.Data;

namespace PointCloudRenderer.APP.Converters;

public sealed class StringToReducedStringConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is string text && parameter is int len)
		{
			if (len >= text.Length)
				return text;

			var span = text.AsSpan();

			if (span.Length <= 7 || len <= 7)
				return span[..Math.Min(len, span.Length)].ToString();

			return $"{span[..3]}... {span[new Index(len - 7, true)..]}";
		}

		return null;
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
