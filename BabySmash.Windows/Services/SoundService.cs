using BabySmash.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using System.IO;
using System.Reflection;

namespace BabySmash.Windows.Services
{
	class SoundService : ISoundService
	{
		public SoundService()
		{
			this.mediaElement = App.Current.Resources[nameof(this.mediaElement)] as MediaElement;
			if(this.mediaElement == null)
				throw new ArgumentNullException(nameof(this.mediaElement));

			this.mediaElement.MediaEnded += MediaElementMediaEnded;
			this.mediaElement.MediaFailed += MediaElementMediaFailed;
		}


		public Task PlayUrlAsync(Uri url)
		{
			return Play(null, null, url);
		}

		public async Task PlayEmbebedResourceAsync(string path)
		{
			var assembly = typeof(ILanguageService).GetTypeInfo().Assembly;
			using(var stream = assembly.GetManifestResourceStream(path)) {
				await Play(stream.AsRandomAccessStream(), "wav");
			}
		}

		public Task PlayStreamAsync(Stream stream, string mimeType)
		{
			return Play(stream.AsRandomAccessStream(), mimeType);
		}

		private int playCount;
		private MediaElement mediaElement;
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
			if(!this.tcsPlaying.Task.IsCompleted) {
				this.tcsPlaying.TrySetException(new Exception("SpeakService was disposed"));
			}
			if(this.mediaElement.CurrentState == global::Windows.UI.Xaml.Media.MediaElementState.Playing)
				this.mediaElement.Stop();

			this.mediaElement.MediaEnded -= MediaElementMediaEnded;
			this.mediaElement.MediaFailed -= MediaElementMediaFailed;
		}

		
	}
}
