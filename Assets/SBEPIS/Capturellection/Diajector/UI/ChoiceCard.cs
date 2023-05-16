using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection.UI
{
	public class ChoiceCard : MonoBehaviour
	{
		public Transform point;
		public ChoiceCardSlot slot;
		public CardTarget target;
		
		public UnityEvent onChosen = new();
		
		private bool isChosen;
		public bool IsChosen
		{
			get => isChosen;
			set
			{
				isChosen = value;
				
				target.transform.SetPositionAndRotation(
					(isChosen ? slot.transform : point).position,
					(isChosen ? slot.transform : point).rotation);
				
				if (isChosen)
					onChosen.Invoke();
			}
		}
		
		public void ClampNewPosition()
		{
			if (!isChosen && (transform.position - slot.transform.position).sqrMagnitude < (transform.position - point.position).sqrMagnitude)
				slot.ChosenCard = this;
		}
	}
}
