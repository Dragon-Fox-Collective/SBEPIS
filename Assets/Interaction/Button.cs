using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(Collider))]
	public class Button : MonoBehaviour
	{
		public ButtonEvent onPressed;
	}

	[Serializable]
	public class ButtonEvent : UnityEvent<ItemHolder> { }
}