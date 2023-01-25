using SBEPIS.Physics;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Capturllection
{
	public class DequeOwner : MonoBehaviour
	{
		[SerializeField]
		private CaptureDeque initialDeque;
		public Diajector diajector;

		public Transform attachmentTarget;
		public StrengthSettings attachmentStrength;

		private JointTargetter joint;

		private CaptureDeque _deque;
		public CaptureDeque deque
		{
			get => _deque;
			set
			{
				if (_deque == value)
					return;
				
				_deque = value;
				diajector.deque = _deque;
				
				if (joint)
					Destroy(joint);
				if (_deque)
				{
					joint = gameObject.AddComponent<JointTargetter>();
					joint.connectedBody = _deque.grabbable.rigidbody;
					joint.target = attachmentTarget;
					joint.strength = attachmentStrength;
				}
			}
		}

		private void Start()
		{
			deque = initialDeque;
		}

		public void OnToggleDeque(CallbackContext context)
		{
			if (!isActiveAndEnabled || !context.performed)
				return;
			if (!diajector.deque)
				return;

			ToggleDiajector();
		}
		
		public void ToggleDiajector()
		{
			if (diajector.gameObject.activeSelf)
				DesummonDiajector();
			else
				SummonDiajector();
		}

		public void SummonDiajector()
		{
			if (diajector.gameObject.activeSelf)
				return;

			diajector.StartAssembly();
		}

		public void DesummonDiajector()
		{
			if (!diajector.gameObject.activeSelf)
				return;

			diajector.StartDisassembly();
		}
	}
}
