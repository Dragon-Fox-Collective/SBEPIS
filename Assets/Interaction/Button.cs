using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Interaction
{
	public class Button : MonoBehaviour
	{
		public ButtonEvent onPressed;
	}

	[Serializable]
	public class ButtonEvent : UnityEvent<ItemHolder> { }
}