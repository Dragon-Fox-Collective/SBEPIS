using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class CapturellectorInventorySetter : MonoBehaviour
	{
		[SerializeField, Anywhere] private List<Capturellector> capturellectors;
		
		public void SetNewInventory(Inventory newInventory)
		{
			capturellectors.ForEach(capturellector => capturellector.Inventory = newInventory);
		}
	}
}