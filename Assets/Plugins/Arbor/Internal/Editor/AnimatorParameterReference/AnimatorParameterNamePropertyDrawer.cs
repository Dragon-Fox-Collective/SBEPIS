//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace ArborEditor
{
	using Arbor;
	[CustomPropertyDrawer(typeof(AnimatorParameterNameAttribute))]
	public class AnimatorParameterNamePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (fieldInfo.FieldType == typeof(AnimatorName))
			{
				AnimatorNameProperty animatorNameProperty = new AnimatorNameProperty(property);

				AnimatorController animatorController = property.GetStateData<AnimatorController>();
				if (animatorController != null)
				{
					AnimatorParameterNameAttribute attribute = this.attribute as AnimatorParameterNameAttribute;
					AnimatorGUITools.AnimatorParameterField(position, animatorController, animatorNameProperty.nameProperty, null, label, attribute.hasType, attribute.type);
				}
				else
				{
					EditorGUI.PropertyField(position, animatorNameProperty.nameProperty, label);
				}
			}
			else
			{
				EditorGUI.PropertyField(position, property, label);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (fieldInfo.FieldType == typeof(AnimatorName))
			{
				AnimatorNameProperty animatorNameProperty = new AnimatorNameProperty(property);
				AnimatorController animatorController = property.GetStateData<AnimatorController>();
				if (animatorController != null)
				{
					AnimatorParameterNameAttribute attribute = this.attribute as AnimatorParameterNameAttribute;
					return AnimatorGUITools.GetAnimatorParameterFieldHeight(animatorController, animatorNameProperty.nameProperty, null, attribute.hasType);
				}
				else
				{
					return EditorGUI.GetPropertyHeight(animatorNameProperty.nameProperty, label);
				}
			}
			else
			{
				return EditorGUI.GetPropertyHeight(property, label);
			}
		}
	}
}