using UnityEngine;

namespace SBEPIS.Capturellection.UI
{
	public class ChoiceCardSlot : MonoBehaviour
	{
		private ChoiceCard chosenCard;
		public ChoiceCard ChosenCard
		{
			get => chosenCard;
			set
			{
				if (chosenCard) chosenCard.IsChosen = false;
				chosenCard = value;
				chosenCard.IsChosen = true;
			}
		}
	}
}