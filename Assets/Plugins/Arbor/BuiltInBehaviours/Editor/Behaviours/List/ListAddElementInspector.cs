//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using Arbor.StateMachine.StateBehaviours;

	[CustomEditor(typeof(ListAddElement))]
	internal sealed class ListAddElementInspector : ListElementBaseInspector
	{
		protected override void OnGUI()
		{
			ElementGUI();
		}
	}
}