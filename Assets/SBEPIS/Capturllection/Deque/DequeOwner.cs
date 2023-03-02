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
		
		public Transform cardParent;
		
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
			dequeBox.grabbable.onUse.RemoveListener(CloseDiajector);
			
			Destroy(dequeAnimator);
			
			dequeBox.state.isBound = false;
			dequeBox.state.isDiajectorOpen = false;
			dequeBox.state.isDeployed = false;
		}
		
		private void SetupNewDeque()
		{
			dequeBox.owner = this;
			dequeBox.collisionTrigger.trigger.AddListener(StartDiajectorAssembly);
			dequeBox.grabbable.onUse.AddListener(CloseDiajector);
			
			storage.SyncDeque(dequeBox);
			
			dequeAnimator = dequeBox.gameObject.AddComponent<LerpTargetAnimator>();
			dequeAnimator.curve = retrievalAnimationCurve;
			
			dequeBox.state.isBound = true;
			dequeBox.state.isDiajectorOpen = diajector.isOpen;
			dequeBox.state.isDeployed = diajector.isOpen;
			
			diajector.UpdateCardTexture();
		}
		
		private void UnsetDeque()
		{
			if (diajector.isOpen)
				diajector.ForceClose();
		}
		
		private void Awake()
		{
			socket = GetComponent<CouplingSocket>();
			lerpTarget = GetComponent<LerpTarget>();
			storage = GetComponent<DequeStorage>();
			
			socket.onDecouple.AddListener(DecoupleDeque);
		}
		
		private void Start()
		{
			diajector.owner = this;
			
			dequeBox = initialDeque;
			if (dequeBox)
				RetrieveDeque();
			
			storage.CreateInitialCards(this);
		}
		
		public void OnToggleDeque(CallbackContext context)
		{
			if (!isActiveAndEnabled || !context.performed)
				return;
			if (!dequeBox)
				return;
			
			if (dequeBox.grabbable.isBeingHeld)
				CloseDiajector();
			else if (isDequeDeployed)
				RetrieveDeque();
			else
				TossDeque();
		}

		private void CloseDiajector(Grabber grabber, Grabbable grabbable) => CloseDiajector();
		private void CloseDiajector()
		{
			dequeBox.state.isDiajectorOpen = false;
		}
		
		private void RetrieveDeque()
		{
			CloseDiajector();
			dequeBox.state.isDeployed = false;
		}

		private void TossDeque()
		{
			DecoupleDeque();
			dequeBox.state.Toss();
		}
		
		private void StartDiajectorAssembly()
		{
			dequeBox.state.isDiajectorOpen = true;
		}

		private void DecoupleDeque(CouplingPlug plug, CouplingSocket socket) => DecoupleDeque();
		private void DecoupleDeque()
		{
			dequeBox.state.isDeployed = true;
			dequeBox.state.isCoupled = false;
		}
	}
}
