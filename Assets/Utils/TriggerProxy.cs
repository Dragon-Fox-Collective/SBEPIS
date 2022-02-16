using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Utils
{
	public class TriggerProxy : MonoBehaviour
	{
		public TriggerEvent onTriggerEnter, onTriggerStay, onTriggerExit;

		private void OnTriggerEnter(Collider other)
		{
			onTriggerEnter.Invoke(other);
		}

		private void OnTriggerStay(Collider other)
		{
			onTriggerStay.Invoke(other);
		}

		private void OnTriggerExit(Collider other)
		{
			onTriggerExit.Invoke(other);
		}

		[Serializable]
		public class TriggerEvent : UnityEvent<Collider> { }
	}
}
