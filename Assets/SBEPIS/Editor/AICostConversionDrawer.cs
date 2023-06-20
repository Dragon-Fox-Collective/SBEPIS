using System.Linq;
using NaughtyAttributes.Editor;
using SBEPIS.AI;
using SBEPIS.Utils;
using UnityEditor;
using UnityEngine;

namespace SBEPIS.Bits
{
	[CustomPropertyDrawer(typeof(AICostConversion))]
	public class AICostConversionDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => property.GetChildren().Select(EditorGUI.GetPropertyHeight).Sum();
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			
			//position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			
			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			
			foreach (SerializedProperty prop in property.GetChildren())
			{
				Rect propRect = new(position.xMin, position.yMin, position.width, EditorGUI.GetPropertyHeight(prop));
				NaughtyEditorGUI.PropertyField(propRect, prop, true);
				position.yMin = propRect.yMax;
			}
			
			EditorGUI.indentLevel = indent;
			EditorGUI.EndProperty();
		}
	}
}
