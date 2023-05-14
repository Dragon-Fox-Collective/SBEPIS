//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using Arbor.StateMachine.StateBehaviours;

	[CustomEditor(typeof(InvokeMethod))]
	internal sealed class InvokeMethodInspector : Editor
	{
		private SerializedProperty _EventEntriesProperty;

		private static class Defaults
		{
			public static readonly GUIContent iconToolbarMinus;
			public static readonly GUIContent[] eventTypes;
			public static readonly GUIContent addButtonContent;

			static Defaults()
			{
				addButtonContent = Localization.GetTextContent("Add New Event Type");

				// Have to create a copy since otherwise the tooltip will be overwritten.
				iconToolbarMinus = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus"));
				iconToolbarMinus.tooltip = "Remove all events in this list.";

				string[] eventNames = System.Enum.GetNames(typeof(InvokeMethod.StateTriggerType));
				eventTypes = new GUIContent[eventNames.Length];
				for (int i = 0; i < eventNames.Length; ++i)
				{
					eventTypes[i] = new GUIContent(eventNames[i]);
				}
			}
		}
		private GUIContent _EventIDName;

		void OnEnable()
		{
			if (target == null)
			{
				// Unity 2018.4.16f1: Target may be null after compilation.
				// "ArgumentException: Object at index 0 is null"
				return;
			}

			_EventEntriesProperty = serializedObject.FindProperty("_EventEntries");
			_EventIDName = new GUIContent("");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			int toBeRemovedEntry = -1;

			EditorGUILayout.Space();

			Vector2 removeButtonSize = GUIStyle.none.CalcSize(Defaults.iconToolbarMinus);

			for (int i = 0; i < _EventEntriesProperty.arraySize; ++i)
			{
				SerializedProperty entryProperty = _EventEntriesProperty.GetArrayElementAtIndex(i);
				SerializedProperty triggerTypeProperty = entryProperty.FindPropertyRelative("triggerType");
				SerializedProperty callbackProperty = entryProperty.FindPropertyRelative("callback");

				_EventIDName.text = triggerTypeProperty.enumDisplayNames[triggerTypeProperty.enumValueIndex];

				EditorGUILayout.PropertyField(callbackProperty, _EventIDName);

				Rect callbackRect = GUILayoutUtility.GetLastRect();

				Rect removeButtonPos = new Rect(callbackRect.xMax - removeButtonSize.x - 8, callbackRect.y + 1, removeButtonSize.x, removeButtonSize.y);
				if (GUI.Button(removeButtonPos, Defaults.iconToolbarMinus, GUIStyle.none))
				{
					toBeRemovedEntry = i;
				}

				EditorGUILayout.Space();
			}

			if (toBeRemovedEntry > -1)
			{
				RemoveEntry(toBeRemovedEntry);
			}

			Rect btPosition = GUILayoutUtility.GetRect(Defaults.addButtonContent, GUI.skin.button);
			const float addButonWidth = 200f;
			btPosition.x += (btPosition.width - addButonWidth) / 2;
			btPosition.width = addButonWidth;
			if (GUI.Button(btPosition, Defaults.addButtonContent))
			{
				ShowAddTriggermenu();
			}

			serializedObject.ApplyModifiedProperties();
		}

		private void RemoveEntry(int toBeRemovedEntry)
		{
			SerializedProperty entryProperty = _EventEntriesProperty.GetArrayElementAtIndex(toBeRemovedEntry);
			entryProperty.Clear(true);

			_EventEntriesProperty.DeleteArrayElementAtIndex(toBeRemovedEntry);
		}

		void ShowAddTriggermenu()
		{
			// Now create the menu, add items and show it
			GenericMenu menu = new GenericMenu();
			for (int i = 0; i < Defaults.eventTypes.Length; ++i)
			{
				bool active = true;

				// Check if we already have a Entry for the current eventType, if so, disable it
				for (int p = 0; p < _EventEntriesProperty.arraySize; ++p)
				{
					SerializedProperty entryProperty = _EventEntriesProperty.GetArrayElementAtIndex(p);
					SerializedProperty triggerTypeProperty = entryProperty.FindPropertyRelative("triggerType");
					if (triggerTypeProperty.enumValueIndex == i)
					{
						active = false;
					}
				}

				if (active)
				{
					menu.AddItem(Defaults.eventTypes[i], false, OnAddNewSelected, i);
				}
				else
				{
					menu.AddDisabledItem(Defaults.eventTypes[i]);
				}
			}
			menu.ShowAsContext();
			Event.current.Use();
		}

		private void OnAddNewSelected(object index)
		{
			int selected = (int)index;

			int insertIndex = 0;
			for (int count = _EventEntriesProperty.arraySize; insertIndex < count; ++insertIndex)
			{
				SerializedProperty entryProperty = _EventEntriesProperty.GetArrayElementAtIndex(insertIndex);
				SerializedProperty triggerTypeProperty = entryProperty.FindPropertyRelative("triggerType");
				if (triggerTypeProperty.enumValueIndex > selected)
				{
					break;
				}
			}

			_EventEntriesProperty.InsertArrayElementAtIndex(insertIndex);

			SerializedProperty newEntryProperty = _EventEntriesProperty.GetArrayElementAtIndex(insertIndex);
			SerializedProperty newTriggerTypeProperty = newEntryProperty.FindPropertyRelative("triggerType");
			SerializedProperty newCallbackProperty = newEntryProperty.FindPropertyRelative("callback");
			newTriggerTypeProperty.enumValueIndex = selected;
			newCallbackProperty.Clear(true);

			serializedObject.ApplyModifiedProperties();
		}
	}
}