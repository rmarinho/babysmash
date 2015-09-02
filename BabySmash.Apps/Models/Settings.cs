using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabySmash.Core.Models
{
	public class Settings
	{

		private static Settings defaultInstance;
        
        public static Settings Default {
            get {
				return defaultInstance ?? (defaultInstance = new Settings());
            }
        }
		public Settings()
		{
			ClearAfter = 35;
			ForceUppercase = true;
			Speak = true;
			UseEffects = false;
			UseAnimations = true;
			FadeAfter = 30;
			FadeAway = true;
			FontSize = 150;
		}
		public int ClearAfter
		{
			get; set;
		}
		public bool ForceUppercase
		{
			get;
			set;
		}
		public string FontFamily
		{
			get;
			set;
		}

		public bool Speak
		{
			get;
			set;
		}

		public bool UseEffects
		{
			get;
			set;
		}

		public bool UseAnimations
		{
			get;
			set;
		}

		public int FadeAfter
		{
			get;
			set;
		}
		public bool FadeAway
		{
			get;
			set;
		}
		public int FontSize
		{
			get;
			internal set;
		}
	}
}
