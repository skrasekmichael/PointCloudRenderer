using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PointCloudRenderer.APP.Converters;

public sealed class BoolToVisibilityConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is bool isVisible)
			return isVisible ? Visibility.Visible : Visibility.Collapsed;

		return null;
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
