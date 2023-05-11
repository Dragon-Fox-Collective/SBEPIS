//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// FromとToの間で制限したスムージングで補間する。
	/// </summary>
#else
	/// <summary>
	/// Interpolate with limited smoothing between From and To.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Mathf/Mathf.SmoothStep")]
	[BehaviourTitle("Mathf.SmoothStep")]
	[BuiltInBehaviour]
	public sealed class MathfSmoothStepCalculator : Calculator
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
			_Result.SetValue(Mathf.SmoothStep(_From.value, _To.value, _T.value));
		}
	}
}
