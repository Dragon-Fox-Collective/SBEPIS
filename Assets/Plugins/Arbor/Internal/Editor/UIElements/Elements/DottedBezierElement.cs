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

	internal sealed class DottedBezierElement : VisualElement
	{
		private Vector2 _StartPosition;
		private Vector2 _StartControl;
		private Vector2 _EndPosition;
		private Vector2 _EndControl;
		private Color _StartColor;
		private Color _EndColor;
		private bool _Shadow;
		private Vector2 _ShadowOffset = Vector2.one * 3;
		private Color _ShadowColor;
		private float _EdgeWidth;
		private float _Space;
		private Texture2D _Tex;

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

		public Color startColor
		{
			get
			{
				return _StartColor;
			}
			set
			{
				if (_StartColor != value)
				{
					_StartColor = value;
					_IsDirtyLocalBezier = true;
					MarkDirtyRepaint();
				}
			}
		}

		public Color endColor
		{
			get
			{
				return _EndColor;
			}
			set
			{
				if (_EndColor != value)
				{
					_EndColor = value;
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

		public Vector2 shadowOffset
		{
			get
			{
				return _ShadowOffset;
			}
			set
			{
				if (_ShadowOffset != value)
				{
					_ShadowOffset = value;
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
					UpdateLayout();
					MarkDirtyRepaint();
				}
			}
		}

		public float space
		{
			get
			{
				return _Space;
			}
			set
			{
				if (_Space != value)
				{
					_Space = value;
					MarkDirtyRepaint();
				}
			}
		}

		public Texture2D tex
		{
			get
			{
				return _Tex;
			}
			set
			{
				if (_Tex != value)
				{
					_Tex = value;
					MarkDirtyRepaint();
				}
			}
		}

		private VisualElement _Viewport;
		private Bezier2D _LocalBezier;
		private bool _IsDirtyLocalBezier = true;

		private bool _IsHover;
		public bool isHover
		{
			get
			{
				return _IsHover;
			}
		}

		public DottedBezierElement(VisualElement viewport)
		{
			_Viewport = viewport;

			_LocalBezier = new Bezier2D();

			generateVisualContent += GenerateVisualContent;

			RegisterCallback<MouseOverEvent>(OnMouseOver);
			RegisterCallback<MouseOutEvent>(OnMouseOut);
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
			return (d <= _EdgeWidth * 0.5f);
		}

		void UpdateLocalBezier()
		{
			if (!_IsDirtyLocalBezier)
			{
				return;
			}

			_LocalBezier.startPosition = _Viewport.ChangeCoordinatesTo(this, _StartPosition);
			_LocalBezier.startControl = _Viewport.ChangeCoordinatesTo(this, _StartControl);
			_LocalBezier.endPosition = _Viewport.ChangeCoordinatesTo(this, _EndPosition);
			_LocalBezier.endControl = _Viewport.ChangeCoordinatesTo(this, _EndControl);

			_IsDirtyLocalBezier = false;
		}

		public void UpdateLayout()
		{
			if (parent == null)
			{
				return;
			}

			Vector2 startPosition = _Viewport.ChangeCoordinatesTo(parent, _StartPosition);
			Vector2 startControl = _Viewport.ChangeCoordinatesTo(parent, _StartControl);
			Vector2 endPosition = _Viewport.ChangeCoordinatesTo(parent, _EndPosition);
			Vector2 endControl = _Viewport.ChangeCoordinatesTo(parent, _EndControl);

			Vector2 min = startPosition;
			Vector2 max = startPosition;

			for (int i = 0; i < 2; i++)
			{
				min[i] = Mathf.Min(startControl[i], min[i]);
				min[i] = Mathf.Min(endPosition[i], min[i]);
				min[i] = Mathf.Min(endControl[i], min[i]);

				max[i] = Mathf.Max(startControl[i], max[i]);
				max[i] = Mathf.Max(endPosition[i], max[i]);
				max[i] = Mathf.Max(endControl[i], max[i]);
			}

			Rect rect = Rect.MinMaxRect(min.x, min.y, max.x, max.y);

			float margin = _EdgeWidth;
			rect.xMin -= margin;
			rect.yMin -= margin;
			rect.xMax += margin;
			rect.yMax += margin;

			if (layout != rect)
			{
				this.SetLayout(rect);
			}
		}

		void GenerateVisualContent(MeshGenerationContext mgc)
		{
			UpdateLocalBezier();

			if (_IsHover)
			{
				EditorGUITools.GenerateBezier(mgc, _LocalBezier, Vector2.zero, Color.cyan, Color.cyan, _EdgeWidth - 5f, 40, EditorContents.selectedConnectionTexture);
			}

			EditorGUITools.GenerateDottedBezier(mgc, _LocalBezier, _StartColor, _EndColor, _EdgeWidth, _Space, _Shadow, _ShadowOffset, _ShadowColor, _Tex);
		}
	}
}