﻿//-----------------------------------------------------
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
	[AddBehaviourMenu("RaycastHit/RaycastHit.Distance")]
	[BehaviourTitle("RaycastHit.Distance")]
	[BuiltInBehaviour]
	public sealed class RaycastHitDistanceCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit
		/// </summary>
		[SerializeField] private InputSlotRaycastHit _RaycastHit = new InputSlotRaycastHit();

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
			RaycastHit raycastHit = new RaycastHit();
			if (_RaycastHit.GetValue(ref raycastHit))
			{
				_Distance.SetValue(raycastHit.distance);
			}
		}
	}
}
