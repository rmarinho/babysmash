using BabySmash.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabySmash.Core.Services
{
	public interface IInteractionService
	{
		event EventHandler<InteractionEventArgs> InteractionOccured;
		void HandleMouseDown();
		void HandleMouseUp();

		bool IsHardKeyboardPresent
		{
			get;
		}
	}

	public class InteractionEventArgs : EventArgs
	{
		public InteractionEventArgs(InteractionType interaction)
		{
			this.Interaction = interaction;
		}
		public InteractionType Interaction
		{
			get; set;
		}

		public string Key
		{
			get; set;
		}
		
	}
}