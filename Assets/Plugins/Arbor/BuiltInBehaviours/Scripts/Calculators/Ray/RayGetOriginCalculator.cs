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
	/// Rayの原点を取得する
	/// </summary>
#else
	/// <summary>
	/// Get the origin of Ray
	/// </summary>
#endif
	[AddBehaviourMenu("Ray/Ray.GetOrigin")]
	[BehaviourTitle("Ray.GetOrigin")]
	[BuiltInBehaviour]
	[AddComponentMenu("")]
	public sealed class RayGetOriginCalculator : Calculator
	{
		/// <summary>
		/// Ray
		/// </summary>
		[SerializeField]
		private InputSlotRay _Ray = new InputSlotRay();

#if ARBOR_DOC_JA
		/// <summary>
		/// 原点を出力
		/// </summary>
#else
		/// <summary>
		/// Output origin
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotVector3 _Origin = new OutputSlotVector3();

		// Use this for calculate
		public override void OnCalculate()
		{
			Ray ray = default;
			if (_Ray.GetValue(ref ray))
			{
				_Origin.SetValue(ray.origin);
			}
		}
	}
}