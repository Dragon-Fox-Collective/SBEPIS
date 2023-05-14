//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.StateMachine.StateBehaviours.ObjectPooling
{
	using Arbor.StateMachine.StateBehaviours.ObjectPooling;
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(AdvancedPooling))]
	internal sealed class AdvancedPoolingInspector : InspectorBase
	{
		// Paths
		protected override void OnRegisterElements()
		{
			RegisterProperty("_PoolingItems");
		}
	}
}
