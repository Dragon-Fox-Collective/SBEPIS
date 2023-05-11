//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Ray上の点を取得する。
	/// </summary>
#else
	/// <summary>
	/// Get a point on Ray.
	/// </summary>
#endif
	[AddBehaviourMenu("Ray/Ray.GetPoint")]
	[BehaviourTitle("Ray.GetPoint")]
	[BuiltInBehaviour]
	[AddComponentMenu("")]
	public sealed class RayGetPointCalculator : Calculator
	{
		/// <summary>
		/// Ray
		/// </summary>
		[SerializeField]
		private InputSlotRay _Ray = new InputSlotRay();

#if ARBOR_DOC_JA
		/// <summary>
		/// 距離
		/// </summary>
#else
		/// <summary>
		/// Distance
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Distance = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Ray上の点を出力
		/// </summary>
#else
		/// <summary>
		/// Output points on Ray
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotVector3 _Point = new OutputSlotVector3();

		// Use this for calculate
		public override void OnCalculate()
		{
			Ray ray = default;
			if (_Ray.GetValue(ref ray))
			{
				_Point.SetValue(ray.GetPoint(_Distance.value));
			}
		}
	}
}