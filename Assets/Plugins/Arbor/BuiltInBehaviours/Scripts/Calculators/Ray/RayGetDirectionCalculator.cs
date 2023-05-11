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
	/// Rayの方向を取得する。
	/// </summary>
#else
	/// <summary>
	/// Get the direction of Ray.
	/// </summary>
#endif
	[AddBehaviourMenu("Ray/Ray.GetDirection")]
	[BehaviourTitle("Ray.GetDirection")]
	[BuiltInBehaviour]
	[AddComponentMenu("")]
	public sealed class RayGetDirectionCalculator : Calculator
	{
		/// <summary>
		/// Ray
		/// </summary>
		[SerializeField]
		private InputSlotRay _Ray = new InputSlotRay();

#if ARBOR_DOC_JA
		/// <summary>
		/// 方向を出力
		/// </summary>
#else
		/// <summary>
		/// Output direction
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotVector3 _Direction = new OutputSlotVector3();

		// Use this for calculate
		public override void OnCalculate()
		{
			Ray ray = default;
			if (_Ray.GetValue(ref ray))
			{
				_Direction.SetValue(ray.direction);
			}
		}
	}
}