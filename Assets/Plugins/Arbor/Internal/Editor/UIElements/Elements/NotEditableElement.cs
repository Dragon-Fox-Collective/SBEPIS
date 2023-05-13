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
	internal sealed class NotEditableElement : VisualElement
	{
		private Label _Label;

		public NotEditableElement()
		{
			pickingMode = PickingMode.Ignore;
			AddToClassList("not-editable");

			_Label = new Label();
			_Label.AddToClassList("graph-label");
			_Label.AddManipulator(new LocalizationManipulator("NotEditable", LocalizationManipulator.TargetText.Text));
			UIElementsUtility.SetBoldFont(_Label);
			Add(_Label);
		}
	}
}