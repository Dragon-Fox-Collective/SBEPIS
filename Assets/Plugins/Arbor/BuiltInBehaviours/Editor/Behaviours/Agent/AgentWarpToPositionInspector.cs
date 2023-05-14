//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	[CustomEditor(typeof(AgentWarpToPosition))]
	internal sealed class AgentWarpToPositionInspector : AgentBaseInspector
	{
		private SerializedProperty _TargetProperty;

		protected override void OnRegisterElements()
		{
			base.OnRegisterElements();

			RegisterProperty("_Target");
		}
	}
}