//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

using Arbor;

namespace ArborEditor
{
	[CustomEditor(typeof(AgentController))]
	internal sealed class AgentControllerInspector : Editor
	{
		SerializedProperty _Agent;
		SerializedProperty _Animator;
		SerializedProperty _MovingParameter;
		SerializedProperty _MovingSpeedThreshold;
		SerializedProperty _SpeedParameter;
		SerializedProperty _SpeedType;
		SerializedProperty _SpeedDivValue;
		SerializedProperty _SpeedDampTime;
		SerializedProperty _MovementType;
		SerializedProperty _MovementDivValue;
		SerializedProperty _MovementXParameter;
		SerializedProperty _MovementXDampTime;
		SerializedProperty _MovementYParameter;
		SerializedProperty _MovementYDampTime;
		SerializedProperty _MovementZParameter;
		SerializedProperty _MovementZDampTime;
		SerializedProperty _TurnParameter;
		SerializedProperty _TurnType;
		SerializedProperty _TurnDampTime;
		SerializedProperty _EnableTraverseOffMeshLink;
		OffMeshLinkTraverseDataProperty _JumpAcross;
		OffMeshLinkTraverseDataProperty _DropDown;

		void OnEnable()
		{
			_Agent = serializedObject.FindProperty("_Agent");
			_Animator = serializedObject.FindProperty("_Animator");
			_MovingParameter = serializedObject.FindProperty("_MovingParameter");
			_MovingSpeedThreshold = serializedObject.FindProperty("_MovingSpeedThreshold");
			_SpeedParameter = serializedObject.FindProperty("_SpeedParameter");
			_SpeedType = serializedObject.FindProperty("_SpeedType");
			_SpeedDivValue = serializedObject.FindProperty("_SpeedDivValue");
			_SpeedDampTime = serializedObject.FindProperty("_SpeedDampTime");
			_MovementType = serializedObject.FindProperty("_MovementType");
			_MovementDivValue = serializedObject.FindProperty("_MovementDivValue");
			_MovementXParameter = serializedObject.FindProperty("_MovementXParameter");
			_MovementXDampTime = serializedObject.FindProperty("_MovementXDampTime");
			_MovementYParameter = serializedObject.FindProperty("_MovementYParameter");
			_MovementYDampTime = serializedObject.FindProperty("_MovementYDampTime");
			_MovementZParameter = serializedObject.FindProperty("_MovementZParameter");
			_MovementZDampTime = serializedObject.FindProperty("_MovementZDampTime");
			_TurnParameter = serializedObject.FindProperty("_TurnParameter");
			_TurnType = serializedObject.FindProperty("_TurnType");
			_TurnDampTime = serializedObject.FindProperty("_TurnDampTime");
			_EnableTraverseOffMeshLink = serializedObject.FindProperty("_EnableTraverseOffMeshLink");
			_JumpAcross = new OffMeshLinkTraverseDataProperty(serializedObject.FindProperty("_JumpAcross"));
			_DropDown = new OffMeshLinkTraverseDataProperty(serializedObject.FindProperty("_DropDown"));
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(_Agent);

			EditorGUILayout.PropertyField(_Animator);

			Animator animator = _Animator.objectReferenceValue as Animator;
			AnimatorController animatorController = AnimatorGUITools.GetAnimatorController(animator);

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Moving", EditorStyles.boldLabel);
			_MovingParameter.SetStateData(animatorController);
			EditorGUILayout.PropertyField(_MovingParameter);
			EditorGUILayout.PropertyField(_MovingSpeedThreshold);

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Speed", EditorStyles.boldLabel);
			_SpeedParameter.SetStateData(animatorController);
			EditorGUILayout.PropertyField(_SpeedParameter);
			EditorGUILayout.PropertyField(_SpeedType);
			if(EnumUtility.GetValueFromIndex<AgentController.SpeedType>(_SpeedType.enumValueIndex) == AgentController.SpeedType.DivValue)
			{
				EditorGUILayout.PropertyField(_SpeedDivValue);
			}
			EditorGUILayout.PropertyField(_SpeedDampTime);

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Movement", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(_MovementType);
			if(EnumUtility.GetValueFromIndex<AgentController.MovementType>(_MovementType.enumValueIndex) == AgentController.MovementType.DivValue)
			{
				EditorGUILayout.PropertyField(_MovementDivValue);
			}

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("MovementX", EditorStyles.boldLabel);
			_MovementXParameter.SetStateData(animatorController);
			EditorGUILayout.PropertyField(_MovementXParameter);
			EditorGUILayout.PropertyField(_MovementXDampTime);

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("MovementY", EditorStyles.boldLabel);
			_MovementYParameter.SetStateData(animatorController);
			EditorGUILayout.PropertyField(_MovementYParameter);
			EditorGUILayout.PropertyField(_MovementYDampTime);

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("MovementZ", EditorStyles.boldLabel);
			_MovementZParameter.SetStateData(animatorController);
			EditorGUILayout.PropertyField(_MovementZParameter);
			EditorGUILayout.PropertyField(_MovementZDampTime);

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Turn", EditorStyles.boldLabel);
			_TurnParameter.SetStateData(animatorController);
			EditorGUILayout.PropertyField(_TurnParameter);
			EditorGUILayout.PropertyField(_TurnType);
			EditorGUILayout.PropertyField(_TurnDampTime);

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Traverse Off Mesh Link", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(_EnableTraverseOffMeshLink);
			if (_EnableTraverseOffMeshLink.boolValue)
			{
				_JumpAcross.parameterProperty.property.SetStateData(animatorController);
				EditorGUILayout.PropertyField(_JumpAcross.property);

				_DropDown.parameterProperty.property.SetStateData(animatorController);
				EditorGUILayout.PropertyField(_DropDown.property);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}