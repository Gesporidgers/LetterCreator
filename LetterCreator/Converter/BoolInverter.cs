using System.Globalization;

namespace LetterCreator.Converter
{
	public class BoolInverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool booleanValue)
			{
				return !booleanValue;
			}
			return value;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool booleanValue)
			{
				return !booleanValue;
			}
			return value;
		}
	}
}
