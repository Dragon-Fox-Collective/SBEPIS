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
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	internal sealed class PolyLineElement : VisualElement
	{
		private List<Vector2> _Points = new List<Vector2>();
		private float _EdgeWidth;
		private Color _LineColor = Color.white;
		private bool _Loop;

		private List<Vector2> _LocalPoints = new List<Vector2>();

		public int pointCount
		{
			get
			{
				return _Points.Count;
			}
			set
			{
				if (_Points.Count != value)
				{
					if (_Points.Count > value)
					{
						int removeCount = _Points.Count - value;
						_Points.RemoveRange(_Points.Count - removeCount, removeCount);
					}
					else
					{
						int addCount = value - _Points.Count;
						for (int i = 0; i < addCount; i++)
						{
							_Points.Add(Vector2.zero);
						}
					}

					UpdateLayout();
					MarkDirtyRepaint();
				}
			}
		}
		
		public float edgeWidth
		{
			get
			{
				return _EdgeWidth;
			}
			set
			{
				if (_EdgeWidth != value)
				{
					_EdgeWidth = value;
					UpdateLayout();
					MarkDirtyRepaint();
				}
			}
		}

		public Color lineColor
		{
			get
			{
				return _LineColor;
			}
			set
			{
				if (_LineColor != value)
				{
					_LineColor = value;
					MarkDirtyRepaint();
				}
			}
		}

		public bool loop
		{
			get
			{
				return _Loop;
			}
			set
			{
				if (_Loop != value)
				{
					_Loop = value;
					MarkDirtyRepaint();
				}
			}
		}

		public PolyLineElement()
		{
			generateVisualContent += GenereteVisualContent;
		}

		public void SetPoint(int index, Vector2 point)
		{
			if (_Points[index] != point)
			{
				_Points[index] = point;

				UpdateLayout();
				MarkDirtyRepaint();
			}
		}

		public void AddPoint(Vector2 point)
		{
			_Points.Add(point);

			UpdateLayout();
			MarkDirtyRepaint();
		}

		public void SetPoints(params Vector2[] points)
		{
			SetPoints((IList<Vector2>)points);
		}

		public void SetPoints(IList<Vector2> points)
		{
			bool changed = false;

			int newCount = points.Count;
			int oldCount = _Points.Count;
			if (newCount < oldCount)
			{
				int removeCount = oldCount - newCount;
				_Points.RemoveRange(oldCount - removeCount, removeCount);
				oldCount = newCount;
				changed = true;
			}

			for (int i = 0; i < oldCount; i++)
			{
				if (_Points[i] != points[i])
				{
					_Points[i] = points[i];
					changed = true;
				}
			}

			if (newCount > oldCount)
			{
				int addCount = newCount - oldCount;
				for (int i = 0; i < addCount; i++)
				{
					_Points.Add(points[oldCount + i]);
				}
				changed = true;
			}

			if (changed)
			{
				UpdateLayout();
				MarkDirtyRepaint();
			}
		}

		public override bool ContainsPoint(Vector2 localPoint)
		{
			if (!base.ContainsPoint(localPoint))
			{
				return false;
			}

			for (int i = 0; i < _LocalPoints.Count - 1; i++)
			{
				float d = HandleUtility.DistancePointLine(localPoint, _LocalPoints[i], _LocalPoints[i + 1]);
				if (d <= _EdgeWidth * 0.5f)
				{
					return true;
				}
			}

			return false;
		}

		void UpdateLocalPoints()
		{
			int oldCount = _LocalPoints.Count;
			int newCount = _Points.Count;

			if (oldCount > newCount)
			{
				int removeCount = oldCount - newCount;
				_LocalPoints.RemoveRange(oldCount - removeCount, removeCount);
				oldCount = newCount;
			}

			for (int i = 0; i < oldCount; i++)
			{
				_LocalPoints[i] = parent.ChangeCoordinatesTo(this, _Points[i]);
			}

			if (oldCount < newCount)
			{
				int addCount = newCount - oldCount;
				for (int i = 0; i < addCount; i++)
				{
					_LocalPoints.Add(parent.ChangeCoordinatesTo(this, _Points[oldCount+i]));
				}
			}
		}

		public void UpdateLayout()
		{
			if (parent == null)
			{
				return;
			}

			if (_Points.Count < 2)
			{
				return;
			}

			Vector2 startPoint = _Points[0];
			
			Vector2 min = startPoint;
			Vector2 max = startPoint;

			for (int i = 1; i < _Points.Count; i++)
			{
				var point = _Points[i];
				min = Vector2.Min(min, point);
				max = Vector2.Max(max, point);
			}

			Rect rect = Rect.MinMaxRect(min.x, min.y, max.x, max.y);

			float margin = _EdgeWidth * 0.5f;
			rect.xMin -= margin;
			rect.yMin -= margin;
			rect.xMax += margin;
			rect.yMax += margin;

			if (layout != rect)
			{
				this.SetLayout(rect);
			}

			UpdateLocalPoints();
		}

		void GenereteVisualContent(MeshGenerationContext mgc)
		{
			if (_LocalPoints.Count < 2)
			{
				return;
			}

			EditorGUITools.GeneratePolyLine(mgc, _LocalPoints, _LineColor, _EdgeWidth, _Loop, EditorContents.outlineConnectionTexture);
		}
	}
}