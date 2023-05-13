//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;
	using ArborEditor.UIElements;

	internal sealed class StateLinkSelectorWindow : EditorWindow
	{
		private static StateLinkSelectorWindow _Instance;

		public static StateLinkSelectorWindow instance
		{
			get
			{
				if (_Instance == null)
				{
					StateLinkSelectorWindow[] objects = Resources.FindObjectsOfTypeAll<StateLinkSelectorWindow>();
					if (objects.Length > 0)
					{
						_Instance = objects[0];
					}
				}
				if (_Instance == null)
				{
					_Instance = ScriptableObject.CreateInstance<StateLinkSelectorWindow>();
				}
				return _Instance;
			}
		}

		private NodeGraphEditor _NodeGraphEditor;

		private NodeListElement _NodeListElement = null;

		public delegate void OnSelectCallback(NodeEditor nodeEditor);

		public OnSelectCallback onSelect;

		private int _SelectedStateID;

		private StateLinkSelectorWindow()
		{
		}

		void OnEnable()
		{
			_NodeListElement = new NodeListElement()
			{
				isShowCallback = IsShowNode,
				sortComparisonCallback = StateMachineGraphEditor.InternalNodeListSortComparison,
				onSelectionChange = OnSelectionChangeList,
				isSelectionCallback = IsSelectionNode,
			};
			rootVisualElement.Add(_NodeListElement);

			_NodeListElement.graphEditor = _NodeGraphEditor;

			ArborStyleSheets.Setup(rootVisualElement);
		}

		private void OnSelectionChangeList(IEnumerable<NodeEditor> selection)
		{
			var nodeEditor = selection.FirstOrDefault();
			if (nodeEditor != null)
			{
				onSelect?.Invoke(nodeEditor);

				nodeEditor.graphEditor.Repaint();

				Close();
			}
		}

		void OnDisable()
		{
			_NodeListElement.graphEditor = null;
		}

		bool IsShowNode(NodeEditor nodeEditor)
		{
			State state = nodeEditor.node as State;
			return (state != null && !state.resident);
		}

		bool IsSelectionNode(NodeEditor nodeEditor)
		{
			return nodeEditor != null && nodeEditor.nodeID == _SelectedStateID;
		}

		public void Open(NodeGraphEditor graphEditor, Rect buttonRect, int selectedStateID, OnSelectCallback onSelect)
		{
			_SelectedStateID = selectedStateID;

			_NodeGraphEditor = graphEditor;

			if (_NodeListElement != null)
			{
				_NodeListElement.graphEditor = graphEditor;
			}

			this.onSelect = onSelect;

			ShowAsDropDown(buttonRect, new Vector2(300f, 320f));

			Focus();
		}
	}
}