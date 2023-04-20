using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class DequeSettingsPageChanger : ValidatedMonoBehaviour
	{
		[SerializeField, Parent(Flag.IncludeInactive)]
		private Diajector diajector;
		
		public DequeSettingsPageCreator pageCreator;
		
		public void ChangePage()
		{
			if (pageCreator.FirstSettingsPage)
				diajector.ChangePage(pageCreator.FirstSettingsPage.Page);
		}
	}
}
