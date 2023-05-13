//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor.UIElements
{
	using Arbor;

	sealed class NodeListBreakpoint : VisualElement
	{
		public static readonly string ussClassName = "node-list-breakpoint";

		private NodeEditor _NodeEditor;
		public NodeEditor nodeEditor
		{
			get
			{
				return _NodeEditor;
			}
			set
			{
				if (_NodeEditor != value)
				{
					if (_NodeEditor != null)
					{
						UnregisterCallbackFromNodeEditor();
					}

					_NodeEditor = value;

					if (_NodeEditor != null)
					{
						RegisterCallbackToNodeEditor();
					}
				}
			}
		}

		private NodeGraph _NodeGraph;

		private System.Func<bool> _IsStoppingBreakpoint;

		public System.Func<bool> isStoppingBreakpoint
		{
			get
			{
				return _IsStoppingBreakpoint;
			}
			set
			{
				if (_IsStoppingBreakpoint != value)
				{
					_IsStoppingBreakpoint = value;

					UpdateStoppingBreakpoint();
				}
			}
		}

		public NodeListBreakpoint()
		{
			AddToClassList(ussClassName);
			AddToClassList("break-point");

			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
		}

		void OnAttachToPanel(AttachToPanelEvent e)
		{
			RegisterCallbackToNodeEditor();

			EditorApplication.pauseStateChanged += OnPauseStateChanged;
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			UnregisterCallbackFromNodeEditor();

			EditorApplication.pauseStateChanged -= OnPauseStateChanged;
		}

		void RegisterCallbackToNodeEditor()
		{
			_NodeGraph = _NodeEditor.graphEditor.nodeGraph;
			NodeGraphCallback.RegisterStateChangedCallback(_NodeGraph, OnStateChanged);
		}

		void UnregisterCallbackFromNodeEditor()
		{
			if (_NodeGraph is object)
			{
				NodeGraphCallback.UnregisterStateChangedCallback(_NodeGraph, OnStateChanged);
				_NodeGraph = null;
			}
		}

		void OnPauseStateChanged(PauseState pauseState)
		{
			UpdateStoppingBreakpoint();
		}

		void OnStateChanged(NodeGraph nodeGraph)
		{
			UpdateStoppingBreakpoint();
		}

		void UpdateStoppingBreakpoint()
		{
			if (isStoppingBreakpoint != null)
			{
				EnableInClassList("break-point-on", isStoppingBreakpoint());
			}
		}
	}
}