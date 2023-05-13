//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	internal sealed class FlexibleComponentPropertyEditor : FlexibleSceneObjectPropertyEditor
	{
		private ClassConstraintInfo _ConstraintInfo;

		ClassConstraintInfo GetConstraint()
		{
			FlexibleComponent flexibleComponent = SerializedPropertyUtility.GetPropertyObject<FlexibleComponent>(property);
			if (flexibleComponent != null)
			{
				return flexibleComponent.GetConstraint();
			}

			return null;
		}

		System.Type GetConnectableBaseType()
		{
			if (_ConstraintInfo != null)
			{
				System.Type connectableType = _ConstraintInfo.GetConstraintBaseType();
				if (connectableType != null && typeof(Component).IsAssignableFrom(connectableType))
				{
					return connectableType;
				}
			}

			return typeof(Component);
		}

		protected override System.Type GetConstantObjectType()
		{
			return GetConnectableBaseType();
		}

		protected override FlexibleSceneObjectProperty CreateProperty(SerializedProperty property)
		{
			return new FlexibleComponentProperty(property);
		}

		protected override void OnConstantGUI(Rect position, GUIContent label)
		{
			SerializedProperty valueProperty = flexibleProperty.valueProperty;

			System.Type type = GetConnectableBaseType();

			EditorGUI.BeginChangeCheck();

			Object objectReferenceValue = EditorGUI.ObjectField(position, label, valueProperty.objectReferenceValue, type, true);

			if (EditorGUI.EndChangeCheck() && (objectReferenceValue == null || _ConstraintInfo == null || _ConstraintInfo.IsConstraintSatisfied(objectReferenceValue.GetType())))
			{
				valueProperty.objectReferenceValue = objectReferenceValue;
			}
			else if (valueProperty.objectReferenceValue != null && _ConstraintInfo != null && !_ConstraintInfo.IsConstraintSatisfied(valueProperty.objectReferenceValue.GetType()))
			{
				valueProperty.objectReferenceValue = null;
			}
		}

		protected override void DoGUI(Rect position, GUIContent label)
		{
			_ConstraintInfo = GetConstraint();

			base.DoGUI(position, label);
		}
	}

	[CustomPropertyDrawer(typeof(FlexibleComponent))]
	internal sealed class FlexibleComponentPropertyDrawer : PropertyEditorDrawer<FlexibleComponentPropertyEditor>
	{
	}
}
