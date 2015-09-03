using BabySmash.Core.Models;
using BabySmash.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using System.Linq;
using static BabySmash.Core.Utils;

namespace BabySmash.Core.ViewModels
{
	public class MainViewModel : BaseViewModel, IDisposable
	{
		private const int timerDelay = 30 * 1000;
		private IInteractionService interactionService;
		private ISpeakService speakService;
		private ILanguageService languageService;
		private Timer timer;
		public MainViewModel(IInteractionService interactionService, ISpeakService speakService, ILanguageService languageService)
		{
			
			if(interactionService == null)
				throw new ArgumentNullException(nameof(interactionService));

			if(speakService == null)
				throw new ArgumentNullException(nameof(speakService));

			if(languageService == null)
				throw new ArgumentNullException(nameof(languageService));

			this.interactionService = interactionService;
			this.speakService = speakService;
			this.languageService = languageService;

			this.timer = new Timer((obj) => {
				CheckShapesToRemove();
			}, null, timerDelay, timerDelay);

			this.interactionService.InteractionOccured += InteractionService_InteractionOccured;

		}
	
		public void Dispose()
		{
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


		private ObservableCollection<BabyShape> _shapes = new ObservableCollection<BabyShape>();
		public ObservableCollection<BabyShape> Shapes
		{
			get
			{
				return _shapes;
			}
			set
			{
				SetField(ref _shapes, value);
			}
		}

	


		private void InteractionService_InteractionOccured(object sender, InteractionEventArgs e)
		{
			switch (e.Interaction) {
				case Models.InteractionType.MouseClick:
				break;
				case Models.InteractionType.MouseMove:
				break;
				case Models.InteractionType.KeyPress:
				ProcessKey(e.Key);
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
			Shapes.Clear();
		}

		private void ProcessKey(string key)
		{
			if(string.IsNullOrEmpty(key))
				return;

			//could be a letter or number
			if(key.Length == 1) {
				// If a letter was pressed, display the letter.
				if(Regex.IsMatch(key, @"^[a-zA-Z]+$"))
					AddLetter(key[0]);

				// If a number is pressed, display the number.
				if(Regex.IsMatch(key, @"^[0-9]+$")) {
					int number;
					if(int.TryParse(key, out number))
						AddNumber(int.Parse(key));
				}
			} else {
				// Otherwise, display a random shape.
				AddShape(new BabyShapeFigure());
			}

			CheckShapesToRemove();
		}

		private void CheckShapesToRemove()
		{
			if(Shapes.Count >= Settings.Default.ClearAfter)
				Shapes.RemoveAt(0);

			for(int i = Shapes.Count-1; i >= 0; i--) {
				var shape = Shapes[i];
				if(!shape.IsVisible) {
					Device.BeginInvokeOnMainThread(() => {
						Shapes.Remove(shape);
					});
				}
			}
		}

		private async void AddLetter(char letter)
		{
			AddShape(new BabyShapeLetter(letter));
			if(Settings.Default.Speak)
				await speakService.SpeakText(letter.ToString());
		}

		private void AddNumber(int number)
		{
			AddShape(new BabyShapeNumber(number));
		}

		private void AddShape(BabyShape shape)
		{

		    shape.StrokeColor = GetRandomColor();
			shape.FillColor = GetRandomColor();

			//TODO: what should this be
			var shapeWidth = Settings.Default.FontSize;
			var shapeHeight = Settings.Default.FontSize;

			//TODO: get this from DI
			var availableWidth = 1000;
			var availableHeight = 700;

			shape.Size = new Size(shapeWidth, shapeHeight);
			var x = RandomBetweenTwoNumbers(0, Convert.ToInt32(availableWidth - shape.Size.Width));
			var y = RandomBetweenTwoNumbers(0, Convert.ToInt32(availableHeight - shape.Size.Height));
			shape.Position = new Point(x, y);
    
			
			
			// var nameFunc = hashTableOfFigureGenerators[Utils.RandomBetweenTwoNumbers(0, hashTableOfFigureGenerators.Count - 1)];
			Shapes.Add(shape);
		}

		
	}
}
