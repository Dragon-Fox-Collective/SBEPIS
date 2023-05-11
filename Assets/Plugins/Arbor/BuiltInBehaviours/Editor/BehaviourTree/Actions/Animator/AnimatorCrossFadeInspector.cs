//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

using Arbor;
using Arbor.BehaviourTree.Actions;

namespace ArborEditor.BehaviourTree.Actions
{
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(AnimatorCrossFade))]
	internal sealed class AnimatorCrossFadeInspector : InspectorBase
	{
		FlexibleComponentProperty _AnimatorProperty;
		FlexibleFieldProperty _LayerNameProperty;
		FlexibleFieldProperty _StateNameProperty;

		protected override void OnRegisterElements()
		{
			_AnimatorProperty = new FlexibleComponentProperty(serializedObject.FindProperty("_Animator"));
			_LayerNameProperty = new FlexibleFieldProperty(serializedObject.FindProperty("_LayerName"));
			_StateNameProperty = new FlexibleFieldProperty(serializedObject.FindProperty("_StateName"));

			RegisterProperty(_AnimatorProperty.property);
			RegisterIMGUI(OnStateGUI);
			RegisterProperty("_InFixedTime");
			RegisterProperty("_TransitionDuration");
			RegisterProperty("_TimeOffset");
			RegisterProperty("_CheckInTransition");
			RegisterProperty("_WaitTransition");
			RegisterProperty("_CheckResult");
		}

		void OnStateGUI()
		{
			if (_AnimatorProperty.type == FlexibleSceneObjectType.Constant)
			{
				Animator animator = _AnimatorProperty.valueProperty.objectReferenceValue as Animator;
				AnimatorController animatorController = AnimatorGUITools.GetAnimatorController(animator);

				AnimatorGUITools.AnimatorStateField(animatorController, _LayerNameProperty, _StateNameProperty);
			}
			else
			{
				EditorGUILayout.PropertyField(_LayerNameProperty.property);
				EditorGUILayout.PropertyField(_StateNameProperty.property);
			}
		}
	}
}