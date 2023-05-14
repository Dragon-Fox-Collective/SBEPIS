//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ArborEditor
{
	using Arbor;

	[CustomPropertyDrawer(typeof(StateTriggerFlags))]
	internal sealed class StateTriggerFlagsPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			StateTriggerFlags triggerFlags = (StateTriggerFlags)property.intValue;
			EditorGUI.BeginChangeCheck();

			StateTriggerMaskAttribute maskAttr = property.GetStateData<StateTriggerMaskAttribute>();
			if (maskAttr == null)
			{
				maskAttr = AttributeHelper.GetAttribute<StateTriggerMaskAttribute>(fieldInfo);
			}

			if (maskAttr != null)
			{
				var values = EnumUtility.GetValues<StateTriggerFlags>();
				var contents = EnumUtility.GetContents<StateTriggerFlags>();

				List<int> flagValues = new List<int>();
				List<string> flagNames = new List<string>();
				int flagValue = 0;

				for (int flagIndex = 0; flagIndex < values.Length; flagIndex++)
				{
					var flag = values[flagIndex];
					if ((flag & maskAttr.mask) != 0)
					{
						if ((triggerFlags & flag) != 0)
						{
							flagValue |= (1 << flagValues.Count);
						}

						flagValues.Add((int)flag);
						flagNames.Add(contents[flagIndex].text);
					}
				}

				flagValue = EditorGUI.MaskField(position, label, flagValue, flagNames.ToArray());

				if (flagValue == 0)
				{
					triggerFlags = (StateTriggerFlags)0;
				}
				else if (flagValue == ~0)
				{
					triggerFlags = (StateTriggerFlags)~0;
				}
				else
				{
					triggerFlags = 0;
					for (int bitIndex = 0; bitIndex < flagValues.Count; bitIndex++)
					{
						if ((flagValue & (1 << bitIndex)) != 0)
						{
							triggerFlags |= (StateTriggerFlags)flagValues[bitIndex];
						}
					}
				}
			}
			else
			{
				triggerFlags = (StateTriggerFlags)EditorGUI.EnumFlagsField(position, label, triggerFlags);
			}
			if (EditorGUI.EndChangeCheck())
			{
				property.intValue = (int)triggerFlags;
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label);
		}
	}
}