//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace ArborEditor.Events
{
	[System.Flags]
	public enum MemberFilterFlags
	{
		Method = 1 << 0,
		Field = 1 << 1,
		ReadOnlyField = 1 << 2,
		SetProperty = 1 << 3,
		GetProperty = 1 << 4,
		Instance = 1 << 5,
		Static = 1 << 6,

		Fields = Field | ReadOnlyField,
		Property = SetProperty | GetProperty,
	}
}