using UnityEngine;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Grabbable))]
	public class CouplingPlug : MonoBehaviour
	{
		public CoupleEvent onCouple = new();
		public CoupleEvent onDecouple = new();

		public bool isCoupled => coupledSocket;
		
		public CouplingSocket coupledSocket { get; private set; }
		
		public Grabbable grabbable { get; private set; }

		private void Awake()
		{
			grabbable = GetComponent<Grabbable>();
		}

		public void GetCoupled(CouplingSocket socket)
		{
			if (isCoupled)
			{
				Debug.LogError($"Tried to couple {this} to {socket} when socket already coupled to {coupledSocket}");
				return;
			}

			coupledSocket = socket;
			
			if (grabbable.isBeingHeld)
				grabbable.grabbingGrabber.Drop();
			grabbable.onGrab.AddListener(socket.Decouple);
			
			onCouple.Invoke(this, coupledSocket);
		}

		public void GetDecoupled()
		{
			if (!isCoupled)
			{
				Debug.LogError($"Tried to decouple plug {this} when already decoupled");
				return;
			}

			CouplingSocket socket = coupledSocket;
			coupledSocket = null;
			
			grabbable.onGrab.RemoveListener(socket.Decouple);
			
			onDecouple.Invoke(this, socket);
		}
	}
}
