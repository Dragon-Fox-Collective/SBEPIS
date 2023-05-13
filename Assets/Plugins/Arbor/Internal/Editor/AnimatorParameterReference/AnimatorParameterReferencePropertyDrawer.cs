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
	[CustomPropertyDrawer(typeof(AnimatorParameterReference), true)]
	internal sealed class AnimatorParameterReferencePropertyDrawer : PropertyDrawer
	{
		Rect DoGUI(Rect position, SerializedProperty property, GUIContent label, bool isDraw)
		{
			if (property.IsInvalidManagedReference())
			{
				position.height = EditorGUI.GetPropertyHeight(property, label);
				if (isDraw)
				{
					EditorGUI.PropertyField(position, property, label);
				}
				return position;
			}

			AnimatorParameterReferenceProperty referenceProperty = new AnimatorParameterReferenceProperty(property);

			Rect lineRect = new Rect(position);

			lineRect.height = EditorGUIUtility.singleLineHeight;

			if (isDraw)
			{
				EditorGUI.LabelField(lineRect, label);
			}

			lineRect.y += lineRect.height + EditorGUIUtility.standardVerticalSpacing;
			lineRect.height = 0f;

			int indentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel++;

			lineRect.height = EditorGUI.GetPropertyHeight(referenceProperty.animatorProperty.property);

			if (isDraw)
			{
				EditorGUI.PropertyField(lineRect, referenceProperty.animatorProperty.property);
			}

			lineRect.y += lineRect.height + EditorGUIUtility.standardVerticalSpacing;
			lineRect.height = 0f;

			System.Type elementType = Arbor.Serialization.SerializationUtility.ElementType(fieldInfo.FieldType);
			Arbor.Internal.AnimatorParameterTypeAttribute parameterTypeAttribute = AttributeHelper.GetAttribute<Arbor.Internal.AnimatorParameterTypeAttribute>(elementType);

			bool hasType = parameterTypeAttribute != null;
			AnimatorControllerParameterType parameterType = (parameterTypeAttribute != null) ? parameterTypeAttribute.parameterType : AnimatorControllerParameterType.Bool;

			Animator animator = referenceProperty.animatorProperty.constantValue as Animator;
			AnimatorController animatorController = AnimatorGUITools.GetAnimatorController(animator);
			lineRect.height = AnimatorGUITools.GetAnimatorParameterFieldHeight(animatorController, referenceProperty.nameProperty, referenceProperty.typeProperty, hasType);

			if (isDraw)
			{
				AnimatorGUITools.AnimatorParameterField(lineRect, animatorController, referenceProperty.nameProperty, referenceProperty.typeProperty, GUIContentCaches.Get("Parameter"), hasType, parameterType);
			}

			lineRect.y += lineRect.height + EditorGUIUtility.standardVerticalSpacing;
			lineRect.height = 0f;

			EditorGUI.indentLevel = indentLevel;

			position.yMax = Mathf.Max(position.yMax, lineRect.yMax - EditorGUIUtility.standardVerticalSpacing);

			return position;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			DoGUI(position, property, label, true);
			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			Rect position = DoGUI(new Rect(), property, label, false);

			return position.height;
		}
	}
}
