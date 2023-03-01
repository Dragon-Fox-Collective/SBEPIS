using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class DequePageSwitcher : MonoBehaviour
	{
		public DequePage dequePage;
		
		private Diajector diajector;
		
		private void Start()
		{
			diajector = GetComponentInParent<Diajector>();
		}
		
		public void SwitchPage()
		{
			diajector.ChangePage(dequePage);
		}
	}
}
