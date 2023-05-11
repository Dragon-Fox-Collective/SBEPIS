//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using Arbor.StateMachine.StateBehaviours;

	[CustomEditor(typeof(ListRemoveAtIndex))]
	internal sealed class ListRemoveAtIndexInspector : ListBehaviourBaseInspector
	{
		private const string kIndexPath = "_Index";

		private SerializedProperty _IndexProperty;

		protected override void OnEnable()
		{
			base.OnEnable();

			_IndexProperty = serializedObject.FindProperty(kIndexPath);
		}

		protected override void OnGUI()
		{
			EditorGUILayout.PropertyField(_IndexProperty, true);
		}
	}
}