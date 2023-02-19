using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(CouplingSocket), typeof(LerpTarget), typeof(DequeStorage))]
	public class DequeOwner : MonoBehaviour
	{
		[SerializeField]
		private DequeBox initialDeque;
		public Diajector diajector;
		
		public Transform tossTarget;
		[Tooltip("Height above the hand the deque should toss through, must be non-negative")]
		public float tossHeight;
		
		public AnimationCurve retrievalAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
		
		public CouplingSocket socket { get; private set; }
		public LerpTarget lerpTarget { get; private set; }
		public DequeStorage storage { get; private set; }
		
		public LerpTargetAnimator dequeAnimator { get; private set; }
		
		private bool isDequeDeployed => dequeBox && dequeBox.isDeployed;

		private DequeBox _dequeBox;
		public DequeBox dequeBox
		{
			get => _dequeBox;
			set
			{
				if (_dequeBox == value)
					return;
				
				if (_dequeBox)
					FinishUpOldDeque();
				
				if (value)
				{
					_dequeBox = value;
					SetupNewDeque();
				}
				else
				{
					UnsetDeque();
					_dequeBox = null;
				}
			}
		}
		
		private void FinishUpOldDeque()
		{
			dequeBox.owner = null;
			dequeBox.collisionTrigger.trigger.RemoveListener(StartDiajectorAssembly);
			dequeBox.grabbable.onUse.RemoveListener(RetrieveDeque);

			storage.definition = null;
			
			Destroy(dequeAnimator);
			
			dequeBox.state.SetBool(DequeBox.IsBound, false);
		}
		
		private void SetupNewDeque()
		{
			dequeBox.owner = this;
			dequeBox.collisionTrigger.trigger.AddListener(StartDiajectorAssembly);
			dequeBox.grabbable.onUse.AddListener(RetrieveDeque);

			storage.definition = dequeBox.definition;
			
			dequeAnimator = dequeBox.gameObject.AddComponent<LerpTargetAnimator>();
			dequeAnimator.curve = retrievalAnimationCurve;
			
			dequeBox.state.SetBool(DequeBox.IsBound, true);
			
			diajector.dequeBox = dequeBox;
		}
		
		private void UnsetDeque()
		{
			diajector.dequeBox = null;
			if (diajector.isOpen)
				diajector.ForceClose(dequeBox.lowerTarget);
		}
		
		private void Awake()
		{
			socket = GetComponent<CouplingSocket>();
			lerpTarget = GetComponent<LerpTarget>();
			storage = GetComponent<DequeStorage>();
		}
		
		private void Start()
		{
			dequeBox = initialDeque;
			RetrieveDeque();
		}
		
		public void OnToggleDeque(CallbackContext context)
		{
			if (!isActiveAndEnabled || !context.performed)
				return;
			if (!dequeBox)
				return;
			
			if (isDequeDeployed)
				RetrieveDeque();
			else
				TossDeque();
		}

		private void RetrieveDeque(Grabber grabber, Grabbable grabbable) => RetrieveDeque();
		private void RetrieveDeque()
		{
			dequeBox.state.SetTrigger(DequeBox.Retrieve);
		}

		private void TossDeque()
		{
			dequeBox.state.SetBool(DequeBox.IsCoupled, false);
			dequeBox.state.SetTrigger(DequeBox.Toss);
		}
		
		private void StartDiajectorAssembly()
		{
			dequeBox.state.SetTrigger(DequeBox.Collide);
		}
	}
}
