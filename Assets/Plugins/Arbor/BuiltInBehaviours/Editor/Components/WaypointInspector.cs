//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;
using UnityEditorInternal;

using Arbor;
namespace ArborEditor
{
	[CustomEditor(typeof(Waypoint))]
	internal sealed class WaypointInspector : Editor
	{
		private ReorderableList _PointsList;
		private SerializedProperty _GizmosColorProperty;

		private void OnEnable()
		{
			SerializedProperty pointsProperty = serializedObject.FindProperty("_Points");

			_GizmosColorProperty = serializedObject.FindProperty("_GizmosColor");

			_PointsList = new ReorderableList(serializedObject, pointsProperty)
			{
				drawHeaderCallback = (rect) =>
				{
					EditorGUI.LabelField(rect, pointsProperty.displayName);
				},
				drawElementCallback = (rect, index, isActive, isFocused) =>
				{
					SerializedProperty element = pointsProperty.GetArrayElementAtIndex(index);
					rect.height -= EditorGUIUtility.standardVerticalSpacing * 2;
					rect.y += EditorGUIUtility.standardVerticalSpacing;
					EditorGUI.PropertyField(rect, element);
				},
				onRemoveCallback = (list) =>
				{
					int index = list.index;
					SerializedProperty property = pointsProperty.GetArrayElementAtIndex(index);
					property.objectReferenceValue = null;
					pointsProperty.DeleteArrayElementAtIndex(index);
				},
			};
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			_PointsList.DoLayoutList();
			EditorGUILayout.PropertyField(_GizmosColorProperty);

			serializedObject.ApplyModifiedProperties();
		}
	}
}