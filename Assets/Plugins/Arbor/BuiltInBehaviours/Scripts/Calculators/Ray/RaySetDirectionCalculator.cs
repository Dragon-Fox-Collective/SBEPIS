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
	/// Rayの方向を設定する。
	/// </summary>
#else
	/// <summary>
	/// Set the Ray direction.
	/// </summary>
#endif
	[AddBehaviourMenu("Ray/Ray.SetDirection")]
	[BehaviourTitle("Ray.SetDirection")]
	[BuiltInBehaviour]
	[AddComponentMenu("")]
	public sealed class RaySetDirectionCalculator : Calculator
	{
		/// <summary>
		/// Ray
		/// </summary>
		[SerializeField]
		private InputSlotRay _Ray = new InputSlotRay();

#if ARBOR_DOC_JA
		/// <summary>
		/// 方向
		/// </summary>
#else
		/// <summary>
		/// Direction
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _Direction = new FlexibleVector3();

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
				ray.direction = _Direction.value;
				_Result.SetValue(ray);
			}
		}
	}
}