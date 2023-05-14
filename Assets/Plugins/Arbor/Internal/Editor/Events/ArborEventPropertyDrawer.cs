//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.Events
{
	using Arbor.Events;

	[CustomPropertyDrawer(typeof(ArborEvent))]
	internal sealed class ArborEventPropertyDrawer : PropertyEditorDrawer<CallListGUI>
	{
	}
}