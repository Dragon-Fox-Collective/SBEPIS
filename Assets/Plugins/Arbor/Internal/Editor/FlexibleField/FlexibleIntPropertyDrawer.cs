//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	internal sealed class FlexibleIntPropertyEditor : FlexibleNumericPropertyEditor
	{
		protected override void OnConstantGUI(Rect position, GUIContent label)
		{
			ConstantRangeAttribute rangeAttribute = AttributeHelper.GetAttribute<ConstantRangeAttribute>(fieldInfo);
			if (rangeAttribute != null)
			{
				EditorGUI.IntSlider(position, flexibleNumericProperty.valueProperty, (int)rangeAttribute.min, (int)rangeAttribute.max, label);
			}
			else
			{
				base.OnConstantGUI(position, label);
			}
		}
	}

	[CustomPropertyDrawer(typeof(FlexibleInt))]
	internal sealed class FlexibleIntPropertyDrawer : PropertyEditorDrawer<FlexibleIntPropertyEditor>
	{
	}
}
