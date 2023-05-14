//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	public sealed class ListGUI
	{
		private SerializedProperty _Property;
		private GUIContent _Label;

		public ListGUI(SerializedProperty property)
		{
			_Property = property;
			_Label = new GUIContent(EditorGUITools.NicifyVariableName(property.name));
		}

		public delegate void DelegateAddButton();

		public DelegateAddButton addButton;

		public delegate void DelegateOnAddItem(SerializedProperty property);

		public DelegateOnAddItem onAddItem;

		public delegate void DelegateDrawChild(SerializedProperty property);

		public DelegateDrawChild drawChild;

		public delegate void DelegateRemove(SerializedProperty property);

		public DelegateRemove remove;

		public void OnGUI()
		{
			using (new Arbor.ProfilerScope("ListGUI.OnGUI"))
			{
				GUILayout.BeginHorizontal(BuiltInStyles.RLHeader);

				GUILayout.Label(_Label);

				GUILayout.FlexibleSpace();
				if (GUILayout.Button(EditorContents.addIconContent, BuiltInStyles.invisibleButton, GUILayout.Width(20f), GUILayout.Height(20)))
				{
					if (addButton != null)
					{
						addButton();
					}
					else
					{
						_Property.arraySize++;
						onAddItem?.Invoke(_Property.GetArrayElementAtIndex(_Property.arraySize - 1));
					}

					_Property.serializedObject.ApplyModifiedProperties();
					_Property.serializedObject.Update();
				}
				GUILayout.EndHorizontal();

				if (_Property.arraySize > 0)
				{
					EditorGUILayout.BeginVertical(BuiltInStyles.RLBackground);

					for (int i = 0; i < _Property.arraySize; i++)
					{
						EditorGUILayout.BeginHorizontal();

						EditorGUILayout.BeginVertical();

						SerializedProperty property = _Property.GetArrayElementAtIndex(i);

						if (drawChild != null)
						{
							using (new Arbor.ProfilerScope("DrawChild"))
							{
								drawChild(property);
							}
						}
						else
						{
							EditorGUILayout.PropertyField(property, true);
						}

						EditorGUILayout.EndVertical();

						if (GUILayout.Button(EditorContents.removeIconContent, BuiltInStyles.invisibleButton, GUILayout.Width(20f), GUILayout.Height(20)))
						{
							remove?.Invoke(property);
							_Property.DeleteArrayElementAtIndex(i);
							break;
						}

						EditorGUILayout.EndHorizontal();

						if (i < _Property.arraySize - 1)
						{
							EditorGUILayout.Space();

							EditorGUITools.DrawSeparator();
						}
					}

					GUILayout.Space(10);

					EditorGUILayout.EndVertical();
				}
			}
		}
	}
}
