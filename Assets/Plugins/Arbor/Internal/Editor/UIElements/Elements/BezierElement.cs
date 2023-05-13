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

	internal sealed class BezierElement : VisualElement
	{
		private Vector2 _StartPosition;
		private Vector2 _StartControl;
		private Vector2 _EndPosition;
		private Vector2 _EndControl;
		private bool _Shadow;
		private Color _ShadowColor;
		private float _EdgeWidth;

		public Vector2 startPosition
		{
			get
			{
				return _StartPosition;
			}
			set
			{
				if (_StartPosition != value)
				{
					_StartPosition = value;
					_IsDirtyLocalBezier = true;
					MarkDirtyRepaint();
				}
			}
		}

		public Vector2 startControl
		{
			get
			{
				return _StartControl;
			}
			set
			{
				if (_StartControl != value)
				{
					_StartControl = value;
					_IsDirtyLocalBezier = true;
					MarkDirtyRepaint();
				}
			}
		}

		public Vector2 endPosition
		{
			get
			{
				return _EndPosition;
			}
			set
			{
				if (_EndPosition != value)
				{
					_EndPosition = value;
					_IsDirtyLocalBezier = true;
					MarkDirtyRepaint();
				}
			}
		}

		public Vector2 endControl
		{
			get
			{
				return _EndControl;
			}
			set
			{
				if (_EndControl != value)
				{
					_EndControl = value;
					_IsDirtyLocalBezier = true;
					MarkDirtyRepaint();
				}
			}
		}

		public bool shadow
		{
			get
			{
				return _Shadow;
			}
			set
			{
				if (_Shadow != value)
				{
					_Shadow = value;
					MarkDirtyRepaint();
				}
			}
		}

		public Color shadowColor
		{
			get
			{
				return _ShadowColor;
			}
			set
			{
				if (_ShadowColor != value)
				{
					_ShadowColor = value;
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
					MarkDirtyRepaint();
				}
			}
		}

		private Color _LineColor = Color.white;
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
					if (_ArrowCapElement != null)
					{
						_ArrowCapElement.color = _LineColor;
					}
					MarkDirtyRepaint();
				}
			}
		}

		private ArrowCapElement _ArrowCapElement;

		public VisualElement arrowCapElement
		{
			get
			{
				return _ArrowCapElement;
			}
		}

		private bool _Arrow;
		public bool arrow
		{
			get
			{
				return _Arrow;
			}
			set
			{
				if (_Arrow != value)
				{
					_Arrow = value;

					_IsDirtyLocalBezier = true;

					if (_Arrow)
					{
						if (_ArrowCapElement == null)
						{
							_ArrowCapElement = new ArrowCapElement()
							{
								containsPreciseShape = true,
								width = _ArrowWidth,
							};
							_ArrowCapElement.color = _LineColor;
						}

						if (_ArrowCapElement.parent == null)
						{
							Add(_ArrowCapElement);
						}
					}
					else
					{
						if (_ArrowCapElement != null && _ArrowCapElement.parent != null)
						{
							_ArrowCapElement.RemoveFromHierarchy();
						}
					}
				}
			}
		}

		private float _ArrowWidth = EditorGUITools.kBranchArrowWidth;
		public float arrowWidth
		{
			get
			{
				return _ArrowWidth;
			}
			set
			{
				if (_ArrowWidth != value)
				{
					_ArrowWidth = value;

					_IsDirtyLocalBezier = true;

					if (_ArrowCapElement != null)
					{
						_ArrowCapElement.width = value;
					}
				}
			}
		}

		private bool _IsHover;
		public bool isHover
		{
			get
			{
				return _IsHover;
			}
		}

		private bool _Hoverable = true;
		public bool hoverable
		{
			get
			{
				return _Hoverable;
			}
			set
			{
				if (_Hoverable != value)
				{
					_Hoverable = value;
					MarkDirtyRepaint();
				}
			}
		}

		private VisualElement _Viewport;
		private Bezier2D _LocalBezier;
		private bool _IsDirtyLocalBezier = true;

		public BezierElement(VisualElement viewport)
		{
			_Viewport = viewport;
			_LocalBezier = new Bezier2D();

			RegisterCallback<MouseOverEvent>(OnMouseOver);
			RegisterCallback<MouseOutEvent>(OnMouseOut);

			generateVisualContent += GenerateVisualContent;
		}

		void OnMouseOver(MouseOverEvent evt)
		{
			if (!_IsHover)
			{
				_IsHover = true;
				MarkDirtyRepaint();
			}
		}

		void OnMouseOut(MouseOutEvent evt)
		{
			if (_IsHover)
			{
				_IsHover = false;
				MarkDirtyRepaint();
			}
		}

		public override bool ContainsPoint(Vector2 localPoint)
		{
			if (!base.ContainsPoint(localPoint))
			{
				return false;
			}

			UpdateLocalBezier();

			float d = HandleUtility.DistancePointBezier(localPoint, _LocalBezier.startPosition, _LocalBezier.endPosition, _LocalBezier.startControl, _LocalBezier.endControl);
			return d <= edgeWidth * 0.5f;
		}

		void UpdateLocalBezier()
		{
			if (!_IsDirtyLocalBezier)
			{
				return;
			}

			Vector2 v = Vector2.zero;
			if (_Arrow)
			{
				Vector2 direction = (_EndPosition - _EndControl).normalized;
				v = direction * _ArrowCapElement.width;
			}

			_LocalBezier.startPosition = _Viewport.ChangeCoordinatesTo(this, _StartPosition);
			_LocalBezier.startControl = _Viewport.ChangeCoordinatesTo(this, _StartControl);
			_LocalBezier.endPosition = _Viewport.ChangeCoordinatesTo(this, _EndPosition - v);
			_LocalBezier.endControl = _Viewport.ChangeCoordinatesTo(this, _EndControl - v);

			_IsDirtyLocalBezier = false;
		}

		public void UpdateLayout()
		{
			if (parent == null)
			{
				return;
			}

			Vector2 v = Vector2.zero;
			Vector2 direction = Vector2.right;
			if (arrow)
			{
				direction = (_EndPosition - _EndControl).normalized;
				v = direction * _ArrowCapElement.width;
			}

			Vector2 startPosition = _Viewport.ChangeCoordinatesTo(parent, _StartPosition);
			Vector2 startControl = _Viewport.ChangeCoordinatesTo(parent, _StartControl);
			Vector2 endPosition = _Viewport.ChangeCoordinatesTo(parent, _EndPosition - v);
			Vector2 endControl = _Viewport.ChangeCoordinatesTo(parent, _EndControl - v);

			Rect rect = Bezier2D.GetBoundingBox(startPosition, startControl, endPosition, endControl);

			float margin = (_EdgeWidth + 6) * 0.5f;
			rect.xMin -= margin;
			rect.yMin -= margin;
			rect.xMax += margin;
			rect.yMax += margin;

			if (layout != rect)
			{
				_IsDirtyLocalBezier = true;
				this.SetLayout(rect);
			}

			if (_Arrow && _ArrowCapElement != null)
			{
				UpdateLocalBezier();

				_ArrowCapElement.position = _LocalBezier.endPosition;
				_ArrowCapElement.direction = direction;
				_ArrowCapElement.UpdateLayout();
			}
		}

		void GenerateVisualContent(MeshGenerationContext mgc)
		{
			UpdateLocalBezier();

			bool isHover = _Hoverable && _IsHover;

			if (isHover)
			{
				EditorGUITools.GenerateBezier(mgc, _LocalBezier, Vector2.zero, Color.cyan, Color.cyan, _EdgeWidth + 6f, 40, EditorContents.selectedConnectionTexture);
			}

			if (_Shadow && !isHover)
			{
				EditorGUITools.GenerateBezier(mgc, _LocalBezier, Vector2.one * 3, _ShadowColor, _ShadowColor, _EdgeWidth, 40, EditorContents.connectionTexture);
			}

			EditorGUITools.GenerateBezier(mgc, _LocalBezier, Vector2.zero, _LineColor, _LineColor, _EdgeWidth, 40, EditorContents.connectionTexture);
		}
	}
}