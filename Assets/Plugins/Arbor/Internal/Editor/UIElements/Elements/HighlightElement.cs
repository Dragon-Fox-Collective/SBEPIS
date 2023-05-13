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
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	internal sealed class HighlightElement : VisualElement
	{
		private readonly GraphView _GraphView;

		private Rect _Position;
		public Rect position
		{
			get
			{
				return _Position;
			}
			set
			{
				if (_Position != value)
				{
					_Position = value;

					UpdateLayout();
				}
			}
		}

		public HighlightElement(GraphView graphView)
		{
			_GraphView = graphView;

			var frame = new VisualElement();
			frame.AddToClassList("highlight");
			Add(frame);

			pickingMode = PickingMode.Ignore;

			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
		}

		void OnAttachToPanel(AttachToPanelEvent evt)
		{
			_GraphView.RegisterCallback<ChangeGraphScrollEvent>(OnChangeGraphView);
			_GraphView.RegisterCallback<ChangeGraphExtentsEvent>(OnChangeGraphView);

			UpdateLayout();
		}

		void OnDetachFromPanel(DetachFromPanelEvent evt)
		{
			_GraphView.UnregisterCallback<ChangeGraphScrollEvent>(OnChangeGraphView);
			_GraphView.UnregisterCallback<ChangeGraphExtentsEvent>(OnChangeGraphView);
		}

		void OnChangeGraphView(IChangeGraphViewEvent e)
		{
			UpdateLayout();
		}

		void UpdateLayout()
		{
			if (parent == null)
			{
				return;
			}

			this.SetLayout(_GraphView.GraphToElement(parent, _Position));
		}
	}
}