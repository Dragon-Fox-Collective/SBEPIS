using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class DiajectorCloser : MonoBehaviour
	{
		private Diajector diajector;
		
		public void CloseOldDiajector(Diajector newDiajector)
		{
			if (newDiajector == diajector)
				return;
			
			if (diajector && diajector.IsOpen)
				diajector.StartDisassembly();
			
			diajector = newDiajector;
		}
	}
}
