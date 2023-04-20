using SBEPIS.Capturellection.Storage;
using TMPro;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public abstract class DequeSettingsPageLayout : MonoBehaviour
	{
		public TMP_Text title;
		
		public DequeRuleset Ruleset { get; set; }
	}

	public abstract class DequeSettingsPageLayout<T> : DequeSettingsPageLayout where T : DequeRuleset
	{
		public new T Ruleset { get => base.Ruleset as T; set => base.Ruleset = value; }
	}
}
