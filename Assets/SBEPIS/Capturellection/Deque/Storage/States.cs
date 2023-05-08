using UnityEngine;

namespace SBEPIS.Capturellection.Storage
{
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
		public Storable FlippedStorable { get; set; }
	}
	
	public interface TopState
	{
		public Storable TopStorable { get; set; }
	}
}
