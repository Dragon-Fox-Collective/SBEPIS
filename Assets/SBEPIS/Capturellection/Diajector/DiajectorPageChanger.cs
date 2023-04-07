using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	public class DiajectorPageChanger : MonoBehaviour
	{
		[FormerlySerializedAs("dequePage")]
		public DiajectorPage diajectorPage;
		
		private Diajector diajector;
		
		private void Start()
		{
			diajector = GetComponentInParent<Diajector>();
		}
		
		public void ChangePage()
		{
			diajector.ChangePage(diajectorPage);
		}
	}
}
