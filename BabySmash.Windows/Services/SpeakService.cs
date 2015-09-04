using BabySmash.Core.Services;
using System;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using BabySmash.Core.Models;
using System.Linq;
using System.IO;

namespace BabySmash.Windows.Services
{
	public class SpeakService : ISpeakService
	{
		public SpeakService(ISoundService soundService)
		{
			this.synthesizer = new SpeechSynthesizer();
			this.soundService = soundService;
		}

		public void SetLanguage(Language language)
		{
			this.synthesizer.Voice = SpeechSynthesizer.AllVoices.FirstOrDefault(v => v.Id == language.Id);
		}

		public async Task SpeakTextAsync(string text)
		{
			using(var stream = await this.synthesizer.SynthesizeTextToStreamAsync(text)) {
				await this.soundService.PlayStreamAsync(stream.AsStream(), stream.ContentType);
			}
		}

		public Task SpeakUriStreamAsync(Uri url)
		{
			return this.soundService.PlayUrlAsync(url);
		}

		public async Task SpeakSSMLAsync(string text)
		{
			using(var stream = await this.synthesizer.SynthesizeSsmlToStreamAsync(text)) {
				await this.soundService.PlayStreamAsync(stream.AsStream(), stream.ContentType);
			}
		}

		public void Dispose()
		{
			if(this.synthesizer != null) {
				this.synthesizer.Dispose();
				this.synthesizer = null;
			}
			
		}

		private SpeechSynthesizer synthesizer;
		private readonly ISoundService soundService;
	
	
	}
}
