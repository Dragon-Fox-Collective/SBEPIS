using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Commands.Commands
{
	public class KillCommand : Command
	{
		[SerializeField]
		private UnityEvent<bool> onKill = new();
		public event UnityAction<bool> OnKill
		{
			add => onKill.AddListener(value);
			remove => onKill.RemoveListener(value);
		}
		
		private static readonly Note[] Pattern = {Notes.D4, Notes.D4, Notes.D5};
		
		protected override void InterpretInternal(ArraySegment<Note> slice)
		{
			slice = slice.Eat(Pattern);
			(bool killPlayer, _) = slice.EatBool();
			onKill.Invoke(killPlayer);
		}
	}
}