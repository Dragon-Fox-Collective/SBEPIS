//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 補間パラメータTにより0からLengthの間をループさせる。
	/// </summary>
#else
	/// <summary>
	/// Loop from 0 to Length by interpolation parameter T.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Mathf/Mathf.Repeat")]
	[BehaviourTitle("Mathf.Repeat")]
	[BuiltInBehaviour]
	public sealed class MathfRepeatCalculator : Calculator
	{
		#region Serialize fields

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
		/// 長さ
		/// </summary>
#else
		/// <summary>
		/// Length
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Length = new FlexibleFloat();

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
			_Result.SetValue(Mathf.Repeat(_T.value, _Length.value));
		}
	}
}
