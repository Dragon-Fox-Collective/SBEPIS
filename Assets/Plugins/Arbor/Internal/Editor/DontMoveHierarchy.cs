//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	[InitializeOnLoad]
	internal static class DontMoveHierarchy
	{
		static DontMoveHierarchy()
		{
			EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
			EditorApplication.projectWindowItemOnGUI += OnProjectWindowGUI;
		}

		static bool IsDontMove(Object obj)
		{
			if (obj is NodeGraph || obj is ParameterDraggingObject)
			{
				return true;
			}

			if (obj is NodeBehaviour)
			{
				return BehaviourDragInfo.GetBehaviourDragInfo() != null;
			}

			return false;
		}

		static void OnHierarchyGUI(int instanceID, Rect selectionRect)
		{
			Event current = Event.current;

			switch (current.type)
			{
				case EventType.DragUpdated:
					{
						Object[] dragObjects = DragAndDrop.objectReferences;
						int dragObjCount = dragObjects.Length;
						for (int dragObjIndex = 0; dragObjIndex < dragObjCount; dragObjIndex++)
						{
							Object dragObj = dragObjects[dragObjIndex];

							if (IsDontMove(dragObj))
							{
								DragAndDrop.visualMode = DragAndDropVisualMode.None;
								current.Use();
								break;
							}
						}
					}
					break;
			}
		}

		static void OnProjectWindowGUI(string guid, Rect selectionRect)
		{
			Event current = Event.current;

			switch (current.type)
			{
				case EventType.DragUpdated:
					{
						Object[] dragObjects = DragAndDrop.objectReferences;
						int dragObjCount = dragObjects.Length;
						for (int dragObjIndex = 0; dragObjIndex < dragObjCount; dragObjIndex++)
						{
							Object dragObj = dragObjects[dragObjIndex];

							if (IsDontMove(dragObj))
							{
								DragAndDrop.visualMode = DragAndDropVisualMode.None;
								current.Use();
								break;
							}
						}
					}
					break;
			}
		}
	}
}