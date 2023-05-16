using System;
using KBCore.Refs;
using SBEPIS.Controller;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection.UI
{
	[RequireComponent(typeof(CardTarget))]
	public class SwitchCardAttacher : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private CardTarget cardTarget;
		
		[FormerlySerializedAs("offPoint")]
		[SerializeField, Anywhere] private Transform falsePoint;
		[FormerlySerializedAs("onPoint")]
		[SerializeField, Anywhere] private Transform truePoint;
		public UnityEvent<bool> onSwitchValueChanged = new();
		
		private SwitchCard switchCard;
		
		private bool switchValue;
		public bool SwitchValue
		{
			get
			{
				if (switchCard)
					return switchCard.SwitchValue;
				else
					return switchValue;
			}
			set
			{
				if (switchCard)
					switchCard.SwitchValue = value;
				else
					switchValue = value;
			}
		}
		
		public void Attach(DequeElement card)
		{
			if (!card.TryGetComponent(out Grabbable grabbable))
				throw new NullReferenceException($"Card {card} has no grabbable");
			
			switchCard = card.gameObject.AddComponent<SwitchCard>();
			switchCard.offPoint = falsePoint;
			switchCard.onPoint = truePoint;
			switchCard.target = cardTarget;
			switchCard.SwitchValue = switchValue;
			switchCard.onSwitchValueChanged = onSwitchValueChanged;
			grabbable.onDrop.AddListener((_, _) => switchCard.ClampNewPosition());
		}
	}
}