//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// e (ネイピア数) を指定した乗数を計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculates e raised to the specified power.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Mathf/Mathf.Exp")]
	[BehaviourTitle("Mathf.Exp")]
	[BuiltInBehaviour]
	public sealed class MathfExpCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 指数値
		/// </summary>
#else
		/// <summary>
		/// Exponent value
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
			_Result.SetValue(Mathf.Exp(_Power.value));
		}
	}
}
