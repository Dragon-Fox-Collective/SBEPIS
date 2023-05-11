//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ARBOR_SUPPORT_UGUI
using UnityEditor;

using Arbor;
using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(UISetToggle))]
	internal sealed class UISetToggleInspector : InspectorBase
	{
		FlexibleBoolProperty _ValueProperty;
		SerializedProperty _ChangeTimingUpdateProperty;

		protected override void OnRegisterElements()
		{
			_ValueProperty = new FlexibleBoolProperty(serializedObject.FindProperty("_Value"));
			_ChangeTimingUpdateProperty = serializedObject.FindProperty("_ChangeTimingUpdate");

			RegisterProperty("_Toggle");
			RegisterProperty(_ValueProperty.property);
			RegisterIMGUI(OnParameterGUI);
		}

		void OnParameterGUI()
		{
			FlexiblePrimitiveType type = _ValueProperty.type;
			if (type == FlexiblePrimitiveType.Parameter)
			{
				EditorGUILayout.PropertyField(_ChangeTimingUpdateProperty);
			}
		}
	}
}
#endif