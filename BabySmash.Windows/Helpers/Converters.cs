using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BabySmash.Windows.Helpers
{
	public class FormsColorConverter : IValueConverter
	{
		// This converts the DateTime object to the string to display.
		public object Convert(object value, Type targetType,
			object parameter, string language)
		{
			if (value == null)
				return global::Windows.UI.Colors.Black;
			// Retrieve the format string and use it to format the value.
			Xamarin.Forms.Color color = (Xamarin.Forms.Color) value;
			return color.ToWinColor();
		}

		// No need to implement converting back on a one-way binding 
		public object ConvertBack(object value, Type targetType,
			object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}

}
