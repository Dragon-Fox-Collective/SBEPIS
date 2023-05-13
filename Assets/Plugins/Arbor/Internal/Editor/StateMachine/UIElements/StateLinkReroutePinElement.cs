using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor.UIElements
{
	internal class StateLinkReroutePinElement : VisualElement
	{
		private readonly VisualElement _PinIcon;

		public StyleColor pinColor
		{
			get => _PinIcon.style.unityBackgroundImageTintColor;
			set
			{
				_PinIcon.style.unityBackgroundImageTintColor = value;
			}
		}

		public StateLinkReroutePinElement()
		{
			AddToClassList("pin-center");

			_PinIcon = new VisualElement();
			_PinIcon.AddToClassList("pin");
			Add(_PinIcon);
		}

		public void EnableActive(bool active)
		{
			_PinIcon.EnableInClassList("pin-active", active);
		}
	}
}