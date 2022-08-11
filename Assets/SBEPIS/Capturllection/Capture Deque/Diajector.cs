using SBEPIS.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class Diajector : MonoBehaviour
	{
		public Transform upperTarget;
		public GameObject cardPrefab;
		public DequePage mainPage;
		public float cardDelay = 0.5f;
		public float pageTime = 1;

		public Rigidbody staticRigidbody;
		public StrengthSettings cardStrength;

		public void StartAssembly(CaptureDeque deque)
		{
			gameObject.SetActive(true);
			mainPage.StartAssembly(deque, this, staticRigidbody, cardStrength);
		}

		public void StartDisassembly(CaptureDeque deque)
		{
			mainPage.StartDisassembly(deque, this);
			gameObject.SetActive(false);
		}
	}
}
