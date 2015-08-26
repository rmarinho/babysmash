
using BabySmash.Core.Models;
using BabySmash.Core.Services;
using System;
using Windows.System;
using Windows.UI.Xaml;

namespace BabySmash.Windows.Services
{
	public class InteractionService : IInteractionService, IDisposable
	{
		private bool isCtrlKeyPressed;
		private bool disposed;
		private bool isKeyboardShowing;
		private bool isUsingSoftKeyboard;
		public InteractionService()
		{
			Window.Current.CoreWindow.KeyUp += KeyUp;
			Window.Current.CoreWindow.KeyDown += KeyDown;
		}

		private void KeyDown(global::Windows.UI.Core.CoreWindow sender, global::Windows.UI.Core.KeyEventArgs e)
		{
			char k = (char)0;
			if (e.VirtualKey == VirtualKey.Control)
				this.isCtrlKeyPressed = true;
			else if (isCtrlKeyPressed) {
				switch (e.VirtualKey) {
					case VirtualKey.X:
					OnInteractionOccured( new InteractionEventArgs(InteractionType.Exit) );
					break;
				}
			}
			
			if (e.VirtualKey >= VirtualKey.Number0 && e.VirtualKey <= VirtualKey.Number9)
            {
                k =   (char) ('0' + e.VirtualKey - VirtualKey.Number0);
            }

			if (e.VirtualKey >= VirtualKey.NumberPad0 && e.VirtualKey <= VirtualKey.NumberPad9)
            {
                k =   (char) ('0' + e.VirtualKey - VirtualKey.NumberPad0);
            }

         	OnInteractionOccured( new InteractionEventArgs(InteractionType.KeyPress) { Key = k == 0 ? e.VirtualKey.ToString() : k.ToString() });
		}


		private void KeyUp(global::Windows.UI.Core.CoreWindow sender, global::Windows.UI.Core.KeyEventArgs e)
		{
			 if (e.VirtualKey == VirtualKey.Control) isCtrlKeyPressed = true;
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

		public bool IsHardKeyboardPresent
		{
			get
			{
				var keyboardCaps = new global::Windows.Devices.Input.KeyboardCapabilities();
				return keyboardCaps.KeyboardPresent == 0;
			}
		}

		private void ShowsoftKeyboard(bool show)
		{
		    if (this.IsHardKeyboardPresent && show)
            {
				this.isUsingSoftKeyboard = true;
				//show softkeyboard
            }
		}

        void KeyboardShowing(global::Windows.UI.ViewManagement.InputPane sender, global::Windows.UI.ViewManagement.InputPaneVisibilityEventArgs args)
        {
        }

        void HekyboardHiding(global::Windows.UI.ViewManagement.InputPane sender, global::Windows.UI.ViewManagement.InputPaneVisibilityEventArgs args)
        {
        }

		
		public void Dispose()
		{
			if (!this.disposed) {
				this.disposed = true;
				Window.Current.CoreWindow.KeyUp -= KeyUp;
				Window.Current.CoreWindow.KeyDown -= KeyDown;
				if (this.isUsingSoftKeyboard) {
					//get rid of soft keyboard events
				}
			}
		}
	}
}
