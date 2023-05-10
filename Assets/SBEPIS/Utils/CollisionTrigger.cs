using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Utils
{
	public class CollisionTrigger : MonoBehaviour
	{
		public float primeDelay = 0.2f;
		public UnityEvent trigger = new();
		
		public bool IsDelaying { get; private set; }
		public bool IsPrimed { get; private set; }
		public float TimeSinceStart { get; private set; }
		
		private void FixedUpdate()
		{
			if (IsDelaying)
			{
				TimeSinceStart += Time.fixedDeltaTime;
				if (TimeSinceStart > primeDelay)
					Prime();
			}
		}

		private void OnCollisionEnter()
		{
			if (IsPrimed)
				Trigger();
		}

		public void StartPrime()
		{
			IsDelaying = true;
			IsPrimed = false;
			TimeSinceStart = 0;
		}

		public void Prime()
		{
			IsDelaying = false;
			IsPrimed = true;
		}

		public void Trigger()
		{
			CancelPrime();
			trigger.Invoke();
		}

		public void CancelPrime()
		{
			IsDelaying = false;
			IsPrimed = false;
		}
	}
}
