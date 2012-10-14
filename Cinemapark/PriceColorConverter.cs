using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Cinemapark
{
	public class PriceColorConverter : IValueConverter
	{

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var price = int.Parse(value.ToString());

			if (price <= 300)
				return new SolidColorBrush(Colors.Green);
			if (price <= 450)
				return new SolidColorBrush(Colors.Yellow);
			return new SolidColorBrush(Colors.Red);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
