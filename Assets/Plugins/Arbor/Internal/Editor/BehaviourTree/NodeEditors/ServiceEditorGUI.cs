//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.BehaviourTree
{
	using Arbor.BehaviourTree;

	[System.Serializable]
	internal sealed class ServiceEditorGUI : TreeNodeBehaviourEditorGUI
	{
		protected override bool HasBehaviourEnable()
		{
			return true;
		}

		protected override bool GetBehaviourEnable()
		{
			Service service = behaviourObj as Service;
			if (service is object)
			{
				return service.behaviourEnabled;
			}
			return false;
		}

		protected override void SetBehaviourEnable(bool enable)
		{
			Service service = behaviourObj as Service;
			service.behaviourEnabled = enable;
		}

		void MoveUpContextMenu()
		{
			if (treeBehaviourEditor != null)
			{
				treeBehaviourEditor.MoveService(behaviourIndex, behaviourIndex - 1);
			}
		}

		void MoveDownContextMenu()
		{
			if (treeBehaviourEditor != null)
			{
				treeBehaviourEditor.MoveService(behaviourIndex, behaviourIndex + 1);
			}
		}

		void DeleteContextMenu()
		{
			if (nodeEditor != null)
			{
				TreeBehaviourNodeEditor behaviourNodeEditor = nodeEditor as TreeBehaviourNodeEditor;
				behaviourNodeEditor.DestroyServiceAt(behaviourIndex);
			}
		}

		protected override void SetPopupMenu(GenericMenu menu)
		{
			bool editable = nodeEditor.graphEditor.editable;

			ServiceList serviceList = treeBehaviourNode.serviceList;
			int serviceCount = serviceList.count;

			if (behaviourIndex >= 1 && editable)
			{
				menu.AddItem(EditorContents.moveUp, false, MoveUpContextMenu);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.moveUp);
			}

			if (behaviourIndex < serviceCount - 1 && editable)
			{
				menu.AddItem(EditorContents.moveDown, false, MoveDownContextMenu);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.moveDown);
			}

			base.SetPopupMenu(menu);

			if (editable)
			{
				menu.AddItem(EditorContents.delete, false, DeleteContextMenu);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.delete);
			}
		}
	}
}