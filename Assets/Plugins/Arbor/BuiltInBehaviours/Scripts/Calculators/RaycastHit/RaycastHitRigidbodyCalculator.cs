//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ヒットしたColliderにアタッチされているのRigidbody。
	/// </summary>
	/// <remarks>
	/// ColliderにRigidbodyがアタッチされていない場合はnullを返す。
	/// </remarks>
#else
	/// <summary>
	/// The Rigidbody of the collider that was hit.
	/// </summary>
	/// <remarks>
	/// If the collider is not attached to a rigidbody then it is null.
	/// </remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RaycastHit/RaycastHit.Rigidbody")]
	[BehaviourTitle("RaycastHit.Rigidbody")]
	[BuiltInBehaviour]
	public sealed class RaycastHitRigidbodyCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit
		/// </summary>
		[SerializeField] private InputSlotRaycastHit _RaycastHit = new InputSlotRaycastHit();

#if ARBOR_DOC_JA
		/// <summary>
		/// 当たったRigidbodyを出力
		/// </summary>
#else
		/// <summary>
		/// Output hit Rigidbody
		/// </summary>
#endif
		[SerializeField] private OutputSlotRigidbody _Rigidbody = new OutputSlotRigidbody();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RaycastHit raycastHit = new RaycastHit();
			if (_RaycastHit.GetValue(ref raycastHit))
			{
				_Rigidbody.SetValue(raycastHit.rigidbody);
			}
		}
	}
}
