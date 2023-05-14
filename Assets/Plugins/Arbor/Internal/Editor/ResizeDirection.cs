//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace ArborEditor
{
	[System.Flags]
	public enum ResizeDirection
	{
		Top = 0x01,
		Bottom = 0x02,
		Left = 0x04,
		Right = 0x08,

		TopLeft = Top | Left,
		TopRight = Top | Right,
		BottomLeft = Bottom | Left,
		BottomRight = Bottom | Right,
	}
}