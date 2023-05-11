//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

using Arbor;
using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(AnimatorSetLayerWeight))]
	internal sealed class AnimatorSetLayerWeightInspector : InspectorBase
	{
		FlexibleComponentProperty _AnimatorProperty;
		FlexibleFieldProperty _LayerNameProperty;

		protected override void OnRegisterElements()
		{
			_AnimatorProperty = new FlexibleComponentProperty(serializedObject.FindProperty("_Animator"));
			_LayerNameProperty = new FlexibleFieldProperty(serializedObject.FindProperty("_LayerName"));

			RegisterProperty(_AnimatorProperty.property);
			RegisterIMGUI(OnLayerGUI);
			RegisterProperty("_Weight");
		}

		void OnLayerGUI()
		{
			if (_AnimatorProperty.type == FlexibleSceneObjectType.Constant)
			{
				Animator animator = _AnimatorProperty.valueProperty.objectReferenceValue as Animator;
				AnimatorController animatorController = AnimatorGUITools.GetAnimatorController(animator);

				AnimatorGUITools.AnimatorLayerField(animatorController, _LayerNameProperty);
			}
			else
			{
				EditorGUILayout.PropertyField(_LayerNameProperty.property);
			}
		}
	}
}
