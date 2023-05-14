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

	internal sealed class OffMeshLinkTraverseWaitDataPropertyEditor : PropertyEditor
	{
		private static readonly RectOffset s_LayoutMargin = new RectOffset(0, 0, 0, 2);

		private SerializedProperty _TypeProperty;
		private SerializedProperty _TimeProperty;
		private SerializedProperty _TriggerProperty;
		private LayoutArea _LayoutArea = new LayoutArea();

		protected override void OnInitialize()
		{
			_TypeProperty = property.FindPropertyRelative(nameof(OffMeshLinkTraverseWaitData.type));
			_TimeProperty = property.FindPropertyRelative(nameof(OffMeshLinkTraverseWaitData.time));
			_TriggerProperty = property.FindPropertyRelative(nameof(OffMeshLinkTraverseWaitData.eventName));
		}

		void DoGUI(GUIContent label)
		{
			property.isExpanded = _LayoutArea.Foldout(property.isExpanded, label, true);
			if (property.isExpanded)
			{
				EditorGUI.indentLevel++;
				_LayoutArea.PropertyField(_TypeProperty);

				var waitType = EnumUtility.GetValueFromIndex<OffMeshLinkTraverseWaitData.WaitType>(_TypeProperty.enumValueIndex);
				switch (waitType)
				{
					case OffMeshLinkTraverseWaitData.WaitType.Time:
						_LayoutArea.PropertyField(_TimeProperty);
						break;
					case OffMeshLinkTraverseWaitData.WaitType.AnimationEvent:
						_LayoutArea.PropertyField(_TriggerProperty);
						break;
				}

				EditorGUI.indentLevel--;
			}

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

	[CustomPropertyDrawer(typeof(OffMeshLinkTraverseWaitData))]
	internal sealed class OffMeshLinkTraverseWaitDataPropertyDrawer : PropertyEditorDrawer<OffMeshLinkTraverseWaitDataPropertyEditor>
	{
	}
}