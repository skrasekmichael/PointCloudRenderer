using System.Globalization;
using System.Windows.Data;

namespace PointCloudRenderer.APP.Converters;

public sealed class NumberToBoolConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is int num)
			return num != 0;

		return null;
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
