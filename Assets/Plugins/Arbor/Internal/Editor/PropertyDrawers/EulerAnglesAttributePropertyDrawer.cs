//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	internal sealed class EulerAnglesAttributePropertyEditor : PropertyEditor
	{
		private enum FieldType
		{
			Default,
			Euler,
		}

		private FieldType _CurrentFieldType = FieldType.Euler;

		protected override float GetHeight(GUIContent label)
		{
			if (property.propertyType == SerializedPropertyType.Quaternion)
			{
				return EditorGUIUtility.singleLineHeight * (EditorGUIUtility.wideMode ? 1f : 2f);
			}
			return EditorGUI.GetPropertyHeight(property, label, true);
		}

		protected override void OnGUI(Rect position, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, property);

			if (property.propertyType == SerializedPropertyType.Quaternion)
			{
				if (_CurrentFieldType == FieldType.Euler)
				{
					Vector3 euler = property.quaternionValue.eulerAngles;
					EditorGUI.BeginChangeCheck();
					euler = EditorGUI.Vector3Field(EditorGUITools.SubtractPopupWidth(position), label, euler);
					if (EditorGUI.EndChangeCheck())
					{
						property.quaternionValue = Quaternion.Euler(euler);
					}
				}
				else
				{
					EditorGUI.BeginChangeCheck();
					Vector4 vector4 = new Vector4(property.quaternionValue.x, property.quaternionValue.y, property.quaternionValue.z, property.quaternionValue.w);
					vector4 = EditorGUI.Vector4Field(EditorGUITools.SubtractPopupWidth(position), label, vector4);
					if (EditorGUI.EndChangeCheck())
					{
						property.quaternionValue = new Quaternion(vector4.x, vector4.y, vector4.z, vector4.w);
					}
				}

				Rect contextPosition = EditorGUITools.GetPopupRect(position);
				if (EditorGUI.DropdownButton(contextPosition, EditorContents.popupIcon, FocusType.Passive, Styles.popupIconButton))
				{
					GenericMenu menu = new GenericMenu();
					var values = EnumUtility.GetValues<FieldType>();
					var contents = EnumUtility.GetContents<FieldType>();
					for (int i = 0; i < values.Length; i++)
					{
						FieldType t = values[i];
						GUIContent content = contents[i];
						menu.AddItem(content, t == _CurrentFieldType, (type) =>
						{
							_CurrentFieldType = (FieldType)type;
							HandleUtility.Repaint();
						}, t);
					}
					menu.DropDown(contextPosition);
				}
			}
			else
			{
				EditorGUI.PropertyField(position, property, label, true);
			}

			EditorGUI.EndProperty();
		}
	}

	[CustomPropertyDrawer(typeof(EulerAnglesAttribute))]
	internal sealed class EulerAnglesAttributePropertyDrawer : PropertyEditorDrawer<EulerAnglesAttributePropertyEditor>
	{
	}
}