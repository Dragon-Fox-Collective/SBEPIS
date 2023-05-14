//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace ArborEditor.UIElements
{
	internal class ToolbarDropdown : ToolbarButton
	{
		public override string text
		{
			get
			{
				return base.text;
			}
			set
			{
				m_TextElement.text = value;
				base.text = value;
			}
		}

		public new static readonly string ussClassName = "unity-toolbar-menu";
		public static readonly string textUssClassName = ussClassName + "__text";
		public static readonly string arrowUssClassName = ussClassName + "__arrow";

		private TextElement m_TextElement;
		private VisualElement m_ArrowElement;

		public ToolbarDropdown(System.Action clickEvent) : base(clickEvent)
		{
			generateVisualContent = null;

			RemoveFromClassList(ToolbarButton.ussClassName);
			AddToClassList(ussClassName);

			m_TextElement = new TextElement();
			m_TextElement.AddToClassList(textUssClassName);
			m_TextElement.pickingMode = PickingMode.Ignore;
			Add(m_TextElement);

			m_ArrowElement = new VisualElement();
			m_ArrowElement.AddToClassList(arrowUssClassName);
			m_ArrowElement.pickingMode = PickingMode.Ignore;
			Add(m_ArrowElement);
		}
	}
}