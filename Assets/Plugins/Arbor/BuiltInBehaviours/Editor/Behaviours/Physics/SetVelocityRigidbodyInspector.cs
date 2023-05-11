//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using Arbor;
	using Arbor.StateMachine.StateBehaviours;
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(SetVelocityRigidbody))]
	internal sealed class SetVelocityRigidbodyInspector : InspectorBase
	{
		private FlexibleEnumProperty<DirectionType> _DirectionTypeProperty;
		private SerializedProperty _DirectionProperty;
		private SerializedProperty _SpeedProperty;
		private SerializedProperty _SpaceProperty;

		protected override void OnRegisterElements()
		{
			_DirectionTypeProperty = new FlexibleEnumProperty<DirectionType>(serializedObject.FindProperty("_DirectionType"));
			_DirectionProperty = serializedObject.FindProperty("_Direction");

			RegisterProperty("_Target");
			RegisterProperty("_ExecuteMethodFlags");
			RegisterProperty(_DirectionTypeProperty.property);
			RegisterIMGUI(OnDirectionGUI);
			RegisterProperty("_Speed");
			RegisterProperty("_Space");
		}

		void OnDirectionGUI()
		{
			GUIContent directionContent = null;

			int indentLevel = EditorGUI.indentLevel;
			if (_DirectionTypeProperty.type == FlexibleType.Constant)
			{
				DirectionType directionType = _DirectionTypeProperty.value;

				switch (directionType)
				{
					case DirectionType.EulerAngle:
						directionContent = GUIContentCaches.Get("Angle");
						break;
					case DirectionType.Vector:
						directionContent = GUIContentCaches.Get("Direction");
						break;
				}
				EditorGUI.indentLevel++;
			}

			EditorGUILayout.PropertyField(_DirectionProperty, directionContent);

			EditorGUI.indentLevel = indentLevel;
		}
	}
}
