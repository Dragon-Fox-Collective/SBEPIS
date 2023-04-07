using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class DequeSettingsPageChanger : MonoBehaviour
	{
		public DequeSettingsPageCreator pageCreator;
		
		private Diajector diajector;
		
		private void Start()
		{
			diajector = GetComponentInParent<Diajector>();
		}
		
		public void ChangePage()
		{
			if (pageCreator.FirstSettingsPage)
				diajector.ChangePage(pageCreator.FirstSettingsPage.page);
		}
	}
}
