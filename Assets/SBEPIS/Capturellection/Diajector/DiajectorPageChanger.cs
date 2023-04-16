using KBCore.Refs;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	public class DiajectorPageChanger : MonoBehaviour
	{
		[FormerlySerializedAs("dequePage")]
		public DiajectorPage diajectorPage;
		
		[SerializeField, Parent(Flag.IncludeInactive)]
		private Diajector diajector;
		
		private void OnValidate() => this.ValidateRefs();
		
		public void ChangePage()
		{
			diajector.ChangePage(diajectorPage);
		}
	}
}
