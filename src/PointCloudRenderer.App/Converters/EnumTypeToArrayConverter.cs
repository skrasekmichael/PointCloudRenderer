using System.Globalization;
using System.Windows.Data;

namespace PointCloudRenderer.APP.Converters;

public class EnumTypeToArrayConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is Type type)
		{
			return Enum.GetValues(type);
		}

		return null;
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
