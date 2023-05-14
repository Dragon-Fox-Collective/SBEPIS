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
	internal sealed class GraphMainLayout : VisualSplitter
	{
		public VisualElement leftPanel
		{
			get; private set;
		}

		public VisualElement rightPanel
		{
			get; private set;
		}

		public GraphMainLayout() : base()
		{
			pickingMode = PickingMode.Ignore;
			style.flexDirection = FlexDirection.Row;
			style.flexGrow = 1f;

			leftPanel = new VisualElement()
			{
				name = "LeftPanel",
				style =
				{
					minWidth = 230.0f,
					width = ArborSettings.sidePanelWidth,
					borderRightColor = EditorGUITools.GetSplitColor(),
					borderRightWidth = 1f,
				}
			};

			leftPanel.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedLeftPanel);

			rightPanel = new VisualElement()
			{
				name = "RightPanel",
				style =
				{
					minWidth = 150.0f,
					flexGrow = 1f,
					flexShrink = 0f,
					flexBasis = 0f,
				}
			};

			Add(rightPanel);

			ShowLeftPanel(ArborSettings.openSidePanel);
		}

		public void ShowLeftPanel(bool show)
		{
			if (show)
			{
				if (leftPanel.parent == null)
				{
					Insert(0, leftPanel);
				}
			}
			else if(leftPanel.parent != null)
			{
				leftPanel.RemoveFromHierarchy();
			}
		}

		void OnGeometryChangedLeftPanel(GeometryChangedEvent e)
		{
			ArborSettings.sidePanelWidth = leftPanel.layout.width;
		}
	}
}