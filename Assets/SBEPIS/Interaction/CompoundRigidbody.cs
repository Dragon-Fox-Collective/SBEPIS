using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(Rigidbody))]
	public class CompoundRigidbody : MonoBehaviour
	{
		[NonSerialized]
		public new Rigidbody rigidbody;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void Start()
		{
			Recalculate();
		}

		public void Recalculate()
		{
			RigidbodyPiece[] pieces = GetComponentsInChildren<RigidbodyPiece>();
			if (pieces.Length == 0)
				return;
		}
	}
}
