using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Commands.Commands
{
	public class Ping : Command
	{
		[SerializeField]
		private UnityEvent onPing = new();
		
		private static readonly Note[] Pattern = {Notes.C4, Notes.D4, Notes.E4};
		
		public override bool Interpret(ArraySegment<Note> slice)
		{
			(bool ping, _) = slice.Eat(Pattern);
			if (ping)
				onPing.Invoke();
			return ping;
		}
	}
}