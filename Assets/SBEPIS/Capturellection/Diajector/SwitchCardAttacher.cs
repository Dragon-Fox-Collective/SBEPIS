using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(CardTarget))]
	public class SwitchCardAttacher : MonoBehaviour
	{
		[SerializeField, Self]
		private CardTarget cardTarget;
		
		private void OnValidate() => this.ValidateRefs();
		
		[FormerlySerializedAs("offPoint")]
		public Transform falsePoint;
		[FormerlySerializedAs("onPoint")]
		public Transform truePoint;
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
			switchCard = card.gameObject.AddComponent<SwitchCard>();
			switchCard.offPoint = falsePoint;
			switchCard.onPoint = truePoint;
			switchCard.target = cardTarget;
			switchCard.SwitchValue = switchValue;
			switchCard.onSwitchValueChanged = onSwitchValueChanged;
			card.Grabbable.onDrop.AddListener((_, _) => switchCard.ClampNewPosition());
		}
	}
}