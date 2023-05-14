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
	/// Rayの原点を設定する。
	/// </summary>
#else
	/// <summary>
	/// Set the origin of Ray.
	/// </summary>
#endif
	[AddBehaviourMenu("Ray/Ray.SetOrigin")]
	[BehaviourTitle("Ray.SetOrigin")]
	[BuiltInBehaviour]
	[AddComponentMenu("")]
	public sealed class RaySetOriginCalculator : Calculator
	{
		/// <summary>
		/// Ray
		/// </summary>
		[SerializeField]
		private InputSlotRay _Ray = new InputSlotRay();

#if ARBOR_DOC_JA
		/// <summary>
		/// 原点
		/// </summary>
#else
		/// <summary>
		/// Origin
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _Origin = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 変更結果のRayを出力
		/// </summary>
#else
		/// <summary>
		/// Output the changed Ray
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotRay _Result = new OutputSlotRay();

		// Use this for calculate
		public override void OnCalculate()
		{
			Ray ray = default;
			if (_Ray.GetValue(ref ray))
			{
				ray.origin = _Origin.value;
				_Result.SetValue(ray);
			}
		}
	}
}