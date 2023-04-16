using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class DequeSettingsPageChanger : MonoBehaviour
	{
		[SerializeField, Parent(Flag.IncludeInactive)]
		private Diajector diajector;
		
		private void OnValidate() => this.ValidateRefs();
		
		public DequeSettingsPageCreator pageCreator;
		
		public void ChangePage()
		{
			if (pageCreator.FirstSettingsPage)
				diajector.ChangePage(pageCreator.FirstSettingsPage.Page);
		}
	}
}
