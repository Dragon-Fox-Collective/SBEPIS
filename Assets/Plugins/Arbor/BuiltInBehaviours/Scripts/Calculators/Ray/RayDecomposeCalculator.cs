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
	/// Rayを分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose Ray.
	/// </summary>
#endif
	[AddBehaviourMenu("Ray/Ray.Decompose")]
	[BehaviourTitle("Ray.Decompose")]
	[BuiltInBehaviour]
	[AddComponentMenu("")]
	public sealed class RayDecomposeCalculator : Calculator
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
			Ray ray = new Ray();
			if (_Ray.GetValue(ref ray))
			{
				_Origin.SetValue(ray.origin);
				_Direction.SetValue(ray.direction);
			}
		}
	}
}