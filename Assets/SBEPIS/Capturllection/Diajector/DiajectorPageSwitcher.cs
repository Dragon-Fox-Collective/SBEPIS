using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturllection
{
	public class DiajectorPageSwitcher : MonoBehaviour
	{
		[FormerlySerializedAs("dequePage")]
		public DiajectorPage diajectorPage;
		
		private Diajector diajector;
		
		private void Start()
		{
			diajector = GetComponentInParent<Diajector>();
		}
		
		public void SwitchPage()
		{
			diajector.ChangePage(diajectorPage);
		}
	}
}
