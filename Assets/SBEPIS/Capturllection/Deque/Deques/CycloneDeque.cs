using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	[CreateAssetMenu(menuName="Deque/"+nameof(CycloneDeque))]
	public class CycloneDeque : DequeType
	{
		public float radius = 0.1f;
		public Quaternion cardRotation = Quaternion.identity;
		public float speed = 20;

		private float time;
		private DequeStorable topCard;

		public override void Tick(List<DequeStorable> cards, float delta)
		{
			time += delta;
			UpdateTopCard(cards);
		}

		private void UpdateTopCard(List<DequeStorable> cards)
		{
			float cardAngle = time * speed;
			float deltaAngle = 360f / cards.Count;
			foreach (DequeStorable card in cards)
			{
				float modAngle = cardAngle.ModAround(360);
				if (Mathf.Abs(modAngle) < deltaAngle / 2)
					topCard = card;
				
				cardAngle += deltaAngle;
			}
		}

		public override void LayoutTargets(Dictionary<DequeStorable, CardTarget> targets)
		{
			float cardAngle = time * speed;
			float deltaAngle = 360f / targets.Count;
			foreach ((DequeStorable card, CardTarget target) in targets)
			{
				target.transform.localPosition = Quaternion.Euler(0, 0, cardAngle) * Vector3.up * radius;
				target.transform.localRotation = Quaternion.Euler(0, 0, cardAngle) * cardRotation;

				cardAngle += deltaAngle;
			}
		}

		public override bool CanFetch(List<DequeStorable> cards, DequeStorable card) => card == topCard;
		
		public override int GetIndexToInsertInto(List<DequeStorable> cards, DequeStorable card) => cards.Count;
	}
}
