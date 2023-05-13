//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	internal sealed class FlexibleStringPropertyEditor : FlexibleFieldPropertyEditor
	{
		private static readonly GUIContent s_TempContent = null;
		const float kLineHeight = 13f;

		static FlexibleStringPropertyEditor()
		{
			s_TempContent = new GUIContent();
		}

		static GUIContent GetTempContent(string text)
		{
			s_TempContent.text = text;
			s_TempContent.tooltip = string.Empty;
			return s_TempContent;
		}

		static int GetTextLine(string text)
		{
			GUIContent content = GetTempContent(text);
			float contentWidth = UnityEditorBridge.EditorGUIUtilityBridge.contextWidth;
			float fullTextHeight = EditorStyles.textArea.CalcHeight(content, contentWidth);
			int lines = Mathf.CeilToInt(fullTextHeight / kLineHeight);

			return Mathf.Max(lines, 1) - 1;
		}

		static float GetTextHeight(string text)
		{
			return GetTextLine(text) * kLineHeight;
		}

		protected override void OnConstantGUI(Rect position, SerializedProperty valueProperty, GUIContent label)
		{
			if (AttributeHelper.HasAttribute<ConstantMultilineAttribute>(fieldInfo))
			{
				Rect labelPosition = EditorGUI.IndentedRect(position);
				labelPosition.height = 16f;
				position.yMin += labelPosition.height;

				EditorGUI.HandlePrefixLabel(position, labelPosition, label);

				EditorGUI.BeginChangeCheck();
				string value = EditorGUI.TextArea(position, valueProperty.stringValue, EditorStyles.textArea);
				if (EditorGUI.EndChangeCheck())
				{
					valueProperty.stringValue = value;
				}
			}
			else if (AttributeHelper.HasAttribute<TagSelectorAttribute>(fieldInfo))
			{
				EditorGUITools.TagField(position, valueProperty, label);
			}
			else
			{
				base.OnConstantGUI(position, valueProperty, label);
			}
		}

		protected override float GetConstantHeight(SerializedProperty valueProperty, GUIContent label)
		{
			if (AttributeHelper.HasAttribute<ConstantMultilineAttribute>(fieldInfo))
			{
				return 32.0f + GetTextHeight(valueProperty.stringValue);
			}

			return base.GetConstantHeight(valueProperty, label);
		}
	}

	[CustomPropertyDrawer(typeof(FlexibleString))]
	internal sealed class FlexibleStringPropertyDrawer : PropertyEditorDrawer<FlexibleStringPropertyEditor>
	{
	}
}
