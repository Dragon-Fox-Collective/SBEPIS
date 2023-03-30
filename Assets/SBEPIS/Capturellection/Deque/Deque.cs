using UnityEngine;
using SBEPIS.Controller;
using SBEPIS.Utils;

namespace SBEPIS.Capturellection
{
	public class Deque : MonoBehaviour
	{
		public LerpTarget lowerTarget;
		public LerpTarget upperTarget;
		
		public StorableGroupDefinition definition;
		
		public EventPropertySlave<DequeOwner, Deque, SetDequeEvent, UnsetDequeEvent> dequeSlaveEvents = new();
		
		public void AdoptDeque(Grabber grabber, Grabbable grabbable)
		{
			Capturellector capturellector = grabber.GetComponent<Capturellector>();
			if (!capturellector)
				return;
			
			capturellector.dequeOwner.Deque = this;
		}
	}
}
