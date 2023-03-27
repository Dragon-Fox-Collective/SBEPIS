using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	public class DequeOwner : MonoBehaviour
	{
		[SerializeField]
		private Deque initialDeque;
		public Diajector diajector;
		
		[Tooltip("Purely organizational for the hierarchy")]
		public Transform cardParent;

		public SetDequeOwnerEvent onSetDequeBox = new();
		public UnsetDequeOwnerEvent onUnsetDequeBox = new();

		private Deque deque;
		public Deque Deque
		{
			get => deque;
			set
			{
				if (deque == value)
					return;

				if (deque)
				{
					Deque oldDeque = deque;
					deque.DequeOwner = null;
					deque = null;
					onUnsetDequeBox.Invoke(this, oldDeque, value);
					oldDeque.onUnsetDequeOwner.Invoke(this, oldDeque, value);
				}
				
				if (value)
				{
					deque = value;
					deque.DequeOwner = this;
					onSetDequeBox.Invoke(this);
					deque.onSetDequeOwner.Invoke(this);
				}
				else
				{
					if (diajector.IsOpen)
						diajector.ForceClose();
				}
			}
		}
		
		private void Awake()
		{
			Deque = initialDeque;
		}
		
		private void Start()
		{
			diajector.DequeOwner = this;
		}
	}
	
	[Serializable]
	public class SetDequeOwnerEvent : UnityEvent<DequeOwner> { }
	[Serializable]
	public class UnsetDequeOwnerEvent : UnityEvent<DequeOwner, Deque, Deque> { }
}
