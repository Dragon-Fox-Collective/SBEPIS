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
	[AddBehaviourMenu("RaycastHit2D/RaycastHit2D.Point")]
	[BehaviourTitle("RaycastHit2D.Point")]
	[BuiltInBehaviour]
	public sealed class RaycastHit2DPointCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit2D
		/// </summary>
		[SerializeField] private InputSlotRaycastHit2D _RaycastHit2D = new InputSlotRaycastHit2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// 衝突点を出力
		/// </summary>
#else
		/// <summary>
		/// Output the impact point
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Point = new OutputSlotVector2();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RaycastHit2D raycastHit2D = new RaycastHit2D();
			if (_RaycastHit2D.GetValue(ref raycastHit2D))
			{
				_Point.SetValue(raycastHit2D.point);
			}
		}
	}
}
