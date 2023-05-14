//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArborEditor.UIElements
{
	public sealed class MinimapTransformManipulator : Manipulator
	{
		private MinimapView _MinimapView;
		private System.Action _Callback;

		public MinimapTransformManipulator(MinimapView minimapView, System.Action callback)
		{
			_MinimapView = minimapView;
			_Callback = callback;
		}

		protected override void RegisterCallbacksOnTarget()
		{
			target.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			target.RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

			if (target.parent != null)
			{
				RegisterCallbacksOnMinimapView();
			}
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			target.UnregisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			target.UnregisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
		}

		void RegisterCallbacksOnMinimapView()
		{
			_MinimapView.RegisterCallback<MinimapTransformEvent>(OnMinimapTransform);
		}

		void UnregisterCallbacksFromMinimapView()
		{
			_MinimapView.UnregisterCallback<MinimapTransformEvent>(OnMinimapTransform);
		}

		void OnAttachToPanel(AttachToPanelEvent e)
		{
			RegisterCallbacksOnMinimapView();
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			UnregisterCallbacksFromMinimapView();
		}

		void OnMinimapTransform(MinimapTransformEvent e)
		{
			_Callback?.Invoke();
		}
	}
}