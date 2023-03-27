using UnityEngine;
using SBEPIS.Controller;
using SBEPIS.Utils;

namespace SBEPIS.Capturllection
{
	public class Deque : MonoBehaviour
	{
		public LerpTarget lowerTarget;
		public LerpTarget upperTarget;
		
		public StorableGroupDefinition definition;
		
		public SetDequeOwnerEvent onSetDequeOwner = new();
		public UnsetDequeOwnerEvent onUnsetDequeOwner = new();
		
		public DequeOwner DequeOwner { get; set; }
		
		public void AdoptDeque(Grabber grabber, Grabbable grabbable)
		{
			Capturellector capturellector = grabber.GetComponent<Capturellector>();
			if (!capturellector)
				return;
			
			capturellector.dequeOwner.Deque = this;
		}
	}
}
