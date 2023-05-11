//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;
using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(MoveGameObjectToScene))]
	internal sealed class MoveGameObjectToSceneInspector : InspectorBase
	{
		protected override void OnRegisterElements()
		{
			RegisterProperty("_GameObject");
			RegisterProperty("_SceneName");
		}
	}
}