﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2 つのベクトルの各成分を乗算する。
	/// </summary>
#else
	/// <summary>
	/// Multiplies two vectors component-wise.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3/Vector3.Scale")]
	[BehaviourTitle("Vector3.Scale")]
	[BuiltInBehaviour]
	public sealed class Vector3ScaleCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Vector3
		/// </summary>
		[SerializeField] private FlexibleVector3 _Vector3 = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 乗算するベクトル
		/// </summary>
#else
		/// <summary>
		/// Vector to multiply
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Scale = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Result = new OutputSlotVector3();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Result.SetValue(Vector3.Scale(_Vector3.value, _Scale.value));
		}
	}
}
