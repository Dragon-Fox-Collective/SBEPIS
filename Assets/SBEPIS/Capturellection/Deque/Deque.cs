using SBEPIS.Capturellection.Storage;
using UnityEngine;
using SBEPIS.Utils;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(Inventory))]
	public class Deque : MonoBehaviour
	{
		public LerpTarget lowerTarget;
		public LerpTarget upperTarget;
		
		public StorableGroupDefinition definition;
		
		public Diajector diajector;
		
		public Inventory Inventory { get; private set; }
		
		private void Awake()
		{
			Inventory = GetComponent<Inventory>();
		}
	}
}
