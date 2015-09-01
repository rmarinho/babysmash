using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabySmash.Core.Services;
using Windows.Media.SpeechSynthesis;
using Windows.Media.Playback;
using Windows.UI.Xaml.Controls;
using Windows.Storage.Streams;

namespace BabySmash.Windows.Services
{
	public class SpeakService : ISpeakService, IDisposable
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

		public async Task SpeakText(string text)
		{
			using(var stream = await synthesizer.SynthesizeTextToStreamAsync(text)) {
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
			playCount++;
			tcsPlaying = new TaskCompletionSource<bool>();
			if(url != null)
				this.mediaElement.Source = url;
			else
				this.mediaElement.SetSource(stream, mimeType);
			mediaElement.Play();
			try {
				await tcsPlaying.Task;
			}
			catch(Exception e) {
				//TODO: handle this here? retry ? 
				throw e;
			}
		}

		private void MediaElementMediaFailed(object sender, global::Windows.UI.Xaml.ExceptionRoutedEventArgs e)
		{
			playCount--;
			tcsPlaying.TrySetException(new Exception("Media failed"));
		}

		private void MediaElementMediaEnded(object sender, global::Windows.UI.Xaml.RoutedEventArgs e)
		{
			playCount--;
			tcsPlaying.TrySetResult(true);
		}


		public void Dispose()
		{
			if(this.synthesizer != null) {
				this.synthesizer.Dispose();
				this.synthesizer = null;
			}
			if(!tcsPlaying.Task.IsCompleted) {
				tcsPlaying.TrySetException(new Exception("SpeakService was dispoesed"));
			}
			if(this.mediaElement.CurrentState == global::Windows.UI.Xaml.Media.MediaElementState.Playing)
				this.mediaElement.Stop();

			this.mediaElement.MediaEnded -= MediaElementMediaEnded;
			this.mediaElement.MediaFailed -= MediaElementMediaFailed;
		}
	}
}
