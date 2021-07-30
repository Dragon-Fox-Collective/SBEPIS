using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WrightWay.SBEPIS
{
	[RequireComponent(typeof(Collider))]
	public class Button : MonoBehaviour
	{
		public ButtonEvent onPressed;
	}

	[Serializable]
	public class ButtonEvent : UnityEvent<Player> { }
}