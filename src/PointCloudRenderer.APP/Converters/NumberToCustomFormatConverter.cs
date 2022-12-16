using System.ComponentModel;
using System.Globalization;
using System.Numerics;
using System.Windows.Data;

namespace PointCloudRenderer.APP.Converters;

public sealed class NumberToCustomFormatConverter : IValueConverter
{
	private static readonly NumberFormatInfo format = new()
	{
		NumberGroupSeparator = " "
	};

	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is int num)
			return num.ToString("n0", format);

		return null;
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
