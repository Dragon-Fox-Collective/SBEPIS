using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class CapturellectorInventorySetter : MonoBehaviour
	{
		[SerializeField] private List<Capturellector> capturellectors;
		
		public void SetNewInventory(Inventory newInventory)
		{
			capturellectors.ForEach(capturellector => capturellector.Inventory = newInventory);
		}
	}
}