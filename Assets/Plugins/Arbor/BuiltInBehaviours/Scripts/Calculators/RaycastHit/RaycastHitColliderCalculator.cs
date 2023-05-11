//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 当たったCollider
	/// </summary>
#else
	/// <summary>
	/// The Collider that was hit.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RaycastHit/RaycastHit.Collider")]
	[BehaviourTitle("RaycastHit.Collider")]
	[BuiltInBehaviour]
	public sealed class RaycastHitColliderCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit
		/// </summary>
		[SerializeField] private InputSlotRaycastHit _RaycastHit = new InputSlotRaycastHit();

#if ARBOR_DOC_JA
		/// <summary>
		/// 当たったColliderを出力
		/// </summary>
#else
		/// <summary>
		/// Output the hit Collider
		/// </summary>
#endif
		[SerializeField] private OutputSlotCollider _Collider = new OutputSlotCollider();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RaycastHit raycastHit = new RaycastHit();
			if (_RaycastHit.GetValue(ref raycastHit))
			{
				_Collider.SetValue(raycastHit.collider);
			}
		}
	}
}
