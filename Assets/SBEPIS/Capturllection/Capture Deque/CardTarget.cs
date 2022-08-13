using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class CardTarget : MonoBehaviour
	{
		public float lifetime { get; private set; }

		private void FixedUpdate()
		{
			lifetime += Time.fixedDeltaTime;
		}
	}
}
