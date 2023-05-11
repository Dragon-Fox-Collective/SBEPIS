//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using Arbor;

	public class AgentUpdateBaseInspector : AgentBaseInspector
	{
		protected override void OnRegisterElements()
		{
			base.OnRegisterElements();

			RegisterProperty("_StopOnStateEnd");
			RegisterProperty("_ClearVelocityOnStop");
		}
	}
}