//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.StateMachine.StateBehaviours
{
	public class AgentMoveBaseInspector : AgentIntervalUpdateInspector
	{
		protected override void OnRegisterElements()
		{
			base.OnRegisterElements();

			RegisterSpace();
			RegisterProperty("_Speed");
		}
	}
}