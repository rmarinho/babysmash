using BabySmash.Core.Models;
using BabySmash.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabySmash.Core.ViewModels
{
	public class SettingsViewModel : BaseViewModel, IDisposable
	{
		private ISpeakService speakService;
		private ILanguageService languageService;

		public SettingsViewModel(ISpeakService speakService, ILanguageService languageService)
		{
			this.speakService = speakService;
			this.languageService = languageService;

			SetupLanguages();
		}

		private void SetupLanguages()
		{
			foreach(var lang in this.languageService.GetLanguages()) {
				AvailableLanguages.Add(lang);
			}

			CurrentLanguage = AvailableLanguages.FirstOrDefault(v => v.Id == this.languageService.DefaultLanguage.Id);
		}
		private ObservableCollection<Language> _availableLanguages = new ObservableCollection<Language>();
		public ObservableCollection<Language> AvailableLanguages
		{
			get
			{
				return _availableLanguages;
			}
			set
			{
				SetField(ref _availableLanguages, value);
			}
		}


		private Language _currentLanguage;
		public Language CurrentLanguage
		{
			get
			{
				return _currentLanguage;
			}
			set
			{
				SetField(ref _currentLanguage, value);

				if(_currentLanguage != null)
					this.speakService.SetLanguage(_currentLanguage);
			}
		}

		public void Dispose()
		{
			
		}
	}
}
