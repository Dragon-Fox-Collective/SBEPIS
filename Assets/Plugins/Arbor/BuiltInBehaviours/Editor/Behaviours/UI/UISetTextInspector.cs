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

	[CustomEditor(typeof(UISetText))]
	internal sealed class UISetTextInspector : InspectorBase
	{
		private FlexibleFieldProperty _StringProperty;
		private SerializedProperty _ChangeTimingUpdateProperty;

		protected override void OnRegisterElements()
		{
			_StringProperty = new FlexibleFieldProperty(serializedObject.FindProperty("_String"));
			_ChangeTimingUpdateProperty = serializedObject.FindProperty("_ChangeTimingUpdate");

			RegisterProperty("_Text");
			RegisterProperty(_StringProperty.property);
			RegisterIMGUI(OnParameterGUI);
		}

		private void OnParameterGUI()
		{
			FlexibleType type = _StringProperty.type;
			if (type == FlexibleType.Parameter)
			{
				EditorGUILayout.PropertyField(_ChangeTimingUpdateProperty);
			}
		}
	}
}
#endif