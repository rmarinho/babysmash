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
	}
}
