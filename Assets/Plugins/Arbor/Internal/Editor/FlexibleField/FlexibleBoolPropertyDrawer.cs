//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	internal sealed class FlexibleBoolPropertyEditor : FlexiblePrimitivePropertyEditor
	{
		private FlexibleBoolProperty flexibleBoolProperty;

		protected override FlexiblePrimitiveProperty CreateFlexibleProperty()
		{
			return flexibleBoolProperty = new FlexibleBoolProperty(property);
		}

		protected override void OnRandomGUI(Rect position, GUIContent label)
		{
			EditorGUI.PropertyField(position, flexibleBoolProperty.probabilityProperty, label, true);
		}

		protected override float GetRandomHeight(GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(flexibleBoolProperty.probabilityProperty, label, true);
		}
	}

	[CustomPropertyDrawer(typeof(FlexibleBool))]
	internal sealed class FlexibleBoolPropertyDrawer : PropertyEditorDrawer<FlexibleBoolPropertyEditor>
	{
	}
}
