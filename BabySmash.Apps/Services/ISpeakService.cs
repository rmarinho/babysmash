using BabySmash.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabySmash.Core.Services
{
	public interface ISpeakService : IDisposable
	{
		Task SpeakText(string text);
		Task SpeakUriStream(Uri url);

		void SetLanguage(Language language);
	}
}
