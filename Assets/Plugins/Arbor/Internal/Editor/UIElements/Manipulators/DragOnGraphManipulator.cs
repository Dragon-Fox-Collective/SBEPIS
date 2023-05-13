using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

namespace ArborEditor.UIElements
{
	internal abstract class DragOnGraphManipulator : DragManipulator
	{
		private GraphView _GraphView;

		protected virtual void RegisterCallbacksOnGraphView(GraphView graphView)
		{
			graphView.RegisterCallback<ChangeGraphScrollEvent>(OnChangeGraphView);
			graphView.RegisterCallback<ChangeGraphExtentsEvent>(OnChangeGraphView);
		}

		protected virtual void UnregisterCallbacksFromGraphView(GraphView graphView)
		{
			graphView.UnregisterCallback<ChangeGraphScrollEvent>(OnChangeGraphView);
			graphView.UnregisterCallback<ChangeGraphExtentsEvent>(OnChangeGraphView);
		}

		protected virtual void OnChangeGraphView(IChangeGraphViewEvent e)
		{
		}

		protected override void OnStartDrag()
		{
			base.OnStartDrag();

			if (_GraphView != null)
			{
				UnregisterCallbacksFromGraphView(_GraphView);
			}

			_GraphView = target.GetFirstOfType<GraphView>();

			if (_GraphView != null)
			{
				RegisterCallbacksOnGraphView(_GraphView);
			}
		}

		protected override void OnEndDrag()
		{
			base.OnEndDrag();

			if (_GraphView != null)
			{
				UnregisterCallbacksFromGraphView(_GraphView);
			}
		}
	}
}