using SBEPIS.Alchemy;
using SBEPIS.Bits;
using SBEPIS.Captchalogue;
using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace SBEPIS.Interaction
{
	public class SylladexOwner : MonoBehaviour
	{
		[SerializeField]
		private Sylladex sylladex;
		[SerializeField]
		private Transform sylladexParent;
		[SerializeField]
		private Flatscreen.ItemHolderFlatscreen itemHolder;
		[SerializeField]
		private PlayerModeSwapper modeSwapper;
		[SerializeField]
		private InputActionReference[] viewingSylladexEnabledActions;
		[SerializeField]
		private InputActionReference[] viewingSylladexDisabledActions;
		[SerializeField]
		private InputActionReference[] usingSylladexEnabledActions;
		[SerializeField]
		private InputActionReference[] usingSylladexDisabledActions;

		private bool _isViewing;
		private bool IsViewing
		{
			get => _isViewing;
			set
			{
				_isViewing = value;
				if (_isViewing)
				{
					foreach (InputActionReference action in viewingSylladexEnabledActions)
						action.action.Enable();
					foreach (InputActionReference action in viewingSylladexDisabledActions)
						action.action.Disable();
					sylladexParentRotTarget = Quaternion.identity;
					sylladexParentScaleTarget = Vector3.one;
				}
				else
				{
					foreach (InputActionReference action in viewingSylladexEnabledActions)
						action.action.Disable();
					foreach (InputActionReference action in viewingSylladexDisabledActions)
						action.action.Enable();
					sylladexParentRotTarget = Quaternion.Euler(0, -90, 0);
					sylladexParentScaleTarget = Vector3.zero;
				}
			}
		}
		private bool _canCaptchalogue;
		private bool CanCaptchalogue
		{
			get => _canCaptchalogue;
			set
			{
				_canCaptchalogue = value;
				if (_canCaptchalogue)
				{
					foreach (InputActionReference action in usingSylladexEnabledActions)
						action.action.Enable();
					foreach (InputActionReference action in usingSylladexDisabledActions)
						action.action.Disable();
					sylladexPosTarget = new Vector3(0, -0.25f, 1);
					sylladexRotTarget = Quaternion.Euler(-75, 180, 0);
				}
				else
				{
					foreach (InputActionReference action in usingSylladexEnabledActions)
						action.action.Disable();
					foreach (InputActionReference action in usingSylladexDisabledActions)
						action.action.Enable();
					sylladexPosTarget = new Vector3(0, 0, 1);
					sylladexRotTarget = Quaternion.Euler(0, 180, 0);
				}
			}
		}
		private Vector3 sylladexPosTarget;
		private Vector3 sylladexPosTargetVel;
		private Quaternion sylladexRotTarget;
		private Quaternion sylladexRotDeriv;
		private Quaternion sylladexParentRotTarget;
		private Quaternion sylladexParentRotDeriv;
		private Vector3 sylladexParentScaleTarget;
		private Vector3 sylladexParentScaleVel;

		private void Start()
		{
			ResetSylladexControls();
		}

		private void Update()
		{
			sylladex.transform.localPosition = Vector3.SmoothDamp(sylladex.transform.localPosition, sylladexPosTarget, ref sylladexPosTargetVel, 0.1f);
			sylladex.transform.localRotation = QuaternionUtil.SmoothDamp(sylladex.transform.localRotation, sylladexRotTarget, ref sylladexRotDeriv, 0.1f);
			sylladexParent.localRotation = QuaternionUtil.SmoothDamp(sylladexParent.localRotation, sylladexParentRotTarget, ref sylladexParentRotDeriv, 0.2f);
			sylladexParent.localScale = Vector3.SmoothDamp(sylladexParent.localScale, sylladexParentScaleTarget, ref sylladexParentScaleVel, 0.1f);
		}

		public void OnSetPlayerMode(PlayerMode mode)
		{
			if (mode == PlayerMode.Gameplay)
				ResetSylladexControls();
		}

		private void ResetSylladexControls()
		{
			IsViewing = false;
			CanCaptchalogue = false;
		}

		public void OnToggleViewingSylladex(CallbackContext context)
		{
			if (!context.performed || sylladex.isWorking)
				return;

			IsViewing = !IsViewing;
		}

		public void OnHoldViewingSylladex(CallbackContext context)
		{
			IsViewing = context.performed && !sylladex.isWorking;
		}

		public void OnCaptchalogueMode(CallbackContext context)
		{
			CanCaptchalogue = context.performed;
		}

		public void OnCaptchalogue(CallbackContext context)
		{
			if (!context.performed)
				return;

			sylladex.StartCaptchaloguing();
		}

		public void CastCaptchalogue()
		{
			Item hitItem;
			if (itemHolder.Cast(out RaycastHit captchaHit) && (hitItem = captchaHit.rigidbody.GetComponent<Item>()))
				sylladex.Captchalogue(hitItem);
		}

		public void OnFetch(CallbackContext context)
		{
			if (!context.performed)
				return;

			sylladex.StartFetching();
		}

		public void Fetch()
		{
			Item fetchingItem = sylladex.Fetch();
			if (fetchingItem)
			{
				fetchingItem.gameObject.SetActive(true);
				fetchingItem.transform.localPosition = Vector3.up;
				fetchingItem.transform.rotation = Quaternion.identity;
				fetchingItem.transform.SetParent(null);
			}
		}

		public void OnCaptchalogueUse(CallbackContext context)
		{
			if (!context.performed || sylladex.isWorking)
				return;

			if (itemHolder.Cast(out RaycastHit captchaHit))
			{
				CaptchalogueCard hitCard = captchaHit.rigidbody.GetComponent<CaptchalogueCard>();
				if (hitCard && hitCard.punchedBits == BitSet.Nothing)
				{
					sylladex.InsertCard(hitCard);
					return;
				}

				Cartridge hitCart = captchaHit.rigidbody.GetComponent<Cartridge>();
				if (hitCart)
				{
					sylladex.StartSyncingCartridge(hitCart);
					return;
				}
			}
		}

		public void OnCaptchaloguePrint(CallbackContext context)
		{
			if (!context.performed || sylladex.isWorking)
				return;

			sylladex.RetrieveCard();
		}

		public void OnFlipSylladex(CallbackContext context)
		{
			if (!context.performed || !IsViewing || CanCaptchalogue)
				return;

			if (sylladexRotTarget == Quaternion.identity)
				sylladexRotTarget = Quaternion.Euler(0, 180, 0);
			else
				sylladexRotTarget = Quaternion.identity;
		}

		public void OnToggleSylladexPanel(CallbackContext context)
		{
			if (!context.performed)
				return;

			sylladex.StartTogglingPanel();
		}
	}
}