using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
	public interface InventoryState
	{
		public List<Storable> Inventory { get; set; }
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
}
