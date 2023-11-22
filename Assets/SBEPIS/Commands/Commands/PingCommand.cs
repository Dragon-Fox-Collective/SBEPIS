using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Commands.Commands
{
	public class PingCommand : Command
	{
		[SerializeField]
		private UnityEvent onPing = new();
		public event UnityAction OnPing
		{
			add => onPing.AddListener(value);
			remove => onPing.RemoveListener(value);
		}
		
		private static readonly Note[] Pattern = {Notes.C4, Notes.D4, Notes.E4};
		
		protected override void InterpretInternal(ArraySegment<Note> slice)
		{
			slice.Eat(Pattern);
			onPing.Invoke();
		}
	}
}