using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace OpenSprinklerApp.Converters
{
    public class DateConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value is DateTime && parameter is string)
			{
				return ((DateTime)value).ToString((string)parameter);
			}
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
