//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	internal class FlexibleNumericPropertyEditor : FlexiblePrimitivePropertyEditor
	{
		protected FlexibleNumericProperty flexibleNumericProperty
		{
			get;
			private set;
		}

		protected override FlexiblePrimitiveProperty CreateFlexibleProperty()
		{
			return flexibleNumericProperty = new FlexibleNumericProperty(property);
		}

		protected override void OnRandomGUI(Rect position, GUIContent label)
		{
			Rect contentsLabel = EditorGUITools.PrefixLabel(position, label);
			contentsLabel.width *= 0.5f;
			EditorGUI.PropertyField(contentsLabel, flexibleNumericProperty.minRangeProperty, GUIContent.none);
			contentsLabel.x += contentsLabel.width;
			EditorGUI.PropertyField(contentsLabel, flexibleNumericProperty.maxRangeProperty, GUIContent.none);
		}

		protected override float GetRandomHeight(GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}
	}
}