using System;
using SBEPIS.Capturllection;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Predicates
{
	[RequireComponent(typeof(DequeOwner))]
	public class IsDequeOwnedByDequeOwnerPredicate : GameObjectPredicate
	{
		private DequeOwner dequeOwner;
		
		private void Awake()
		{
			dequeOwner = GetComponent<DequeOwner>();
		}
		
		public override bool IsTrue(GameObject obj) => obj == dequeOwner.Deque.gameObject;
	}
}
