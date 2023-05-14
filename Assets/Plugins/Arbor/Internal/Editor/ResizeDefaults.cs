//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArborEditor
{
	internal static class ResizeDefaults
	{
		public const int edgeSize = 8;
		public static readonly RectOffset offset = new RectOffset(edgeSize, edgeSize, 0, 0);
		public static readonly RectOffset offsetZero = new RectOffset();
	}
}