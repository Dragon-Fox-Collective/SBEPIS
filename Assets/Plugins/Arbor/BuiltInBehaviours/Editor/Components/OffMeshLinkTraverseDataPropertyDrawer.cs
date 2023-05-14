//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	public class OffMeshLinkTraverseDataPropertyEditor : PropertyEditor
	{
		private static readonly RectOffset s_LayoutMargin = new RectOffset(0, 0, 0, 2);

		private OffMeshLinkTraverseDataProperty _Property;
		private LayoutArea _LayoutArea = new LayoutArea();

		protected override void OnInitialize()
		{
			_Property = new OffMeshLinkTraverseDataProperty(property);
		}

		void DoGUI(GUIContent label)
		{
			label = EditorGUI.BeginProperty(_LayoutArea.rect, label, property);

			property.isExpanded = _LayoutArea.Foldout(property.isExpanded, label, true);
			if (property.isExpanded)
			{
				EditorGUI.indentLevel++;

				_LayoutArea.PropertyField(_Property.parameterProperty.property);
				_LayoutArea.PropertyField(_Property.angularSpeedProperty);
				_LayoutArea.PropertyField(_Property.jumpHeightProperty);
				_LayoutArea.PropertyField(_Property.minSpeedProperty);
				_LayoutArea.PropertyField(_Property.startWaitProperty, null, true);
				_LayoutArea.PropertyField(_Property.endWaitProperty, null, true);

				EditorGUI.indentLevel--;
			}

			EditorGUI.EndProperty();
		}

		protected override void OnGUI(Rect position, GUIContent label)
		{
			_LayoutArea.Begin(position, false, s_LayoutMargin);

			DoGUI(label);

			_LayoutArea.End();
		}

		protected override float GetHeight(GUIContent label)
		{
			_LayoutArea.Begin(new Rect(), true, s_LayoutMargin);

			DoGUI(label);

			_LayoutArea.End();

			return _LayoutArea.rect.height;
		}
	}

	[CustomPropertyDrawer(typeof(OffMeshLinkTraverseData))]
	public class OffMeshLinkTraverseDataPropertyDrawer : PropertyEditorDrawer<OffMeshLinkTraverseDataPropertyEditor>
	{
	}
}