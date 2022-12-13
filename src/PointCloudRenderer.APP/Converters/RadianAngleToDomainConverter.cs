using System.Globalization;
using System.Windows.Data;

namespace PointCloudRenderer.APP.Converters;

public sealed class RadianAngleToDomainConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is float val)
			return val % MathF.Tau;

		return null;
	}

	public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture) => value;
}
