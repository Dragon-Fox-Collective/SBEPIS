//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;
	using ArborEditor.UnityEditorBridge.Extensions;

	internal abstract class FlexiblePrimitivePropertyEditor : OwnsDataSlotPropertyEditor
	{
		protected override DataSlotProperty slotProperty
		{
			get
			{
				return flexibleProperty.slotProperty;
			}
		}

		protected FlexiblePrimitiveProperty flexibleProperty
		{
			get;
			private set;
		}

		protected abstract FlexiblePrimitiveProperty CreateFlexibleProperty();

		private ParameterReferenceEditorGUI _ParameterReferenceEditorGUI = null;

		private bool _IsInGUI;

		void SetType(FlexiblePrimitiveType type)
		{
			DisableConnectionChanged();

			flexibleProperty.type = type;

			EnableConnectionChanged();
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();

			flexibleProperty = CreateFlexibleProperty();

			_ParameterReferenceEditorGUI = new ParameterReferenceEditorGUI(flexibleProperty.parameterProperty);
		}

		protected override void OnConnectionChanged(bool isConnect)
		{
			if (!property.IsValid())
			{
				return;
			}

			bool isInGUI = _IsInGUI || property.serializedObject.hasModifiedProperties;

			if (!isInGUI)
			{
				property.serializedObject.Update();
			}

			FlexiblePrimitiveType type = flexibleProperty.type;
			FlexiblePrimitiveType newType = type;

			if (isConnect)
			{
				newType = FlexiblePrimitiveType.DataSlot;
			}
			else if (type == FlexiblePrimitiveType.DataSlot && (ArborSettings.dataSlotShowMode == DataSlotShowMode.Outside || ArborSettings.dataSlotShowMode == DataSlotShowMode.Flexibly))
			{
				newType = FlexiblePrimitiveType.Constant;
			}

			if (type != newType)
			{
				SetType(newType);
			}

			if (!isInGUI)
			{
				property.serializedObject.ApplyModifiedProperties();
			}
		}

		protected virtual void OnConstantGUI(Rect position, GUIContent label)
		{
			EditorGUI.PropertyField(position, flexibleProperty.valueProperty, label, true);
		}

		protected virtual void OnParameterGUI(Rect position, GUIContent label)
		{
			EditorGUI.PropertyField(position, flexibleProperty.parameterProperty.property, label, true);
		}

		protected virtual void OnRandomGUI(Rect position, GUIContent label)
		{
			EditorGUI.LabelField(position, label);
		}

		protected virtual void OnDataSlotGUI(Rect position, GUIContent label)
		{
			EditorGUI.PropertyField(position, flexibleProperty.slotProperty.property, label, true);
		}

		protected virtual float GetConstantHeight(GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(flexibleProperty.valueProperty, label, true);
		}

		protected virtual float GetParameterHeight(GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(flexibleProperty.parameterProperty.property, label, true);
		}

		protected virtual float GetRandomHeight(GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		protected virtual float GetDataSlotHeight(GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(flexibleProperty.slotProperty.property, label, true);
		}

		protected override void DoGUI(Rect position, GUIContent label)
		{
			_IsInGUI = true;

			label = EditorGUI.BeginProperty(position, label, property);

			FlexiblePrimitiveType type = flexibleProperty.type;

			Rect fieldAreaPosition = position;

			fieldAreaPosition.height = GetFieldAreaHeight(label);

			position.yMin += fieldAreaPosition.height;

			Rect fieldPosition = EditorGUITools.SubtractDropdownWidth(fieldAreaPosition);

			int targetInstanceID = property.serializedObject.targetObject.GetInstanceID();
			BehaviourEditorGUI editorGUI = BehaviourEditorGUI.Get(targetInstanceID);
			if (editorGUI != null)
			{
				if (flexibleProperty.IsShowOutsideSlot())
				{
					editorGUI.SetInputSlotLink(fieldPosition, flexibleProperty.slotProperty.property);
				}
			}

			switch (type)
			{
				case FlexiblePrimitiveType.Constant:
					OnConstantGUI(fieldPosition, label);
					break;
				case FlexiblePrimitiveType.Parameter:
					OnParameterGUI(fieldPosition, label);
					break;
				case FlexiblePrimitiveType.Random:
					OnRandomGUI(fieldPosition, label);
					break;
				case FlexiblePrimitiveType.DataSlot:
					OnDataSlotGUI(fieldPosition, label);
					break;
			}

			Rect popupRect = EditorGUITools.GetDropdownRect(fieldAreaPosition);

			EditorGUI.BeginChangeCheck();
			FlexiblePrimitiveType newType = EditorGUITools.EnumPopupUnIndent(popupRect, GUIContent.none, type, BuiltInStyles.shurikenDropDown);
			if (EditorGUI.EndChangeCheck())
			{
				SetType(newType);
			}

			int indentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel++;

			if (type != FlexiblePrimitiveType.Parameter)
			{
				EditorGUI.BeginChangeCheck();
				_ParameterReferenceEditorGUI.DropParameter(position);
				if (EditorGUI.EndChangeCheck())
				{
					SetType(FlexiblePrimitiveType.Parameter);
				}
			}

			EditorGUI.indentLevel = indentLevel;

			EditorGUI.EndProperty();

			_IsInGUI = false;
		}

		float GetFieldAreaHeight(GUIContent label)
		{
			float height = EditorGUIUtility.singleLineHeight;

			FlexiblePrimitiveType type = flexibleProperty.type;

			switch (type)
			{
				case FlexiblePrimitiveType.Constant:
					height = GetConstantHeight(label);
					break;
				case FlexiblePrimitiveType.Parameter:
					height = GetParameterHeight(label);
					break;
				case FlexiblePrimitiveType.Random:
					height = GetRandomHeight(label);
					break;
				case FlexiblePrimitiveType.DataSlot:
					height = GetDataSlotHeight(label);
					break;
			}

			return height;
		}

		protected override float GetHeight(GUIContent label)
		{
			float height = GetFieldAreaHeight(label);

			FlexiblePrimitiveType type = flexibleProperty.type;

			if (type != FlexiblePrimitiveType.Parameter)
			{
				height += _ParameterReferenceEditorGUI.GetDropParameterHeight();
			}

			return height;
		}
	}
}