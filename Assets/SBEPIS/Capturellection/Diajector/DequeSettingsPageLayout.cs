using SBEPIS.Capturellection.Storage;
using TMPro;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public abstract class DequeSettingsPageLayout : MonoBehaviour
	{
		public TMP_Text title;
	}
	
	public abstract class DequeSettingsPageLayout<T> : DequeSettingsPageLayout where T : DequeRuleset
	{
		public T Ruleset { get; set; }
	}
}
