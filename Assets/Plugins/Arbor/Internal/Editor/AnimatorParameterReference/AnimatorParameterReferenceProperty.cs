//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	public sealed class AnimatorParameterReferenceProperty
	{
		private const string kAnimatorPath = "_Animator";
		private const string kNamePath = "_Name._Name";
		private const string kTypePath = "type";

		private FlexibleComponentProperty _Animator;
		private SerializedProperty _Name;
		private SerializedProperty _Type;

		public SerializedProperty property
		{
			get;
			private set;
		}

		public FlexibleComponentProperty animatorProperty
		{
			get
			{
				if (_Animator == null)
				{
					_Animator = new FlexibleComponentProperty(property.FindPropertyRelative(kAnimatorPath));
				}
				return _Animator;
			}
		}

		public SerializedProperty nameProperty
		{
			get
			{
				if (_Name == null)
				{
					_Name = property.FindPropertyRelative(kNamePath);
				}
				return _Name;
			}
		}

		public SerializedProperty typeProperty
		{
			get
			{
				if (_Type == null)
				{
					_Type = property.FindPropertyRelative(kTypePath);
				}
				return _Type;
			}
		}

		public AnimatorControllerParameterType type
		{
			get
			{
				return EnumUtility.GetValueFromIndex<AnimatorControllerParameterType>(typeProperty.enumValueIndex);
			}
			set
			{
				typeProperty.enumValueIndex = EnumUtility.GetIndexFromValue(value);
			}
		}

		public AnimatorParameterReferenceProperty(SerializedProperty property)
		{
			this.property = property;
		}
	}
}