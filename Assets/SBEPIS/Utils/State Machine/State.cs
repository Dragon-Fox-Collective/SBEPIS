using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Utils
{
	public class State : MonoBehaviour
	{
		public string stateName;
		
		public UnityEvent onEnter = new();
		public UnityEvent onUpdate = new();
		public UnityEvent onExit = new();
	}
}
