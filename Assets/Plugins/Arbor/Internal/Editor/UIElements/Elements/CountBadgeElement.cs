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
	internal sealed class CountBadgeElement : Label
	{
		private long _Count;
		public long count
		{
			get
			{
				return _Count;
			}
			set
			{
				if (_Count != value)
				{
					_Count = value;
					text = _Count.ToString();
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
					UpdatePosition();
				}
			}
		}

		public CountBadgeElement()
		{
			pickingMode = PickingMode.Ignore;
			style.position = Position.Absolute;

			AddToClassList("count-badge");

			RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);

			text = _Count.ToString();
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