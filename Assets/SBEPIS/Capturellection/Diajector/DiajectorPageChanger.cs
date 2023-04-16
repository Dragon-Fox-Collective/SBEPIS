using KBCore.Refs;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	public class DiajectorPageChanger : MonoBehaviour
	{
		[SerializeField, Parent(Flag.IncludeInactive)] private Diajector diajector;
		
		[FormerlySerializedAs("dequePage")]
		[SerializeField, Anywhere] private DiajectorPage diajectorPage;
		
		private void OnValidate() => this.ValidateRefs();
		
		public void ChangePage()
		{
			diajector.ChangePage(diajectorPage);
		}
	}
}
