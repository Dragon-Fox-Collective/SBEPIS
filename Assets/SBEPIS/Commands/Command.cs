using System;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Commands
{
	public abstract class Command : MonoBehaviour
	{
		public bool Interpret(ArraySegment<Note> slice)
		{
			try
			{
				InterpretInternal(slice);
				return true;
			}
			catch (NoteParsingFailed e)
			{
				return false;
			}
		}
		
		protected abstract void InterpretInternal(ArraySegment<Note> slice);
	}
	
	public class NoteParsingFailed : Exception {}
	
	public static class NoteSegmentEx
	{
		public static ArraySegment<Note> Eat(this ArraySegment<Note> slice, IList<Note> pattern)
		{
			return slice.StartsWith(pattern) ? slice[pattern.Count..] : throw new NoteParsingFailed();
		}
		
		public static (bool, ArraySegment<Note>) EatBool(this ArraySegment<Note> slice)
		{
			return
				slice.StartsWith(Notes.A4) ? (true, slice[1..]) :
				slice.StartsWith(Notes.C5) ? (false, slice[1..]) :
				throw new NoteParsingFailed();
		}
	}
}
