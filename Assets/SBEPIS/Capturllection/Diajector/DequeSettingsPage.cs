using UnityEngine;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(DiajectorPage))]
	public class DequeSettingsPage : MonoBehaviour
	{
		public RectTransform settingsParent;
		public CardTarget backButton;
		public CardTarget prevButton;
		public CardTarget nextButton;
		
		public DiajectorPage page { get; private set; }
		
		private void Awake()
		{
			page = GetComponent<DiajectorPage>();
		}
	}
}
