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

	[CustomEditor(typeof(UISetTextFromParameter))]
	internal sealed class UISetTextFromParameterInspector : InspectorBase
	{
		private ParameterReferenceProperty _ParameterReferenceProperty;
		private SerializedProperty _FormatProperty;

		protected override void OnRegisterElements()
		{
			_ParameterReferenceProperty = new ParameterReferenceProperty(serializedObject.FindProperty("_Parameter"));
			_FormatProperty = serializedObject.FindProperty("_Format");

			RegisterProperty("_Text");
			RegisterProperty(_ParameterReferenceProperty.property);
			RegisterIMGUI(OnFormatGUI);
			RegisterProperty("_ChangeTimingUpdate");
		}

		static bool HasFormatString(Parameter.Type type)
		{
			switch (type)
			{
				case Parameter.Type.Int:
				case Parameter.Type.Long:
				case Parameter.Type.Float:
					return true;
			}

			return false;
		}

		void OnFormatGUI()
		{
			bool useFormat = false;
			ParameterReferenceType parameterReferenceType = _ParameterReferenceProperty.type;
			if (parameterReferenceType == ParameterReferenceType.DataSlot)
			{
				useFormat = true;
			}
			else
			{
				Parameter parameter = _ParameterReferenceProperty.GetParameter();
				useFormat = parameter != null && HasFormatString(parameter.type);
			}

			if (useFormat)
			{
				EditorGUILayout.PropertyField(_FormatProperty);
			}
		}
	}
}
#endif
