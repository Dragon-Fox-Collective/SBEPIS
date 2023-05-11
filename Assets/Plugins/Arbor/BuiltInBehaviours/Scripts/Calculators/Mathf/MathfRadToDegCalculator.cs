//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ラジアンから度に変換する。
	/// </summary>
#else
	/// <summary>
	/// Convert from radians to degrees.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Mathf/Mathf.RadToDeg")]
	[BehaviourTitle("Mathf.RadToDeg")]
	[BuiltInBehaviour]
	public sealed class MathfRadToDegCalculator : Calculator
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
			_Result.SetValue(_Value.value * Mathf.Rad2Deg);
		}
	}
}
