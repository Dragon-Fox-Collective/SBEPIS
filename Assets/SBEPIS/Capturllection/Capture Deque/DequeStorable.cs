using System;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(LocalTransformLerper))]
	public class DequeStorable : MonoBehaviour
	{
		public BoxCollider boxCollider;

		[NonSerialized]
		public LocalTransformLerper lerper;

		private void Awake()
		{
			lerper = GetComponent<LocalTransformLerper>();
		}
	}
}
