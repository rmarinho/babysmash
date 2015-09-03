using BabySmash.Core.Services;
using System;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using BabySmash.Core.Models;
using System.Linq;

namespace BabySmash.Windows.Services
{
	public class SpeakService : ISpeakService
	{
		public SpeakService()
		{
			this.mediaElement = App.Current.Resources[nameof(this.mediaElement)] as MediaElement;
			if(this.mediaElement == null)
				throw new ArgumentNullException(nameof(this.mediaElement));

			this.mediaElement.MediaEnded += MediaElementMediaEnded;
			this.mediaElement.MediaFailed += MediaElementMediaFailed;
		
			this.synthesizer = new SpeechSynthesizer();
		}

		public void SetLanguage(Language language)
		{
			this.synthesizer.Voice = SpeechSynthesizer.AllVoices.FirstOrDefault(v => v.Id == language.Id);
		}

		public async Task SpeakText(string text)
		{
			using(var stream = await this.synthesizer.SynthesizeTextToStreamAsync(text)) {
				await Play(stream, stream.ContentType);
			}
		}

		public Task SpeakUriStream(Uri url)
		{
			return Play(null, "", url);
		}

		private int playCount;
		private MediaElement mediaElement;
		private SpeechSynthesizer synthesizer;
		private TaskCompletionSource<bool> tcsPlaying;
		
		private async Task Play(IRandomAccessStream stream, string mimeType, Uri url = null)
		{
			this.playCount++;
			this.tcsPlaying = new TaskCompletionSource<bool>();
			if(url != null)
				this.mediaElement.Source = url;
			else
				this.mediaElement.SetSource(stream, mimeType);
			this.mediaElement.Play();
			try {
				await this.tcsPlaying.Task;
			}
			catch(Exception e) {
				//TODO: handle this here? retry ? 
				throw e;
			}
		}

		private void MediaElementMediaFailed(object sender, global::Windows.UI.Xaml.ExceptionRoutedEventArgs e)
		{
			this.playCount--;
			this.tcsPlaying.TrySetException(new Exception("Media failed"));
		}

		private void MediaElementMediaEnded(object sender, global::Windows.UI.Xaml.RoutedEventArgs e)
		{
			this.playCount--;
			this.tcsPlaying.TrySetResult(true);
		}


		public void Dispose()
		{
			if(this.synthesizer != null) {
				this.synthesizer.Dispose();
				this.synthesizer = null;
			}
			if(!this.tcsPlaying.Task.IsCompleted) {
				this.tcsPlaying.TrySetException(new Exception("SpeakService was disposed"));
			}
			if(this.mediaElement.CurrentState == global::Windows.UI.Xaml.Media.MediaElementState.Playing)
				this.mediaElement.Stop();

			this.mediaElement.MediaEnded -= MediaElementMediaEnded;
			this.mediaElement.MediaFailed -= MediaElementMediaFailed;
		}

		public async Task SpeakSSML(string text)
		{
			using(var stream = await this.synthesizer.SynthesizeSsmlToStreamAsync(text)) {
				await Play(stream, stream.ContentType);
			}
		}
	}
}
