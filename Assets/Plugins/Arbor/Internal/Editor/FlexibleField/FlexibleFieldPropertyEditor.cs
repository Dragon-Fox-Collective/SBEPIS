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

	public class FlexibleFieldPropertyEditor : OwnsDataSlotPropertyEditor
	{
		private bool _IsInGUI;

		private static readonly int s_NotSupportTypeHash = "s_NotSupportTypeHash".GetHashCode();

		protected override DataSlotProperty slotProperty
		{
			get
			{
				return flexibleFieldProperty.slotProperty;
			}
		}

		protected FlexibleFieldProperty flexibleFieldProperty
		{
			get;
			private set;
		}

		private ParameterReferenceEditorGUI _ParameterReferenceEditorGUI = null;

		protected override void OnInitialize()
		{
			base.OnInitialize();

			flexibleFieldProperty = new FlexibleFieldProperty(property);

			_ParameterReferenceEditorGUI = new ParameterReferenceEditorGUI(flexibleFieldProperty.parameterProperty);
		}

		void SetType(FlexibleType type)
		{
			DisableConnectionChanged();

			flexibleFieldProperty.type = type;

			EnableConnectionChanged();
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

			FlexibleType type = flexibleFieldProperty.type;
			FlexibleType newType = type;

			if (isConnect)
			{
				newType = FlexibleType.DataSlot;
			}
			else if (type == FlexibleType.DataSlot && (ArborSettings.dataSlotShowMode == DataSlotShowMode.Outside || ArborSettings.dataSlotShowMode == DataSlotShowMode.Flexibly))
			{
				newType = FlexibleType.Constant;
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

		protected virtual void OnConstantGUI(Rect position, SerializedProperty valueProperty, GUIContent label)
		{
			EditorGUI.PropertyField(position, valueProperty, label, true);
		}

		protected virtual float GetConstantHeight(SerializedProperty valueProperty, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(valueProperty, label, true);
		}

		protected virtual void OnParameterGUI(Rect position, SerializedProperty parameterProperty, GUIContent label)
		{
			EditorGUI.PropertyField(position, parameterProperty, label, true);
		}

		protected virtual float GetParameterHeight(SerializedProperty parameterProperty, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(parameterProperty, label, true);
		}

		protected virtual void OnDataSlotGUI(Rect position, SerializedProperty slotProperty, GUIContent label)
		{
			EditorGUI.PropertyField(position, slotProperty, label, true);
		}

		protected virtual float GetDataSlotHeight(SerializedProperty slotProperty, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(slotProperty, label, true);
		}

		protected virtual void OnDropConstant(Rect position)
		{
		}

		protected virtual float GetDropConstantHeight()
		{
			return 0f;
		}

		protected override void DoGUI(Rect position, GUIContent label)
		{
			_IsInGUI = true;

			label = EditorGUI.BeginProperty(position, label, property);

			FlexibleType type = flexibleFieldProperty.type;

			Rect fieldAreaPosition = position;

			fieldAreaPosition.height = GetFieldAreaHeight(label);

			position.yMin += fieldAreaPosition.height;

			Rect fieldPosition = EditorGUITools.SubtractDropdownWidth(fieldAreaPosition);

			int targetInstanceID = property.serializedObject.targetObject.GetInstanceID();
			BehaviourEditorGUI editorGUI = BehaviourEditorGUI.Get(targetInstanceID);
			if (editorGUI != null)
			{
				if (flexibleFieldProperty.IsShowOutsideSlot())
				{
					editorGUI.SetInputSlotLink(fieldPosition, flexibleFieldProperty.slotProperty.property);
				}
			}

			switch (type)
			{
				case FlexibleType.Constant:
					SerializedProperty valueProperty = flexibleFieldProperty.valueProperty;
					if (valueProperty != null)
					{
						OnFlexibleConstantGUI onFlexibleConstantGUI = flexibleFieldProperty.property.GetStateData<OnFlexibleConstantGUI>();

						if (onFlexibleConstantGUI == null || !onFlexibleConstantGUI(fieldPosition, valueProperty, label))
						{
							OnConstantGUI(fieldPosition, valueProperty, label);
						}
					}
					else
					{
						int controlID = GUIUtility.GetControlID(s_NotSupportTypeHash, FocusType.Passive, position);

						Rect helpPosition = new Rect(fieldPosition);
						fieldPosition.height = EditorGUIUtility.singleLineHeight;
						helpPosition.yMin += fieldPosition.height;

						EditorGUI.PrefixLabel(fieldPosition, controlID, label);
						EditorGUI.HelpBox(helpPosition, Localization.GetWord("FlexibleField.NotSupportType"), MessageType.Error);
					}
					break;
				case FlexibleType.Parameter:
					OnParameterGUI(fieldPosition, flexibleFieldProperty.parameterProperty.property, label);
					break;
				case FlexibleType.DataSlot:
					OnDataSlotGUI(fieldPosition, flexibleFieldProperty.slotProperty.property, label);
					break;
			}

			Rect popupRect = EditorGUITools.GetDropdownRect(fieldAreaPosition);

			EditorGUI.BeginChangeCheck();
			FlexibleType newType = EditorGUITools.EnumPopupUnIndent(popupRect, GUIContent.none, type, BuiltInStyles.shurikenDropDown);
			if (EditorGUI.EndChangeCheck())
			{
				SetType(newType);
			}

			int indentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel++;

			if (type != FlexibleType.Constant)
			{
				EditorGUI.BeginChangeCheck();
				OnDropConstant(position);
				if (EditorGUI.EndChangeCheck())
				{
					SetType(FlexibleType.Constant);
				}
			}

			if (type != FlexibleType.Parameter)
			{
				EditorGUI.BeginChangeCheck();
				_ParameterReferenceEditorGUI.DropParameter(position);
				if (EditorGUI.EndChangeCheck())
				{
					SetType(FlexibleType.Parameter);
				}
			}

			EditorGUI.indentLevel = indentLevel;

			EditorGUI.EndProperty();

			_IsInGUI = false;
		}

		float GetFieldAreaHeight(GUIContent label)
		{
			float height = EditorGUIUtility.singleLineHeight;

			FlexibleType type = flexibleFieldProperty.type;

			switch (type)
			{
				case FlexibleType.Constant:
					SerializedProperty valueProperty = flexibleFieldProperty.valueProperty;
					if (valueProperty != null)
					{
						height = GetConstantHeight(valueProperty, label);
					}
					else
					{
						string message = Localization.GetWord("FlexibleField.NotSupportType");

						height = EditorGUIUtility.singleLineHeight + EditorGUITools.GetHelpBoxHeight(message, MessageType.Error);
					}
					break;
				case FlexibleType.Parameter:
					height = GetParameterHeight(flexibleFieldProperty.parameterProperty.property, label);
					break;
				case FlexibleType.DataSlot:
					height = GetDataSlotHeight(flexibleFieldProperty.slotProperty.property, label);
					break;
			}

			return height;
		}

		protected override float GetHeight(GUIContent label)
		{
			float height = GetFieldAreaHeight(label);

			FlexibleType type = flexibleFieldProperty.type;

			if (type != FlexibleType.Constant)
			{
				height += GetDropConstantHeight();
			}

			if (type != FlexibleType.Parameter)
			{
				height += _ParameterReferenceEditorGUI.GetDropParameterHeight();
			}

			return height;
		}
	}
}