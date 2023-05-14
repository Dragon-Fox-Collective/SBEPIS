//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 当たったCollider2D
	/// </summary>
#else
	/// <summary>
	/// The Collider2D that was hit.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RaycastHit2D/RaycastHit2D.Collider")]
	[BehaviourTitle("RaycastHit2D.Collider")]
	[BuiltInBehaviour]
	public sealed class RaycastHit2DColliderCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit
		/// </summary>
		[SerializeField] private InputSlotRaycastHit2D _RaycastHit2D = new InputSlotRaycastHit2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// 当たったColliderを出力
		/// </summary>
#else
		/// <summary>
		/// Output the hit Collider
		/// </summary>
#endif
		[SerializeField] private OutputSlotCollider2D _Collider = new OutputSlotCollider2D();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RaycastHit2D raycastHit2D = new RaycastHit2D();
			if (_RaycastHit2D.GetValue(ref raycastHit2D))
			{
				_Collider.SetValue(raycastHit2D.collider);
			}
		}
	}
}
