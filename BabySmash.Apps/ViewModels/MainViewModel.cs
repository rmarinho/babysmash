using BabySmash.Core.Models;
using BabySmash.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace BabySmash.Core.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		private IInteractionService interactionService;
		public MainViewModel(IInteractionService interactionService)
		{
			if (interactionService == null)
				throw new ArgumentNullException(nameof(interactionService));

			this.interactionService = interactionService;
			this.interactionService.InteractionOccured += InteractionService_InteractionOccured;
		}

		private void InteractionService_InteractionOccured(object sender, InteractionEventArgs e)
		{
			switch (e.Interaction) {
				case Models.InteractionType.MouseClick:
				AddShape();
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
				return;
			}

			if (key.ToLower().StartsWith("number")) {
			}
			// Otherwise, display a random shape.
			AddShape();

			if (Shapes.Count >= Settings.Default.ClearAfter) {
				Shapes.RemoveAt(0);
			}
		}

		private void AddLetter(char letter)
		{
			Shapes.Add(new BabyShapeLetter(letter));
		}

		private void AddNumber(int number)
		{
			Shapes.Add(new BabyShapeNumber(number));
		}

		private void AddShape()
		{
			Shapes.Add(new BabyShapeFigure());
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
