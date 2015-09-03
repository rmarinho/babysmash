using BabySmash.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabySmash.Core.Models;
using Windows.Media.SpeechSynthesis;

namespace BabySmash.Windows.Services
{
	public class LanguageService : ILanguageService
	{
		public LanguageService()
		{
			this.synthesizer = new SpeechSynthesizer();
		}
		public Language DefaultLanguage
		{
			get
			{
				return this.defaultLanguage ?? (this.defaultLanguage = GetLanguages().FirstOrDefault(v => v.Id == SpeechSynthesizer.DefaultVoice.Id));
			}
		}

		public IList<Language> GetLanguages()
		{
			return this.availableLanguages ?? (this.availableLanguages = SpeechSynthesizer.AllVoices.Select(v => new Language { Id = v.Id, Locale = v.Language, FriendlyName = v.DisplayName }).ToList());
		}

		public string GetLanguageTextForLetter(string letter)
		{
			throw new NotImplementedException();
		}

		public string GetLanguageTextForShape(string shape)
		{
			throw new NotImplementedException();
		}

		private SpeechSynthesizer synthesizer;
		private Language defaultLanguage;
		private List<Language> availableLanguages;
	}
}
