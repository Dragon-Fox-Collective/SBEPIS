using System.Linq;
using NaughtyAttributes.Editor;
using SBEPIS.AI;
using SBEPIS.Utils;
using UnityEditor;
using UnityEngine;

namespace SBEPIS.Bits
{
	[CustomPropertyDrawer(typeof(AIValuedAction))]
	public class AIValuedActionDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => property.isExpanded ? property.GetChildren().Select(EditorGUI.GetPropertyHeight).Sum() : EditorGUIUtility.singleLineHeight;
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			
			//position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			
			int indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			
			property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), property.isExpanded, GUIContent.none);
			
			if (property.isExpanded)
			{
				foreach (SerializedProperty prop in property.GetChildren())
				{
					Rect propRect = new(position.xMin, position.yMin, position.width, EditorGUI.GetPropertyHeight(prop));
					NaughtyEditorGUI.PropertyField(propRect, prop, true);
					position.yMin = propRect.yMax;
				}
			}
			else
			{
				NaughtyEditorGUI.PropertyField(position, property.GetChildren().First(), true);
			}
			
			EditorGUI.indentLevel = indent;
			EditorGUI.EndProperty();
		}
	}
}
