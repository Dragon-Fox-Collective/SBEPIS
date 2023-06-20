using SBEPIS.AI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SBEPIS.Bits
{
	[CustomPropertyDrawer(typeof(AIValue))]
	public class AIValueDrawer : PropertyDrawer
	{
		/*
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			VisualElement container = new();
			container.style.flexDirection = FlexDirection.Row;
			container.style.backgroundColor = Color.red;
			
			PropertyField valueType = new(property.FindPropertyRelative("valueType"));
			container.Add(valueType);
			
			PropertyField value = new(property.FindPropertyRelative("value"));
			container.Add(value);
			
			return container;
		}
		*/
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			
			//position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			
			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			
			const float valueWidth = 35;
			const float spacing = 5;
			
			Rect valueTypeRect = new(position.x, position.y, position.width - valueWidth - spacing, position.height);
			Rect valueRect = new(valueTypeRect.xMax + spacing, position.y, valueWidth, position.height);
			
			EditorGUI.PropertyField(valueTypeRect, property.FindPropertyRelative("valueType"), GUIContent.none);
			EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"), GUIContent.none);
			
			EditorGUI.indentLevel = indent;
			EditorGUI.EndProperty();
		}
	}
}
