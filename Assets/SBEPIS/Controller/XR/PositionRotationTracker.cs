using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Controller
{
	public class PositionRotationTracker : MonoBehaviour
	{
		[SerializeField]
		private InputAction positionInput;
		[SerializeField]
		private UnityEvent<Vector3> onPosition;
		
		[SerializeField]
		private InputAction rotationInput;
		[SerializeField]
		private UnityEvent<Quaternion> onRotation;
		
		private void Awake()
		{
			positionInput.performed += OnPosition;
			positionInput.canceled += OnPosition;
			rotationInput.performed += OnRotation;
			rotationInput.canceled += OnRotation;
		}
		
		private void OnPosition(CallbackContext context) => onPosition.Invoke(context.ReadValue<Vector3>());
		private void OnRotation(CallbackContext context) => onRotation.Invoke(context.ReadValue<Quaternion>());

		public void OnEnable()
		{
			positionInput.Enable();
			rotationInput.Enable();
		}

		public void OnDisable()
		{
			positionInput.Disable();
			rotationInput.Disable();
		}
	}
}
