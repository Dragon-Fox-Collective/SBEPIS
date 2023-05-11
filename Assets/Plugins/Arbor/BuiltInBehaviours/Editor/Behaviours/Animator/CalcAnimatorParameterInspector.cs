//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(CalcAnimatorParameter))]
	internal sealed class CalcAnimatorParameterInspector : InspectorBase
	{
		private AnimatorParameterReferenceProperty _ReferenceProperty;
		private FlexibleEnumProperty<CalcAnimatorParameter.Function> _FunctionProperty;
		private FlexibleNumericProperty _FloatValueProperty;
		private FlexibleNumericProperty _IntValueProperty;
		private FlexibleBoolProperty _BoolValueProperty;

		protected override void OnRegisterElements()
		{
			_ReferenceProperty = new AnimatorParameterReferenceProperty(serializedObject.FindProperty("_Reference"));
			_FloatValueProperty = new FlexibleNumericProperty(serializedObject.FindProperty("_FloatValue"));
			_IntValueProperty = new FlexibleNumericProperty(serializedObject.FindProperty("_IntValue"));
			_BoolValueProperty = new FlexibleBoolProperty(serializedObject.FindProperty("_BoolValue"));
			_FunctionProperty = new FlexibleEnumProperty<CalcAnimatorParameter.Function>(serializedObject.FindProperty("_Function"));

			RegisterProperty("_ExecuteMethodFlags");
			RegisterIMGUI(OnGUI);
		}

		void OnGUI()
		{
			AnimatorControllerParameterType parameterType = _ReferenceProperty.type;

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(_ReferenceProperty.property);
			if (EditorGUI.EndChangeCheck())
			{
				AnimatorControllerParameterType newParameterType = _ReferenceProperty.type;
				if (parameterType != newParameterType)
				{
					switch (parameterType)
					{
						case AnimatorControllerParameterType.Bool:
							_BoolValueProperty.Disconnect();
							break;
						case AnimatorControllerParameterType.Float:
							_FloatValueProperty.Disconnect();
							break;
						case AnimatorControllerParameterType.Int:
							_IntValueProperty.Disconnect();
							break;
					}

					switch (newParameterType)
					{
						case AnimatorControllerParameterType.Bool:
							_FunctionProperty.Disconnect();
							break;
						case AnimatorControllerParameterType.Trigger:
							_FunctionProperty.Disconnect();
							break;
					}

					serializedObject.ApplyModifiedProperties();

					GUIUtility.ExitGUI();
				}
				parameterType = newParameterType;
			}

			switch (parameterType)
			{
				case AnimatorControllerParameterType.Float:
					{
						EditorGUILayout.PropertyField(_FunctionProperty.property);
						EditorGUILayout.PropertyField(_FloatValueProperty.property, GUIContentCaches.Get("Float Value"));
					}
					break;
				case AnimatorControllerParameterType.Int:
					{
						EditorGUILayout.PropertyField(_FunctionProperty.property);
						EditorGUILayout.PropertyField(_IntValueProperty.property, GUIContentCaches.Get("Int Value"));
					}
					break;
				case AnimatorControllerParameterType.Bool:
					{
						EditorGUILayout.PropertyField(_BoolValueProperty.property, GUIContentCaches.Get("Bool Value"));
					}
					break;
				case AnimatorControllerParameterType.Trigger:
					break;
			}
		}
	}
}
