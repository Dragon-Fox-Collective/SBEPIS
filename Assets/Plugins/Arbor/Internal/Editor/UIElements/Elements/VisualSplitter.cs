//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor.UIElements
{
	internal class VisualSplitter : ImmediateModeElement
	{
		private const int kDefalutSplitSize = 6;
		public int splitSize = kDefalutSplitSize;

		public VisualSplitter() : base()
		{
			this.AddManipulator(new SplitManipulator());
		}

		public VisualElement[] GetAffectedVisualElements()
		{
			List<VisualElement> visualElementList = new List<VisualElement>();
			for (int index = 0; index < hierarchy.childCount; ++index)
			{
				VisualElement visualElement = hierarchy[index];
				if (visualElement.resolvedStyle.position == Position.Relative)
				{
					visualElementList.Add(visualElement);
				}
			}
			return visualElementList.ToArray();
		}

		public static bool IsHorizontal(FlexDirection flexDirection)
		{
			return flexDirection == FlexDirection.Row || flexDirection == FlexDirection.RowReverse;
		}

		protected override void ImmediateRepaint()
		{
			for (int index = 0; index < this.hierarchy.childCount - 1; ++index)
			{
				EditorGUIUtility.AddCursorRect(GetSplitterRect(hierarchy[index]), IsHorizontal(resolvedStyle.flexDirection) ? MouseCursor.SplitResizeLeftRight : MouseCursor.ResizeVertical);
			}
		}

		public Rect GetSplitterRect(VisualElement visualElement)
		{
			Rect layout = visualElement.layout;
			FlexDirection flexDirection = resolvedStyle.flexDirection;
			switch (flexDirection)
			{
				case FlexDirection.Row:
					layout.xMin = visualElement.layout.xMax - splitSize * 0.5f;
					layout.xMax = visualElement.layout.xMax + splitSize * 0.5f;
					break;
				case FlexDirection.RowReverse:
					layout.xMin = visualElement.layout.xMin - splitSize * 0.5f;
					layout.xMax = visualElement.layout.xMin + splitSize * 0.5f;
					break;
				case FlexDirection.Column:
					layout.yMin = visualElement.layout.yMax - splitSize * 0.5f;
					layout.yMax = visualElement.layout.yMax + splitSize * 0.5f;
					break;
				case FlexDirection.ColumnReverse:
					layout.yMin = visualElement.layout.yMin - splitSize * 0.5f;
					layout.yMax = visualElement.layout.yMin + splitSize * 0.5f;
					break;
			}
			return layout;
		}

		private sealed class SplitManipulator : DragManipulator
		{
			private int m_ActiveVisualElementIndex;
			private int m_NextVisualElementIndex;
			private VisualElement[] m_AffectedElements;

			public SplitManipulator() : base(TrickleDownMode.TrickleDown)
			{
				m_ActiveVisualElementIndex = -1;
				m_NextVisualElementIndex = -1;

				ManipulatorActivationFilter activationFilter = new ManipulatorActivationFilter();
				activationFilter.button = MouseButton.LeftMouse;
				activators.Add(activationFilter);
			}

			protected override void OnMouseDown(MouseDownEvent e)
			{
				VisualSplitter target = this.target as VisualSplitter;
				FlexDirection flexDirection = target.resolvedStyle.flexDirection;
				m_AffectedElements = target.GetAffectedVisualElements();
				for (int index = 0; index < m_AffectedElements.Length - 1; ++index)
				{
					VisualElement affectedElement = m_AffectedElements[index];
					if (target.GetSplitterRect(affectedElement).Contains(e.localMousePosition))
					{
						if (flexDirection == FlexDirection.RowReverse || flexDirection == FlexDirection.ColumnReverse)
						{
							m_ActiveVisualElementIndex = index + 1;
							m_NextVisualElementIndex = index;
						}
						else
						{
							m_ActiveVisualElementIndex = index;
							m_NextVisualElementIndex = index + 1;
						}
						e.StopPropagation();
					}
				}
			}

			protected override void OnMouseMove(MouseMoveEvent e)
			{
				VisualSplitter visualSplitter = this.target as VisualSplitter;
				VisualElement visualElement = m_AffectedElements[m_ActiveVisualElementIndex];
				VisualElement nextVisualElement = m_AffectedElements[m_NextVisualElementIndex];

				FlexDirection flexDirection = visualSplitter.resolvedStyle.flexDirection;

				if (IsHorizontal(flexDirection))
				{
					float minWidth = visualElement.resolvedStyle.minWidth == StyleKeyword.Auto ? 0 : visualElement.resolvedStyle.minWidth.value;
					float nextMinWidth = nextVisualElement.resolvedStyle.minWidth == StyleKeyword.Auto ? 0 : nextVisualElement.resolvedStyle.minWidth.value;
					float availableWidth = visualElement.layout.width + nextVisualElement.layout.width - nextMinWidth;
					float maxWidth = visualElement.resolvedStyle.maxWidth.value <= 0 ? availableWidth : visualElement.resolvedStyle.maxWidth.value;

					float pos = Math.Min(e.localMousePosition.x, visualElement.layout.xMin + maxWidth);
					float relativeMousePosition = pos - visualElement.layout.xMin;

					visualElement.style.width = Mathf.Max(relativeMousePosition, minWidth);
				}
				else
				{
					float minHeight = visualElement.resolvedStyle.minHeight == StyleKeyword.Auto ? 0 : visualElement.resolvedStyle.minHeight.value;
					float nextMinHeight = nextVisualElement.resolvedStyle.minHeight == StyleKeyword.Auto ? 0 : nextVisualElement.resolvedStyle.minHeight.value;
					float availableHeight = visualElement.layout.height + nextVisualElement.layout.height - nextMinHeight;
					float maxHeight = visualElement.resolvedStyle.maxHeight.value <= 0 ? availableHeight : visualElement.resolvedStyle.maxHeight.value;

					float pos = Math.Min(e.localMousePosition.y, visualElement.layout.yMin + maxHeight);
					float relativeMousePosition = pos - visualElement.layout.yMin;

					visualElement.style.height = Mathf.Max(relativeMousePosition, minHeight);
				}
				
				e.StopPropagation();
			}

			protected override void OnEndDrag()
			{
				base.OnEndDrag();

				m_ActiveVisualElementIndex = -1;
				m_NextVisualElementIndex = -1;
			}
		}
	}
}