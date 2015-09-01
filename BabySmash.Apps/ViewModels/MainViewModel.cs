using BabySmash.Core.Models;
using BabySmash.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace BabySmash.Core.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		private IInteractionService interactionService;
		private ISpeakService speakService;
		public MainViewModel(IInteractionService interactionService, ISpeakService speakService)
		{
			if (interactionService == null)
				throw new ArgumentNullException(nameof(interactionService));

			if (speakService == null)
				throw new ArgumentNullException(nameof(speakService));

			this.interactionService = interactionService;
			this.interactionService.InteractionOccured += InteractionService_InteractionOccured;

			this.speakService = speakService;
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
			if (string.IsNullOrEmpty(key))
				return;

			//could be a letter or number
			if (key.Length == 1) {
				// If a letter was pressed, display the letter.
				if (Regex.IsMatch(key, @"^[a-zA-Z]+$"))
					AddLetter(key[0]);

				// If a number is pressed, display the number.
				if (Regex.IsMatch(key, @"^[0-9]+$")) {
					int number;
					if (int.TryParse(key, out number))
						AddNumber(int.Parse(key));
				}
			} else {
				// Otherwise, display a random shape.
				AddShape(new BabyShapeFigure());
			}
			
			if (Shapes.Count >= Settings.Default.ClearAfter) {
				Shapes.RemoveAt(0);
			}
		}

		private async void AddLetter(char letter)
		{
			AddShape(new BabyShapeLetter(letter));
			await speakService.SpeakText(letter.ToString());
		}

		private void AddNumber(int number)
		{
			AddShape(new BabyShapeNumber(number));
		}

		private void AddShape(BabyShape shape)
		{

		    shape.StrokeColor = Utils.GetRandomColor();
			shape.FillColor = Utils.GetRandomColor();

			//TODO: what should this be
			var shapeWidth = 100;
			var shapeHeight = 100;

			//TODO: get this from DI
			var availableWidth = 1000;
			var availableHeight = 700;

			shape.Size = new Size(shapeWidth, shapeHeight);
			var x = Utils.RandomBetweenTwoNumbers(0, Convert.ToInt32(availableWidth - shape.Size.Width));
			var y = Utils.RandomBetweenTwoNumbers(0, Convert.ToInt32(availableHeight - shape.Size.Height));
			shape.Position = new Point(x, y);
    
			
			
			// var nameFunc = hashTableOfFigureGenerators[Utils.RandomBetweenTwoNumbers(0, hashTableOfFigureGenerators.Count - 1)];
           //Canvas.SetLeft(f, Utils.RandomBetweenTwoNumbers(0, Convert.ToInt32(window.ActualWidth - f.Width)));
            // Canvas.SetTop(f, Utils.RandomBetweenTwoNumbers(0, Convert.ToInt32(window.ActualHeight - f.Height)));
			Shapes.Add(shape);
		}
		

		string _helloMessage ="BabySmash! by Scott Hanselman with community contributions! - http://www.babysmash.com ";
		public string HelloMessage
		{
			get
			{
				return _helloMessage;
			}
			set
			{
				SetField(ref _helloMessage, value);
			}
		}


		ObservableCollection<BabyShape> _shapes = new ObservableCollection<BabyShape>();
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
	}
}
