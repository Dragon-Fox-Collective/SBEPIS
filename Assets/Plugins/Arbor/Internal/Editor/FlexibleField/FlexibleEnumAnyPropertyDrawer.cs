//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	internal sealed class FlexibleEnumAnyPropertyEditor : FlexibleFieldPropertyEditor
	{
		private ClassConstraintInfo _ConstraintInfo;

		ClassConstraintInfo GetConstraint()
		{
			FlexibleEnumAny filexibleEnumAny = SerializedPropertyUtility.GetPropertyObject<FlexibleEnumAny>(property);
			if (filexibleEnumAny != null)
			{
				return filexibleEnumAny.GetConstraint();
			}

			return null;
		}

		System.Type GetConnectableBaseType()
		{
			if (_ConstraintInfo != null)
			{
				System.Type connectableType = _ConstraintInfo.GetConstraintBaseType();
				if (EnumFieldUtility.IsEnum(connectableType))
				{
					return connectableType;
				}
			}

			return typeof(System.Enum);
		}

		protected override void OnConstantGUI(Rect position, SerializedProperty valueProperty, GUIContent label)
		{
			System.Type type = GetConnectableBaseType();
			if (EnumFieldUtility.IsEnum(type))
			{
				object enumValue = System.Enum.ToObject(type, valueProperty.intValue);

				EditorGUI.BeginChangeCheck();

				if (AttributeHelper.HasAttribute<System.FlagsAttribute>(type))
				{
					enumValue = EditorGUI.EnumFlagsField(position, label, (System.Enum)enumValue);
				}
				else
				{
					enumValue = EditorGUI.EnumPopup(position, label, (System.Enum)enumValue);
				}

				if (EditorGUI.EndChangeCheck())
				{
					valueProperty.intValue = (int)enumValue;
				}
			}
			else
			{
				base.OnConstantGUI(position, valueProperty, label);
			}
		}

		protected override void DoGUI(Rect position, GUIContent label)
		{
			_ConstraintInfo = GetConstraint();

			base.DoGUI(position, label);
		}
	}

	[CustomPropertyDrawer(typeof(FlexibleEnumAny))]
	internal sealed class FlexibleEnumAnyPropertyDrawer : PropertyEditorDrawer<FlexibleEnumAnyPropertyEditor>
	{
	}
}
