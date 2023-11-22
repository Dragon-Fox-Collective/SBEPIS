using System;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Commands
{
	public abstract class Command : MonoBehaviour
	{
		public abstract bool Interpret(ArraySegment<Note> slice);
	}

	public static class NoteSegmentEx
	{
		public static (bool, ArraySegment<Note>) Eat(this ArraySegment<Note> slice, IList<Note> pattern)
		{
			return slice.StartsWith(pattern) ? (true, slice[pattern.Count..]) : (false, slice);
		}
	}
}
