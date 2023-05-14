//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// FromからToの範囲内で補間する値を生成する補間パラメーターを計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculate a interpolation parameter that generates a value to be interpolated within the range from From to To.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Mathf/Mathf.InverseLerp")]
	[BehaviourTitle("Mathf.InverseLerp")]
	[BuiltInBehaviour]
	public sealed class MathfInverseLerpCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始値
		/// </summary>
#else
		/// <summary>
		/// Starting value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _From = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 終了値
		/// </summary>
#else
		/// <summary>
		/// End value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _To = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 現在値
		/// </summary>
#else
		/// <summary>
		/// Current value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Value = new FlexibleFloat();

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
			_Result.SetValue(Mathf.InverseLerp(_From.value, _To.value, _Value.value));
		}
	}
}
