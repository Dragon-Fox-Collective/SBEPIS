//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 衝突したオブジェクトのワールド座標
	/// </summary>
#else
	/// <summary>
	/// The impact point in world space where the ray hit the collider.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RaycastHit/RaycastHit.Point")]
	[BehaviourTitle("RaycastHit.Point")]
	[BuiltInBehaviour]
	public sealed class RaycastHitPointCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit
		/// </summary>
		[SerializeField] private InputSlotRaycastHit _RaycastHit = new InputSlotRaycastHit();

#if ARBOR_DOC_JA
		/// <summary>
		/// 衝突点を出力
		/// </summary>
#else
		/// <summary>
		/// Output the impact point
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Point = new OutputSlotVector3();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RaycastHit raycastHit = new RaycastHit();
			if (_RaycastHit.GetValue(ref raycastHit))
			{
				_Point.SetValue(raycastHit.point);
			}
		}
	}
}
