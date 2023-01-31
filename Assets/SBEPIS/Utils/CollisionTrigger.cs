using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Utils
{
	public class CollisionTrigger : MonoBehaviour
	{
		public float primeDelay;
		public UnityEvent trigger;

		private bool delaying;
		private bool primed;
		private float timeSinceStart;

		private void FixedUpdate()
		{
			if (delaying)
			{
				timeSinceStart += Time.fixedDeltaTime;
				if (timeSinceStart > primeDelay)
					Prime();
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (primed)
				Trigger();
		}

		public void StartPrime()
		{
			delaying = true;
			primed = false;
			timeSinceStart = 0;
		}

		public void Prime()
		{
			delaying = false;
			primed = true;
		}

		public void Trigger()
		{
			CancelPrime();
			trigger.Invoke();
		}

		public void CancelPrime()
		{
			delaying = false;
			primed = false;
		}
	}
}
