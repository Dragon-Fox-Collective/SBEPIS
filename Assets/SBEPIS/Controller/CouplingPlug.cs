using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Grabbable))]
	public class CouplingPlug : MonoBehaviour
	{
		[SerializeField, Self]
		private Grabbable grabbable;
		public Grabbable Grabbable => grabbable;
		
		private void OnValidate() => this.ValidateRefs();
		
		public CoupleEvent onCouple = new();
		public CoupleEvent onDecouple = new();
		
		public bool IsCoupled => CoupledSocket;
		
		public CouplingSocket CoupledSocket { get; private set; }
		
		public void GetCoupled(CouplingSocket socket)
		{
			if (IsCoupled)
			{
				Debug.LogError($"Tried to couple {this} to {socket} when socket already coupled to {CoupledSocket}");
				return;
			}
			
			CoupledSocket = socket;
			
			Grabbable.Drop();
			Grabbable.onGrab.AddListener(socket.Decouple);
			
			onCouple.Invoke(this, CoupledSocket);
		}
		
		public void GetDecoupled()
		{
			if (!IsCoupled)
			{
				Debug.LogError($"Tried to decouple plug {this} when already decoupled");
				return;
			}
			
			CouplingSocket socket = CoupledSocket;
			CoupledSocket = null;
			
			Grabbable.onGrab.RemoveListener(socket.Decouple);
			
			onDecouple.Invoke(this, socket);
		}
	}
}
