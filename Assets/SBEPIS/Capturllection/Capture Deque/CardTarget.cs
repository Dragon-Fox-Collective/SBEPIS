using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class CardTarget : MonoBehaviour
	{
		public DequeStorable card { get; set; }
		public float lifetime { get; private set; }
		public bool isTemporary { get; set; }

		private void FixedUpdate()
		{
			lifetime += Time.fixedDeltaTime;
		}
	}
}
