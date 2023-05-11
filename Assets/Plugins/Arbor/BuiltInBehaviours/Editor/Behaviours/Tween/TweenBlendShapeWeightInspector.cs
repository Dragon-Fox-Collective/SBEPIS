//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using Arbor;

	internal sealed class BlendShapePropertyEditor : PropertyEditor
	{
		public float elementHeight;

		private SerializedProperty _TargetProperty;
		private SerializedProperty _NameProperty;
		private FlexibleEnumProperty<TweenMoveType> _TweenMoveTypeProperty;
		private FlexibleNumericProperty _FromProperty;
		private SerializedProperty _ToProperty;

		protected override void OnInitialize()
		{
			base.OnInitialize();

			_TargetProperty = property.FindPropertyRelative("_Target");
			_NameProperty = property.FindPropertyRelative("_Name");
			_TweenMoveTypeProperty = new FlexibleEnumProperty<TweenMoveType>(property.FindPropertyRelative("_TweenMoveType"));
			_FromProperty = new FlexibleNumericProperty(property.FindPropertyRelative("_From"));
			_ToProperty = property.FindPropertyRelative("_To");
		}

		Rect DrawGUI(Rect position, GUIContent label, bool isDraw)
		{
			label = EditorGUI.BeginProperty(position, label, property);

			position.height = elementHeight;
			if (isDraw)
			{
				EditorGUI.PrefixLabel(position, label);
			}
			position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

			position.height = EditorGUI.GetPropertyHeight(_TargetProperty);
			if (isDraw)
			{
				EditorGUI.PropertyField(position, _TargetProperty);
			}
			position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

			position.height = EditorGUI.GetPropertyHeight(_NameProperty);
			if (isDraw)
			{
				EditorGUI.PropertyField(position, _NameProperty);
			}
			position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

			FlexibleType tweenMoveTypeFlexibleType = _TweenMoveTypeProperty.type;
			TweenMoveType tweenMoveType = _TweenMoveTypeProperty.value;

			position.height = EditorGUI.GetPropertyHeight(_TweenMoveTypeProperty.property);

			if (isDraw)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.PropertyField(position, _TweenMoveTypeProperty.property);
				if (EditorGUI.EndChangeCheck())
				{
					FlexibleType newTweenMoveTypeFlexibleType = _TweenMoveTypeProperty.type;
					TweenMoveType newTweenMoveType = _TweenMoveTypeProperty.value;
					if (tweenMoveTypeFlexibleType != newTweenMoveTypeFlexibleType || tweenMoveType != newTweenMoveType)
					{
						if (newTweenMoveTypeFlexibleType == FlexibleType.Constant && newTweenMoveType == TweenMoveType.ToAbsolute)
						{
							_FromProperty.Disconnect();

							property.serializedObject.ApplyModifiedProperties();

							GUIUtility.ExitGUI();
						}
					}

					tweenMoveTypeFlexibleType = newTweenMoveTypeFlexibleType;
					tweenMoveType = newTweenMoveType;
				}
			}
			position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

			if (tweenMoveTypeFlexibleType == FlexibleType.Constant)
			{
				switch (tweenMoveType)
				{
					case TweenMoveType.Absolute:
					case TweenMoveType.Relative:
						{
							position.height = EditorGUI.GetPropertyHeight(_FromProperty.property);
							if (isDraw)
							{
								EditorGUI.PropertyField(position, _FromProperty.property);
							}
							position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
						}
						break;
					case TweenMoveType.ToAbsolute:
						break;
				}
			}
			else
			{
				position.height = EditorGUI.GetPropertyHeight(_FromProperty.property);
				if (isDraw)
				{
					EditorGUI.PropertyField(position, _FromProperty.property);
				}
				position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
			}

			position.height = EditorGUI.GetPropertyHeight(_ToProperty);
			if (isDraw)
			{
				EditorGUI.PropertyField(position, _ToProperty);
			}
			position.y += position.height;

			EditorGUI.EndProperty();

			return position;
		}

		protected override void OnGUI(Rect position, GUIContent label)
		{
			DrawGUI(position, label, true);
		}

		protected override float GetHeight(GUIContent label)
		{
			return DrawGUI(new Rect(), label, false).y;
		}
	}

	[CustomEditor(typeof(TweenBlendShapeWeight))]
	internal sealed class TweenBlendShapeWeightInspector : TweenBaseInspector
	{
		private ReorderableList _BlendShapeList;
		private SerializedProperty _BlendShapesProperty;

		protected override void OnEnable()
		{
			base.OnEnable();

			_BlendShapesProperty = serializedObject.FindProperty("_BlendShapes");

			_BlendShapeList = new ReorderableList(serializedObject, _BlendShapesProperty)
			{
				drawHeaderCallback = DrawHeader,
				elementHeightCallback = GetElementHeight,
				drawElementCallback = DrawElement,
			};
		}

		void DrawHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, _BlendShapeList.serializedProperty.displayName);
		}

		BlendShapePropertyEditor GetPropertyEditor(int index)
		{
			SerializedProperty blendShapeProperty = _BlendShapeList.serializedProperty.GetArrayElementAtIndex(index);

			System.Type fieldType;
			var fieldInfo = SerializedPropertyUtility.GetFieldInfoFromProperty(blendShapeProperty, out fieldType);
			BlendShapePropertyEditor propertyEditor = PropertyEditorUtility<BlendShapePropertyEditor>.GetPropertyEditor(blendShapeProperty, fieldInfo);
			if (propertyEditor != null)
			{
				propertyEditor.elementHeight = _BlendShapeList.elementHeight;
			}
			return propertyEditor;
		}

		float GetElementHeight(int index)
		{
			BlendShapePropertyEditor propertyEditor = GetPropertyEditor(index);

			if (propertyEditor != null)
			{
				return propertyEditor.DoGetHeight(null);
			}
			else
			{
				return EditorGUIUtility.singleLineHeight;
			}
		}

		void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			BlendShapePropertyEditor propertyEditor = GetPropertyEditor(index);

			propertyEditor.DoOnGUI(rect, null);
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			DrawBase();

			EditorGUILayout.Space();

			_BlendShapeList.DoLayoutList();

			serializedObject.ApplyModifiedProperties();
		}
	}
}