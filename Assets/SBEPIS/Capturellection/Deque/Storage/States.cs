using System.Collections.Generic;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public interface InventoryState
	{
		public CallbackList<Storable> Inventory { get; set; }
	}
	
	public interface DirectionState
	{
		public Vector3 Direction { get; set; }
	}
	
	public interface TimeState
	{
		public float Time { get; set; }
	}
	
	public interface FlippedState
	{
		public List<Storable> FlippedStorables { get; }
	}
	
	public interface TopState
	{
		public Storable TopStorable { get; set; }
	}
	
	public interface TreeDictionaryState
	{
		public TreeDictionary<string, Storable> Tree { get; }
	}
}
