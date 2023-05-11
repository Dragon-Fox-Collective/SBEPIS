//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.StateMachine.StateBehaviours
{
	public class AgentRotateBaseInspector : AgentIntervalUpdateInspector
	{
		SerializedProperty _AngularSpeedProperty;

		protected override void OnRegisterElements()
		{
			base.OnRegisterElements();

			RegisterSpace();
			RegisterProperty("_AngularSpeed");
		}
	}
}