using System.Linq;
using KBCore.Refs;
using SBEPIS.Capturellection.UI;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class HashmapSettingsPageModule : DequeSettingsPageModule<HashFunctionSettings>
	{
		[SerializeField] private ChoiceCardSlot hashFunctionChoiceSlot;
		[SerializeField, Child] private HashFunctionChoice[] hashFunctionChoices;
		
		private void OnValidate() => this.ValidateRefs();
		
		public void ResetHashFunctionChoice(HashFunctionChoice boundChoice)
		{
			if (boundChoice == hashFunctionChoices[^1] && hashFunctionChoices.All(choice => choice.Attacher.ChoiceCard))
				hashFunctionChoiceSlot.ChosenCard = hashFunctionChoices.First(choice => choice.HashFunction == Settings.HashFunction).Attacher.ChoiceCard;
		}
		public void ChangeHashFunction(HashFunction hashFunction) => Settings.HashFunction = hashFunction;
	}
}