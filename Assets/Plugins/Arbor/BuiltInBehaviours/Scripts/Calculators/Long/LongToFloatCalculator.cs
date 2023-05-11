//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// longをfloatに変換する。
	/// </summary>
#else
	/// <summary>
	/// Convert long to float.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Long/Long.ToFloat")]
	[BehaviourTitle("Long.ToFloat")]
	[BuiltInBehaviour]
	public sealed class LongToFloatCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 値
		/// </summary>
#else
		/// <summary>
		/// Value
		/// </summary>
#endif
		[SerializeField] private FlexibleLong _Value = new FlexibleLong();

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
			_Result.SetValue((float)_Value.value);
		}
	}
}
