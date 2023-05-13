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
	internal sealed class BreakPointElement : VisualElement
	{
		private bool _BreakOn = false;
		public bool breakOn
		{
			get
			{
				return _BreakOn;
			}
			set
			{
				if (_BreakOn != value)
				{
					_BreakOn = value;

					EnableInClassList("break-point-on", _BreakOn);

					MarkDirtyRepaint();
				}
			}
		}

		private Vector2 _AttachPoint;
		public Vector2 attachPoint
		{
			get
			{
				return _AttachPoint;
			}
			set
			{
				if (_AttachPoint != value)
				{
					_AttachPoint = value;
				}
			}
		}

		public BreakPointElement()
		{
			pickingMode = PickingMode.Ignore;
			style.position = Position.Absolute;

			AddToClassList("break-point");

			RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
		}

		private bool _IsLayouted = false;

		void OnGeometryChanged(GeometryChangedEvent e)
		{
			_IsLayouted = true;
			UpdatePosition();
		}

		void UpdatePosition()
		{
			if (!_IsLayouted)
				return;

			Vector2 size = new Vector2(resolvedStyle.width, resolvedStyle.height);
			transform.position = _AttachPoint - size * 0.5f;
		}
	}
}