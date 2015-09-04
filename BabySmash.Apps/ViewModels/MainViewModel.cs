using BabySmash.Core.Models;
using BabySmash.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using static BabySmash.Core.Utils;

namespace BabySmash.Core.ViewModels
{
	public class MainViewModel : BaseViewModel, IDisposable
	{
		private const string introSound = "BabySmash.Core.Data.Sounds.EditedJackPlaysBabySmash.wav";
		private const int timerDelay = 30 * 1000;
		private IInteractionService interactionService;
		private ISpeakService speakService;
		private ISoundService soundService;
		private ILanguageService languageService;
		private Timer timer;
		private bool disposed;
		public MainViewModel(IInteractionService interactionService, ISoundService soundService, ISpeakService speakService, ILanguageService languageService)
		{

			if(interactionService == null)
				throw new ArgumentNullException(nameof(interactionService));

			if(speakService == null)
				throw new ArgumentNullException(nameof(speakService));

			if(languageService == null)
				throw new ArgumentNullException(nameof(languageService));

			if(soundService == null)
				throw new ArgumentNullException(nameof(soundService));

			this.interactionService = interactionService;
			this.soundService = soundService;
			this.speakService = speakService;
			this.languageService = languageService;

			this.timer = new Timer((obj) => {
				CheckFiguresToRemove();
			}, null, timerDelay, timerDelay);

			this.interactionService.InteractionOccured += InteractionService_InteractionOccured;

			//play the intro sound
			this.soundService.PlayEmbebedResourceAsync(introSound);
		}
	
		public void Dispose()
		{
			if(this.disposed)
				return;
			this.disposed = true;

			if(this.timer != null) {
				this.timer.Dispose();
				this.timer = null;
			}

			if(this.interactionService != null) {
				this.interactionService.InteractionOccured -= InteractionService_InteractionOccured;
				this.interactionService = null;
			}

			if(this.speakService != null) {
				this.speakService = null;
			}
		}

		private string _helloMessage ="BabySmash! by Scott Hanselman with community contributions! - http://www.babysmash.com ";
		public string HelloMessage
		{
			get
			{
				return this._helloMessage;
			}
			set
			{
				SetField(ref this._helloMessage, value);
			}
		}

		private ObservableCollection<Figure> _figures = new ObservableCollection<Figure>();
		public ObservableCollection<Figure> Figures
		{
			get
			{
				return _figures;
			}
			set
			{
				SetField(ref _figures, value);
			}
		}
	
		private async void InteractionService_InteractionOccured(object sender, InteractionEventArgs e)
		{
			switch (e.Interaction) {
				case Models.InteractionType.MouseClick:
				break;
				case Models.InteractionType.MouseMove:
				break;
				case Models.InteractionType.KeyPress:
				await ProcessKey(e.Key);
				break;
				case Models.InteractionType.Exit:
				Clear();
				break;
				default:
				break;
			}
		}

		private void Clear()
		{
			Figures.Clear();
		}

		private async Task ProcessKey(string key)
		{
			if(string.IsNullOrEmpty(key))
				return;

			//could be a letter or number
			if(key.Length == 1) {
				// If a letter was pressed, display the letter.
				if(Regex.IsMatch(key, @"^[a-zA-Z]+$"))
					await AddLetter(key[0]);

				// If a number is pressed, display the number.
				if(Regex.IsMatch(key, @"^[0-9]+$")) {
					int number;
					if(int.TryParse(key, out number))
						await AddNumber(int.Parse(key));
				}
			} else {
				// Otherwise, display a random shape.
				await AddFigureAsync(new ShapeFigure());
			}

			CheckFiguresToRemove();
		}

		private void CheckFiguresToRemove()
		{
			if(Figures.Count >= Settings.Default.ClearAfter)
				Device.BeginInvokeOnMainThread(() => {
					Figures.RemoveAt(0);
				});

			for(int i = Figures.Count-1; i >= 0; i--) {
				var shape = Figures[i];
				if(!shape.IsVisible) {
					Device.BeginInvokeOnMainThread(() => {
						Figures.Remove(shape);
					});
				}
			}
		}

		private Task AddLetter(char letter)
		{
			var nl = new LetterFigure(letter);
			return AddFigureAsync(nl);
		}

		private Task AddNumber(int number)
		{
			var nf = new NumberFigure(number);
			return AddFigureAsync(nf);
		}

		private async Task AddFigureAsync(Figure figure)
		{

		    figure.StrokeColor = GetRandomColor();
			figure.FillColor = GetRandomColor();

			//TODO: what should this be
			var shapeWidth = Settings.Default.FontSize;
			var shapeHeight = Settings.Default.FontSize;

			//TODO: get this from DI
			var availableWidth = 1000;
			var availableHeight = 700;

			figure.Size = new Size(shapeWidth, shapeHeight);
			var x = RandomBetweenTwoNumbers(0, Convert.ToInt32(availableWidth - figure.Size.Width));
			var y = RandomBetweenTwoNumbers(0, Convert.ToInt32(availableHeight - figure.Size.Height));
			figure.Position = new Point(x, y);
    		
			
			// var nameFunc = hashTableOfFigureGenerators[Utils.RandomBetweenTwoNumbers(0, hashTableOfFigureGenerators.Count - 1)];
			Figures.Add(figure);
			if(Settings.Default.Speak)
				await this.speakService.SpeakSSMLAsync(this.languageService.GetLanguageTextForLetter(figure.ToString()));
		}

		
	}
}
