//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;
	using ArborEditor.UIElements;

	internal sealed class BehaviourDragInfo
	{
		static BehaviourDragInfo s_BehaviourDragInfo = null;
		const string k_DragBehaviourGenericKey = "Arbor.BehaviourDragInfo";

		public static void BeginDragBehaviour(BehaviourEditorGUI behaviourEditor, int controlId)
		{
			DragAndDrop.PrepareStartDrag();

			if (s_BehaviourDragInfo == null)
			{
				s_BehaviourDragInfo = new BehaviourDragInfo();
			}

			s_BehaviourDragInfo.dragging = true;
			s_BehaviourDragInfo.controlID = controlId;
			s_BehaviourDragInfo.behaviourEditor = behaviourEditor;

			GraphView graphView = behaviourEditor.nodeEditor.graphEditor.graphView;
			graphView.autoScroll = true;

			Object behaviourObj = behaviourEditor.behaviourObj;

			if (!(behaviourObj is NodeBehaviour))
			{
				behaviourObj = null;
			}

			DragAndDrop.SetGenericData(k_DragBehaviourGenericKey, s_BehaviourDragInfo);

			DragAndDrop.objectReferences = new Object[] { behaviourObj };
			DragAndDrop.paths = null;
			DragAndDrop.activeControlID = controlId;
			DragAndDrop.StartDrag(behaviourObj != null ? ObjectNames.GetDragAndDropTitle(behaviourObj) : "Missing");
		}

		public static BehaviourDragInfo GetBehaviourDragInfo()
		{
			return DragAndDrop.GetGenericData(k_DragBehaviourGenericKey) as BehaviourDragInfo;
		}

		public static int GetDragControlID()
		{
			BehaviourDragInfo behaviourDragInfo = GetBehaviourDragInfo();
			if (behaviourDragInfo == null || !behaviourDragInfo.dragging)
			{
				return 0;
			}

			return behaviourDragInfo.controlID;
		}

		public bool dragging = false;
		public int controlID
		{
			get;
			private set;
		}

		public BehaviourEditorGUI behaviourEditor
		{
			get;
			private set;
		}
	}
}