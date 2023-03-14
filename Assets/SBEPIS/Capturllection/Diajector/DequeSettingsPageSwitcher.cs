using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class DequeSettingsPageSwitcher : MonoBehaviour
	{
		private Diajector diajector;
		
		private void Start()
		{
			diajector = GetComponentInParent<Diajector>();
		}
		
		public void SwitchPage()
		{
			if (diajector.owner.firstDequeSettingsPage)
				diajector.ChangePage(diajector.owner.firstDequeSettingsPage.page);
		}
	}
}
