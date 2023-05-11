//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// FromからToの範囲内で角度を線形補間する。
	/// </summary>
#else
	/// <summary>
	/// Linearly interpolate the angle within the range from From to To.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Mathf/Mathf.LerpAngle")]
	[BehaviourTitle("Mathf.LerpAngle")]
	[BuiltInBehaviour]
	public sealed class MathfLerpAngleCalculator : Calculator
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
		/// 補間パラメータ
		/// </summary>
#else
		/// <summary>
		/// Interpolation parameter
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _T = new FlexibleFloat();

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
			_Result.SetValue(Mathf.LerpAngle(_From.value, _To.value, _T.value));
		}
	}
}
