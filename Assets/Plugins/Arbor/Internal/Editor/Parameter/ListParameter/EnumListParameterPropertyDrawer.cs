//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	internal sealed class EnumListParameterEditor : ListParameterEditor
	{
		protected override void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			System.Type enumType = property.GetStateData<System.Type>();

			SerializedProperty valueProperty = _ReorderableList.serializedProperty.GetArrayElementAtIndex(index);

			if (!EnumFieldUtility.IsEnum(enumType))
			{
				EditorGUI.PropertyField(rect, valueProperty, true);
			}
			else
			{
				object enumValue = System.Enum.ToObject(enumType, valueProperty.intValue);
				if (AttributeHelper.HasAttribute<System.FlagsAttribute>(enumType))
				{
					enumValue = EditorGUI.EnumFlagsField(rect, GUIContentCaches.Get(valueProperty.displayName), (System.Enum)enumValue);
				}
				else
				{
					enumValue = EditorGUI.EnumPopup(rect, GUIContentCaches.Get(valueProperty.displayName), (System.Enum)enumValue);
				}
				valueProperty.intValue = (int)enumValue;
			}
		}
	}

	[CustomPropertyDrawer(typeof(EnumListParameter))]
	internal sealed class EnumListParameterPropertyDrawer : PropertyEditorDrawer<EnumListParameterEditor>
	{
	}
}