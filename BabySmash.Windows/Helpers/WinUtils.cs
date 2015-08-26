using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabySmash.Windows.Helpers
{
	internal static class WinUtils
	{

		public static global::Windows.UI.Color ToWinColor(this Xamarin.Forms.Color color)
		{
			return global::Windows.UI.Color.FromArgb((byte)(color.A  * 255),(byte)(color.R  * 255), (byte)(color.G  * 255), (byte)(color.B  * 255));
		}
	}
}
