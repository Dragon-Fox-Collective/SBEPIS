using KBCore.Refs;
using SBEPIS.Capturellection.UI;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class HashFunctionChoice : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private ChoiceCardAttacher attacher;
		public ChoiceCardAttacher Attacher => attacher;
		[SerializeField, Parent] private HashmapSettingsPageModule settingsPageModule;
		[SerializeField, Anywhere] private HashFunction hashFunction;
		public HashFunction HashFunction => hashFunction;
		
		public void Choose() => settingsPageModule.ChangeHashFunction(hashFunction);
	}
}