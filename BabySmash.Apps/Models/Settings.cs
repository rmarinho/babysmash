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
		}
		public int ClearAfter
		{
			get; set;
		}
	}
}
