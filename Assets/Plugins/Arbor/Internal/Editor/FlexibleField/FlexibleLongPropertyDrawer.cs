//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	internal sealed class FlexibleLongPropertyEditor : FlexibleNumericPropertyEditor
	{
		protected override void OnConstantGUI(Rect position, GUIContent label)
		{
			ConstantRangeAttribute rangeAttribute = AttributeHelper.GetAttribute<ConstantRangeAttribute>(fieldInfo);
			if (rangeAttribute != null)
			{
				EditorGUITools.LongSlider(position, flexibleNumericProperty.valueProperty, (long)rangeAttribute.min, (long)rangeAttribute.max, label);
			}
			else
			{
				base.OnConstantGUI(position, label);
			}
		}
	}

	[CustomPropertyDrawer(typeof(FlexibleLong))]
	internal sealed class FlexibleLongPropertyDrawer : PropertyEditorDrawer<FlexibleLongPropertyEditor>
	{
	}
}
