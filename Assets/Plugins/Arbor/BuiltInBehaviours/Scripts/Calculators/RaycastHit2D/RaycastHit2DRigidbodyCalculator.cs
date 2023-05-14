//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ヒットしたCollider2DにアタッチされているのRigidbody2D。
	/// </summary>
	/// <remarks>
	/// Collider2DにRigidbody2Dがアタッチされていない場合はnullを返す。
	/// </remarks>
#else
	/// <summary>
	/// The Rigidbody2D of the Collider2D that was hit.
	/// </summary>
	/// <remarks>
	/// If the Collider2D is not attached to a Rigidbody2D then it is null.
	/// </remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RaycastHit2D/RaycastHit2D.Rigidbody")]
	[BehaviourTitle("RaycastHit2D.Rigidbody")]
	[BuiltInBehaviour]
	public sealed class RaycastHit2DRigidbodyCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit2D
		/// </summary>
		[SerializeField] private InputSlotRaycastHit2D _RaycastHit2D = new InputSlotRaycastHit2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// 当たったRigidbody2Dを出力
		/// </summary>
#else
		/// <summary>
		/// Output hit Rigidbody2D
		/// </summary>
#endif
		[SerializeField] private OutputSlotRigidbody2D _Rigidbody = new OutputSlotRigidbody2D();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RaycastHit2D raycastHit2D = new RaycastHit2D();
			if (_RaycastHit2D.GetValue(ref raycastHit2D))
			{
				_Rigidbody.SetValue(raycastHit2D.rigidbody);
			}
		}
	}
}
