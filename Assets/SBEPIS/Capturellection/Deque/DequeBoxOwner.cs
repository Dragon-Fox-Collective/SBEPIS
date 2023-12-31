using KBCore.Refs;
using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(PlayerReference))]
	public class DequeBoxOwner : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private LerpTarget lerpTarget;
		public LerpTarget LerpTarget => lerpTarget;
		
		[SerializeField, Self] private CouplingSocket socket;
		public CouplingSocket Socket => socket;

		[SerializeField, Self] private PlayerReference playerReference;
		
		[SerializeField, Anywhere] private Transform tossTarget;
		public Transform TossTarget => tossTarget;
		
		[Tooltip("Height above the hand the deque should toss through")]
		[SerializeField] private float tossHeight;
		public float TossHeight => tossHeight;
		
		[SerializeField, Anywhere(Flag.Optional)] private DequeBox dequeBox;
		public DequeBox DequeBox
		{
			get => dequeBox;
			set
			{
				if (dequeBox)
					dequeBox.Detatch();
				
				dequeBox = value;
			}
		}
		
		private bool IsDequeBoxDeployed => dequeBox && dequeBox.IsDeployed;
		
		protected override void OnValidate()
		{
			base.OnValidate();
			tossHeight = Mathf.Max(tossHeight, 0);
		}
		
		private void Start()
		{
			if (dequeBox)
				dequeBox.StartBindToPlayer(playerReference);
		}
		
		public void ToggleDeque()
		{
			if (!isActiveAndEnabled)
				return;
			if (!dequeBox)
				return;
			if (dequeBox.TryGetComponent(out Grabbable grabbable) && grabbable.IsBeingHeld)
				return;
			
			if (IsDequeBoxDeployed)
				dequeBox.Retrieve(this);
			else
				dequeBox.Toss(this);
		}
	}
}
