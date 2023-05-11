//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	internal sealed class FlexibleStateTriggerFlagsPropertyEditor : FlexibleFieldPropertyEditor
	{
		protected override void OnConstantGUI(Rect position, SerializedProperty valueProperty, GUIContent label)
		{
			StateTriggerMaskAttribute maskAttr = AttributeHelper.GetAttribute<StateTriggerMaskAttribute>(fieldInfo);
			if (maskAttr != null)
			{
				valueProperty.SetStateData(maskAttr);
			}
			else
			{
				valueProperty.RemoveStateData<StateTriggerMaskAttribute>();
			}

			base.OnConstantGUI(position, valueProperty, label);
		}
	}

	[CustomPropertyDrawer(typeof(FlexibleStateTriggerFlags))]
	internal sealed class FlexibleStateTriggerFlagsPropertyDrawer : PropertyEditorDrawer<FlexibleStateTriggerFlagsPropertyEditor>
	{
	}
}