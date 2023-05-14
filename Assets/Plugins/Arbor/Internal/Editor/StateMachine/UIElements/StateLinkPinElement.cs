using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArborEditor.UIElements
{
	internal sealed class StateLinkPinElement : VisualElement
	{
		private bool _IsPinRight;

		public bool isPinRight
		{
			get => _IsPinRight;
			set
			{
				if (_IsPinRight != value)
				{
					_IsPinRight = value;

					SetPinDirectionClass();
				}
			}
		}

		public StateLinkPinElement()
		{
			AddToClassList("pin");

			isPinRight = true;
		}

		void SetPinDirectionClass()
		{
			EnableInClassList("right", _IsPinRight);
			EnableInClassList("left", !_IsPinRight);
		}
	}
}