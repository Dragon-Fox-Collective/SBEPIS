using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class DequeSettingsPageSwitcher : MonoBehaviour
	{
		public DequeSettingsPageCreator pageCreator;
		
		private Diajector diajector;
		
		private void Start()
		{
			diajector = GetComponentInParent<Diajector>();
		}
		
		public void SwitchPage()
		{
			if (pageCreator.FirstSettingsPage)
				diajector.ChangePage(pageCreator.FirstSettingsPage.page);
		}
	}
}
