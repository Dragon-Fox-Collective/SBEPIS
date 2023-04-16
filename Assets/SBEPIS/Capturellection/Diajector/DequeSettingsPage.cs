using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class DequeSettingsPage : MonoBehaviour
	{
		[SerializeField, Self]
		private DiajectorPage page;
		public DiajectorPage Page => page;
		
		private void OnValidate() => this.ValidateRefs();
		
		public RectTransform settingsParent;
		public CardTarget backButton;
		public CardTarget prevButton;
		public CardTarget nextButton;
	}
}
