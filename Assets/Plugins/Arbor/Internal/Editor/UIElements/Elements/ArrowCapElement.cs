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
	using ArborEditor.UnityEditorBridge.UIElements;
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	internal sealed class ArrowCapElement : VisualElement
	{
		private Vector3 _Position;
		public Vector3 position
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
					MarkDirtyRepaint();
				}
			}
		}

		private float _Width = EditorGUITools.kBranchArrowWidth;
		public float width
		{
			get
			{
				return _Width;
			}
			set
			{
				if (_Width != value)
				{
					_Width = value;
					MarkDirtyRepaint();
				}
			}
		}

		private Vector3 _Direction;
		public Vector3 direction
		{
			get
			{
				return _Direction;
			}
			set
			{
				if (_Direction != value)
				{
					_Direction = value;
					MarkDirtyRepaint();
				}
			}
		}

		private Color _Color = Color.white;
		public Color color
		{
			get
			{
				return _Color;
			}
			set
			{
				if (_Color != value)
				{
					_Color = value;
					MarkDirtyRepaint();
				}
			}
		}

		private bool _ContainsPreciseShape = false;
		public bool containsPreciseShape
		{
			get
			{
				return _ContainsPreciseShape;
			}
			set
			{
				if (_ContainsPreciseShape != value)
				{
					_ContainsPreciseShape = value;
				}
			}
		}

		public ArrowCapElement()
		{
			generateVisualContent += GenerateVisualContent;
		}

		static void GetVertex(Vector3 position, Vector3 direction, float width, out Vector3 p1, out Vector3 p2, out Vector3 p3)
		{
			Vector3 cross = Vector3.Cross(direction, Vector3.forward).normalized;

			float halfWidth = width * 0.5f;
			Vector3 halfCross = cross * halfWidth;

			p1 = position + direction * width;
			p2 = position - halfCross;
			p3 = position + halfCross;
		}

		public override bool ContainsPoint(Vector2 localPoint)
		{
			if (!base.ContainsPoint(localPoint))
			{
				return false;
			}

			if (!_ContainsPreciseShape)
			{
				return true;
			}

			Vector3 position = parent.ChangeCoordinatesTo(this, _Position);
			GetVertex(position, _Direction, _Width, out Vector3 p1, out Vector3 p2, out Vector3 p3);

			Vector3 d, e;
			float w1, w2;
			d = p2 - p1;
			e = p3 - p1;
			w1 = (e.x * (p1.y - localPoint.y) + e.y * (localPoint.x - p1.x)) / (d.x * e.y - d.y * e.x);
			w2 = (localPoint.y - p1.y - w1 * d.y) / e.y;
			return (w1 >= 0.0) && (w2 >= 0.0) && ((w1 + w2) <= 1.0);
		}

		void GenerateVisualContent(MeshGenerationContext mgc)
		{
			var meshData = mgc.Allocate(3, 3);

			Vector3 position = parent.ChangeCoordinatesTo(this, _Position);
			GetVertex(position, _Direction, _Width, out Vector3 p1, out Vector3 p2, out Vector3 p3);

			p1.z = Vertex.nearZ;
			p2.z = Vertex.nearZ;
			p3.z = Vertex.nearZ;

			Color color = _Color * UIElementsUtilityBridge.editorPlayModeTintColor;

			meshData.SetNextVertex(new Vertex() { position = p1, tint = color });
			meshData.SetNextVertex(new Vertex() { position = p2, tint = color });
			meshData.SetNextVertex(new Vertex() { position = p3, tint = color });

			meshData.SetNextIndex(0);
			meshData.SetNextIndex(1);
			meshData.SetNextIndex(2);
		}

		public void UpdateLayout()
		{
			if (parent == null)
			{
				return;
			}

			Vector3 position = _Position;
			GetVertex(position, _Direction, _Width, out Vector3 p1, out Vector3 p2, out Vector3 p3);
			
			Vector3 min = p1;
			Vector3 max = p1;

			for (int i = 0; i < 2; i++)
			{
				min[i] = Mathf.Min(p2[i], min[i]);
				min[i] = Mathf.Min(p3[i], min[i]);

				max[i] = Mathf.Max(p2[i], max[i]);
				max[i] = Mathf.Max(p3[i], max[i]);
			}

			Rect rect = Rect.MinMaxRect(min.x, min.y, max.x, max.y);

			if (layout != rect)
			{
				this.SetLayout(rect);
			}
		}
	}
}