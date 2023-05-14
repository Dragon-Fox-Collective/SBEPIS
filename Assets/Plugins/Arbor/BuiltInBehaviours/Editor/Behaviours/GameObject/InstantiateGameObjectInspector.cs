//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;
using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(InstantiateGameObject))]
	internal sealed class InstantiateGameObjectInspector : InspectorBase
	{
		FlexibleEnumProperty<PostureType> _PostureTypeProperty;
		FlexibleFieldProperty _InitTransformProperty;
		FlexibleFieldProperty _InitPositionProperty;
		FlexibleFieldProperty _InitRotationProperty;
		FlexibleFieldProperty _InitSpaceProperty;
		FlexibleBoolProperty _UsePoolProperty;
		FlexibleFieldProperty _LifeTimeFlagsProperty;
		FlexibleNumericProperty _LifeDurationProperty;

		protected override void OnRegisterElements()
		{
			RegisterProperty("_Prefab");
			RegisterProperty("_Parent");

			_PostureTypeProperty = new FlexibleEnumProperty<PostureType>(serializedObject.FindProperty("_PostureType"));
			_InitTransformProperty = new FlexibleFieldProperty(serializedObject.FindProperty("_InitTransform"));
			_InitPositionProperty = new FlexibleFieldProperty(serializedObject.FindProperty("_InitPosition"));
			_InitRotationProperty = new FlexibleFieldProperty(serializedObject.FindProperty("_InitRotation"));
			_InitSpaceProperty = new FlexibleFieldProperty(serializedObject.FindProperty("_InitSpace"));

			_UsePoolProperty = new FlexibleBoolProperty(serializedObject.FindProperty("_UsePool"));
			_LifeTimeFlagsProperty = new FlexibleFieldProperty(serializedObject.FindProperty("_LifeTimeFlags"));
			_LifeDurationProperty = new FlexibleNumericProperty(serializedObject.FindProperty("_LifeDuration"));

			RegisterIMGUI(OnGUI);

			RegisterProperty("_Parameter");
			RegisterProperty("_Output");
		}

		void OnGUI()
		{
			FlexibleType postureFlexibleType = _PostureTypeProperty.type;
			PostureType postureType = _PostureTypeProperty.value;

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(_PostureTypeProperty.property);
			if (EditorGUI.EndChangeCheck())
			{
				FlexibleType newPostureFlexibleType = _PostureTypeProperty.type;
				PostureType newPostureType = _PostureTypeProperty.value;
				if (postureFlexibleType != newPostureFlexibleType || postureType != newPostureType)
				{
					if (newPostureFlexibleType == FlexibleType.Constant)
					{
						if (newPostureType != PostureType.Transform)
						{
							_InitTransformProperty.Disconnect();
						}
						if (newPostureType != PostureType.Directly)
						{
							_InitPositionProperty.Disconnect();
							_InitRotationProperty.Disconnect();
							_InitSpaceProperty.Disconnect();
						}

						serializedObject.ApplyModifiedProperties();

						GUIUtility.ExitGUI();
					}
				}

				postureFlexibleType = newPostureFlexibleType;
				postureType = newPostureType;
			}

			if (postureFlexibleType == FlexibleType.Constant)
			{
				int indentLevel = EditorGUI.indentLevel;
				EditorGUI.indentLevel++;

				switch (postureType)
				{
					case PostureType.Transform:
						{
							EditorGUILayout.PropertyField(_InitTransformProperty.property);
						}
						break;
					case PostureType.Directly:
						{
							EditorGUILayout.PropertyField(_InitPositionProperty.property);
							EditorGUILayout.PropertyField(_InitRotationProperty.property);
							EditorGUILayout.PropertyField(_InitSpaceProperty.property);
						}
						break;
				}

				EditorGUI.indentLevel = indentLevel;
			}
			else
			{
				int indentLevel = EditorGUI.indentLevel;

				EditorGUILayout.LabelField("PostureType: Transform", EditorStyles.miniBoldLabel);

				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(_InitTransformProperty.property);
				EditorGUI.indentLevel = indentLevel;

				EditorGUILayout.LabelField("PostureType: Directly", EditorStyles.miniBoldLabel);

				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(_InitPositionProperty.property);
				EditorGUILayout.PropertyField(_InitRotationProperty.property);
				EditorGUILayout.PropertyField(_InitSpaceProperty.property);
				EditorGUI.indentLevel = indentLevel;
			}

			FlexiblePrimitiveType usePoolType = _UsePoolProperty.type;
			bool usePool = _UsePoolProperty.valueProperty.boolValue;

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(_UsePoolProperty.property);
			if (EditorGUI.EndChangeCheck())
			{
				FlexiblePrimitiveType newUsePoolType = _UsePoolProperty.type;
				bool newUsePool = _UsePoolProperty.valueProperty.boolValue;
				if (usePoolType != newUsePoolType || usePool != newUsePool)
				{
					if (newUsePoolType == FlexiblePrimitiveType.Constant)
					{
						if (!newUsePool)
						{
							_LifeTimeFlagsProperty.Disconnect();
							_LifeDurationProperty.Disconnect();
						}

						serializedObject.ApplyModifiedProperties();

						GUIUtility.ExitGUI();
					}
				}

				usePoolType = newUsePoolType;
				usePool = newUsePool;
			}

			if (usePoolType != FlexiblePrimitiveType.Constant || usePool)
			{
				int indentLevel = EditorGUI.indentLevel;

				EditorGUI.indentLevel++;

				EditorGUILayout.PropertyField(_LifeTimeFlagsProperty.property);
				EditorGUILayout.PropertyField(_LifeDurationProperty.property);

				EditorGUI.indentLevel = indentLevel;
			}
		}
	}
}
