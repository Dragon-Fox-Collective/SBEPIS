using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	[CreateAssetMenu(menuName="Deque/"+nameof(StackDeque))]
	public class StackDeque : DequeType
	{
		public float separatingDistance = 0.1f;
		public Quaternion cardRotation = Quaternion.identity;

		private float time;
		
		public override void Tick(List<DequeStorable> cards, float delta) { }

		public override void LayoutTargets(List<DequeStorable> cards, Dictionary<DequeStorable, CardTarget> targets)
		{
			Vector3 right = separatingDistance * (targets.Count - 1) / 2 * Vector3.left;
			foreach ((DequeStorable card, CardTarget target) in targets)
			{
				target.transform.localPosition = right;
				target.transform.localRotation = cardRotation;
				right += Vector3.right * separatingDistance;
			}
		}

		public override bool CanFetch(List<DequeStorable> cards, DequeStorable card) => cards[0] == card;
		
		public override int GetIndexToInsertInto(List<DequeStorable> cards, DequeStorable card) => 0;
	}
}
