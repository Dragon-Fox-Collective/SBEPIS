using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SBEPIS.Utils
{
	[CustomEditor(typeof(PlayerInputDataEvents))]
	public class PlayerInputPerformedEventsEditor : Editor
	{
		[SerializeField] private List<bool> actionMapEventsUnfolded = new();
		
		// TODO: Memory leak - actions are added but never removed
		
		public override void OnInspectorGUI()
		{
			EditorGUI.BeginChangeCheck();
			
			PlayerInputDataEvents targetObject = (PlayerInputDataEvents)serializedObject.targetObject;
			
			while (actionMapEventsUnfolded.Count < targetObject.Input.actions.actionMaps.Count)
				actionMapEventsUnfolded.Add(false);
			while (actionMapEventsUnfolded.Count > targetObject.Input.actions.actionMaps.Count)
				actionMapEventsUnfolded.Pop();
			
			for (int actionMapIndex = 0; actionMapIndex < targetObject.Input.actions.actionMaps.Count; actionMapIndex++)
			{
				InputActionMap actionMap = targetObject.Input.actions.actionMaps[actionMapIndex];
				actionMapEventsUnfolded[actionMapIndex] = EditorGUILayout.Foldout(actionMapEventsUnfolded[actionMapIndex], actionMap.name, toggleOnLabelClick: true);
				if (actionMapEventsUnfolded[actionMapIndex])
				{
					using (new EditorGUI.IndentLevelScope())
					{
						foreach (InputAction action in actionMap.actions)
						{
							switch(action.expectedControlType)
							{
								case "Button":
									HandleAction(action, targetObject.TriggerActions, serializedObject.FindProperty("triggerActions"));
									HandleAction(action, targetObject.BoolActions, serializedObject.FindProperty("boolActions"));
									break;
								
								case "Axis":
									HandleAction(action, targetObject.FloatActions, serializedObject.FindProperty("floatActions"));
									break;
								
								case "Vector2":
									HandleAction(action, targetObject.Vector2Actions, serializedObject.FindProperty("vector2Actions"));
									break;
								
								case "Vector3":
									HandleAction(action, targetObject.Vector3Actions, serializedObject.FindProperty("vector3Actions"));
									break;
								
								case "Pose":
									HandleAction(action, targetObject.PoseActions, serializedObject.FindProperty("poseActions"));
									break;
								
								case "Quaternion":
									HandleAction(action, targetObject.QuaternionActions, serializedObject.FindProperty("quaternionActions"));
									break;
								
								default:
									EditorGUILayout.HelpBox($"{action.name} - Unsupported control type {action.expectedControlType}", MessageType.Error);
									break;
							}
						}
					}
				}
			}
			
			if (EditorGUI.EndChangeCheck())
				serializedObject.ApplyModifiedProperties();
		}
		
		private static void HandleAction(InputAction action, List<PlayerInputEvent> inputEvents, SerializedProperty property)
		{
			string actionId = action.id.ToString();
			
			for (int inputEventIndex = 0; inputEventIndex < inputEvents.Count; inputEventIndex++)
			{
				if (inputEvents[inputEventIndex].ActionId == actionId)
				{
					EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(inputEventIndex), new GUIContent(action.name));
					return;
				}
			}
			
			PlayerInputEvent newInputEvent = new();
			newInputEvent.ActionId = actionId;
			inputEvents.Add(newInputEvent);
		}
		
		private static void HandleAction<T>(InputAction action, List<PlayerInputEvent<T>> inputEvents, SerializedProperty property)
		{
			string actionId = action.id.ToString();
			
			for (int inputEventIndex = 0; inputEventIndex < inputEvents.Count; inputEventIndex++)
			{
				if (inputEvents[inputEventIndex].ActionId == actionId)
				{
					EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(inputEventIndex), new GUIContent(action.name));
					return;
				}
			}
			
			PlayerInputEvent<T> newInputEvent = new();
			newInputEvent.ActionId = actionId;
			inputEvents.Add(newInputEvent);
		}
	}
}
