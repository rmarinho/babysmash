using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BabySmash.Core.ViewModels
{
	public class BaseViewModel : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}
		protected bool SetField<T>(ref T field, T value, Action action = null, IEnumerable<string> additionalprops = null, [CallerMemberName] string propertyName = null)
		{

			if (EqualityComparer<T>.Default.Equals(field, value))
				return false;
			field = value;
			OnPropertyChanged(propertyName);
			//Check for related fields
			if (additionalprops != null) {
				foreach (var s in additionalprops)
					OnPropertyChanged(s);
			};

			if (action != null)
				action();
			return true;
		}

		private bool _isbusy;
		public bool IsBusy
		{
			get
			{
				return _isbusy;
			}
			set
			{
				SetField(ref _isbusy, value);
			}
		}
	}
}
