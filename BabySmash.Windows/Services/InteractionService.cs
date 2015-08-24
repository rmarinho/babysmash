
using BabySmash.Core.Models;
using BabySmash.Core.Services;
using System;
using Windows.System;
using Windows.UI.Xaml;

namespace BabySmash.Windows.Services
{
	public class InteractionService : IInteractionService
	{
		bool isCtrlKeyPressed;

		public InteractionService(UIElement page)
		{
			page.KeyUp += KeyUp;
			page.KeyDown += KeyDown;
		}

		private void KeyDown(object sender, global::Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
		{
			char k = (char)0;
			if (e.Key == VirtualKey.Control)
				this.isCtrlKeyPressed = true;
			else if (isCtrlKeyPressed) {
				switch (e.Key) {
					case VirtualKey.X:
					OnInteractionOccured( new InteractionEventArgs(InteractionType.Exit) );
					break;
				}
			}

		

			if (e.Key >= VirtualKey.Number0 && e.Key <= VirtualKey.Number9)
            {
                k =   (char) ('0' + e.Key - VirtualKey.Number0);
            }

			if (e.Key >= VirtualKey.NumberPad0 && e.Key <= VirtualKey.NumberPad9)
            {
                k =   (char) ('0' + e.Key - VirtualKey.NumberPad0);
            }

         	OnInteractionOccured( new InteractionEventArgs(InteractionType.KeyPress) { Key = k == 0 ? e.Key.ToString() : k.ToString() });
   
		}

		private void KeyUp(object sender, global::Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
		{
			 if (e.Key == VirtualKey.Control) isCtrlKeyPressed = true;
		}

		private void OnInteractionOccured(InteractionEventArgs eventArgs)
		{
			var handler = InteractionOccured;
			if (handler != null) {
				InteractionOccured(this, eventArgs);	
			}
		}

		public event EventHandler<InteractionEventArgs> InteractionOccured;

		public void HandleMouseDown()
		{
			throw new NotImplementedException();
		}

		public void HandleMouseUp()
		{
			throw new NotImplementedException();
		}
	}
}
