using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class EightBall : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private InventoryStorable card;
		public InventoryStorable Card => card;
		[SerializeField, Anywhere] private Transform root;
		public Transform Root => root;
		
		public InventoryStorable OriginalCard { get; set; }
	}
}
