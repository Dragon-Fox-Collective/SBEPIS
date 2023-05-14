//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 原点からヒットしたオブジェクトの距離
	/// </summary>
#else
	/// <summary>
	/// The distance from the ray's origin to the impact point.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RaycastHit2D/RaycastHit2D.Distance")]
	[BehaviourTitle("RaycastHit2D.Distance")]
	[BuiltInBehaviour]
	public sealed class RaycastHit2DDistanceCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit2D
		/// </summary>
		[SerializeField] private InputSlotRaycastHit2D _RaycastHit2D = new InputSlotRaycastHit2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// 当たった距離を出力
		/// </summary>
#else
		/// <summary>
		/// Output the hit distance
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Distance = new OutputSlotFloat();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RaycastHit2D raycastHit2D = new RaycastHit2D();
			if (_RaycastHit2D.GetValue(ref raycastHit2D))
			{
				_Distance.SetValue(raycastHit2D.distance);
			}
		}
	}
}
