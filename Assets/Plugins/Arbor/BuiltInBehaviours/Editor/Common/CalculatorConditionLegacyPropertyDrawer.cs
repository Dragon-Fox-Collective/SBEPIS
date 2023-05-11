//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	[CustomPropertyDrawer(typeof(CalculatorConditionLegacy))]
	internal sealed class CalculatorConditionLegacyPropertyDrawer : PropertyDrawer
	{
		private LayoutArea _LayoutArea = new LayoutArea();

		private static readonly RectOffset s_LayoutMargin = new RectOffset(0, 0, 2, 2);

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			_LayoutArea.Begin(position, false, s_LayoutMargin);

			DoGUI(property, label);

			_LayoutArea.End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			_LayoutArea.Begin(new Rect(), true, s_LayoutMargin);

			DoGUI(property, label);

			_LayoutArea.End();

			return _LayoutArea.rect.height;
		}

		void DoGUI(SerializedProperty property, GUIContent label)
		{
			SerializedProperty typeProperty = property.FindPropertyRelative("_Type");

			CalculatorCondition.Type type = EnumUtility.GetValueFromIndex<CalculatorCondition.Type>(typeProperty.enumValueIndex);

			switch (type)
			{
				case CalculatorCondition.Type.Int:
					{
						_LayoutArea.PropertyField(property.FindPropertyRelative("_CompareType"));
						_LayoutArea.PropertyField(property.FindPropertyRelative("_IntValue1"));
						_LayoutArea.PropertyField(property.FindPropertyRelative("_IntValue2"));
					}
					break;
				case CalculatorCondition.Type.Float:
					{
						_LayoutArea.PropertyField(property.FindPropertyRelative("_CompareType"));
						_LayoutArea.PropertyField(property.FindPropertyRelative("_FloatValue1"));
						_LayoutArea.PropertyField(property.FindPropertyRelative("_FloatValue2"));
					}
					break;
				case CalculatorCondition.Type.Bool:
					{
						_LayoutArea.PropertyField(property.FindPropertyRelative("_BoolValue1"));
						_LayoutArea.PropertyField(property.FindPropertyRelative("_BoolValue2"));
					}
					break;
			}
		}
	}
}