using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(CardTarget))]
	public class SwitchCardAttacher : MonoBehaviour
	{
		[FormerlySerializedAs("offPoint")]
		public Transform falsePoint;
		[FormerlySerializedAs("onPoint")]
		public Transform truePoint;
		public UnityEvent<bool> onSwitchValueChanged = new();

		private SwitchCard switchCard;
		private CardTarget cardTarget;

		private bool _switchValue;
		public bool SwitchValue
		{
			get
			{
				if (switchCard)
					return switchCard.SwitchValue;
				else
					return _switchValue;
			}
			set
			{
				if (switchCard)
					switchCard.SwitchValue = value;
				else
					_switchValue = value;
			}
		}

		public void Attach(DequeStorable card)
		{
			cardTarget = GetComponent<CardTarget>(); // lol. lmao. awake isn't called before this fires
			switchCard = card.gameObject.AddComponent<SwitchCard>();
			switchCard.offPoint = falsePoint;
			switchCard.onPoint = truePoint;
			switchCard.target = cardTarget;
			switchCard.SwitchValue = _switchValue;
			switchCard.onSwitchValueChanged = onSwitchValueChanged;
			card.Grabbable.onDrop.AddListener((_, _) => switchCard.ClampNewPosition());
		}
	}
}