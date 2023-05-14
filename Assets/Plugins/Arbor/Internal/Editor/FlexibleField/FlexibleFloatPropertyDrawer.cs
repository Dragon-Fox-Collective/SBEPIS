//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	internal sealed class FlexibleFloatPropertyEditor : FlexibleNumericPropertyEditor
	{
		protected override void OnConstantGUI(Rect position, GUIContent label)
		{
			ConstantRangeAttribute rangeAttribute = AttributeHelper.GetAttribute<ConstantRangeAttribute>(fieldInfo);
			if (rangeAttribute != null)
			{
				EditorGUI.Slider(position, flexibleNumericProperty.valueProperty, rangeAttribute.min, rangeAttribute.max, label);
			}
			else
			{
				base.OnConstantGUI(position, label);
			}
		}
	}

	[CustomPropertyDrawer(typeof(FlexibleFloat))]
	internal sealed class FlexibleFloatPropertyDrawer : PropertyEditorDrawer<FlexibleFloatPropertyEditor>
	{
	}
}
