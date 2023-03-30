using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class DequeSettingsPage : MonoBehaviour
	{
		public RectTransform settingsParent;
		public CardTarget backButton;
		public CardTarget prevButton;
		public CardTarget nextButton;
		
		public DiajectorPage page { get; private set; }
		
		public void ManualAwake()
		{
			page = GetComponent<DiajectorPage>(); // awake doesnt get called before opening page :(
		}
	}
}
