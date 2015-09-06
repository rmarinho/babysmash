using BabySmash.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabySmash.Core.Services
{
	public interface ILanguageService
	{
		string GetLanguageTextForLetter(string letter);
		string GetLanguageTextForShape(ShapeType shape);

		IList<Language> GetLanguages();

		Language DefaultLanguage
		{
			get;
		}

		string GetResourceText(string key);
	}
}
