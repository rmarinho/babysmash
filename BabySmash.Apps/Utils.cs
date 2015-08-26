using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BabySmash.Core
{
	public static class Utils
	{
		private static readonly List<string> shapes;
		private static readonly Dictionary<Color, string> brushToString;
		private static readonly Random lRandom = new Random(); // BUG BUG: Believe it or not, Random is NOT THREAD SAFE!
		private static readonly Color[] someColors;

		private static readonly string[] sounds = {
													  "giggle.wav",
													  "babylaugh.wav",
													  "babygigl2.wav",
													  "ccgiggle.wav",
													  "laughingmice.wav",
													  "scooby2.wav",
												  };

		static Utils()
		{
			shapes = new List<string> {
					"Circle",
					"Oval",
					"Rectangle",
					"Hexagon",
					"Trapezoid",
					"Star",
					"Square",
					"Triangle",
					"Heart",
			};

			brushToString = new Dictionary<Color, string>
								   {
									{Color.Red, "Red"},
									{Color.Blue, "Blue"},
									{Color.Yellow, "Yellow"},
									{Color.Green, "Green"},
									{Color.Purple, "Purple"},
									{Color.Pink, "Pink"},
									{Color.Fuchsia, "Fuchsia"},
									{Color.Lime, "Lime"},
									{Color.Gray, "Gray"}
								};

			someColors = new Color[brushToString.Count];
			brushToString.Keys.CopyTo(someColors, 0);
		}

		public static Color GetRandomColor()
		{
			Color color = someColors[lRandom.Next(0, someColors.Length)];
			return color;
		}

		public static Color LightenOrDarken(this Color src, double degree)
		{
			Color ret = src.AddLuminosity(degree);
			return ret;
		}
	


		public static string ColorToString(Color b)
		{
			return brushToString[b];
		}

		public static string GetRandomSoundFile()
		{
			return sounds[lRandom.Next(0, sounds.Length)];
		}

		public static string GetRandomShape()
		{
			return shapes[lRandom.Next(0, shapes.Count)];
		}

		public static bool GetRandomBoolean()
		{
			if (lRandom.Next(0, 2) == 0)
				return false;
			return true;
		}

		public static int RandomBetweenTwoNumbers(int min, int max)
		{
			return lRandom.Next(min, max + 1);
		}
	}
}
