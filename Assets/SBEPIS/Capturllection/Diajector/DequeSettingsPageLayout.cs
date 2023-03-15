using TMPro;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class DequeSettingsPageLayout : MonoBehaviour
	{
		public TMP_Text title;
		
		public DequeRuleset ruleset { get; set; }
	}

	public abstract class DequeSettingsPageLayout<T> : DequeSettingsPageLayout where T : DequeRuleset
	{
		public new T ruleset { get => base.ruleset as T; set => base.ruleset = value; }
	}
}
