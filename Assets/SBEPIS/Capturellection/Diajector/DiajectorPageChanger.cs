using KBCore.Refs;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	public class DiajectorPageChanger : ValidatedMonoBehaviour
	{
		[SerializeField, Parent(Flag.IncludeInactive)] private Diajector diajector;
		
		[FormerlySerializedAs("dequePage")]
		[SerializeField, Anywhere] private DiajectorPage diajectorPage;
		
		public void ChangePage()
		{
			diajector.ChangePage(diajectorPage);
		}
	}
}
