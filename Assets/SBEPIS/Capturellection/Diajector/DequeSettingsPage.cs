using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class DequeSettingsPage : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private DiajectorPage page;
		public DiajectorPage Page => page;
		
		public RectTransform settingsParent;
		public CardTarget backButton;
		public CardTarget prevButton;
		public CardTarget nextButton;
	}
}
