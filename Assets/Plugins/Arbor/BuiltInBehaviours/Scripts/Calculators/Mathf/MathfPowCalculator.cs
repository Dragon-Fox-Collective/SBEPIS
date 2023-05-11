//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ValueのPower乗の値を計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculates Value raised to power Power.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Mathf/Mathf.Pow")]
	[BehaviourTitle("Mathf.Pow")]
	[BuiltInBehaviour]
	public sealed class MathfPowCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 基数の値
		/// </summary>
#else
		/// <summary>
		/// Radix value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Value = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 指数の値
		/// </summary>
#else
		/// <summary>
		/// The value of the exponent
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Power = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Result = new OutputSlotFloat();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(Mathf.Pow(_Value.value, _Power.value));
		}
	}
}
