//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 値以下の最大の整数を計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculate the largest integer less than or equal to the value.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Mathf/Mathf.FloorToInt")]
	[BehaviourTitle("Mathf.FloorToInt")]
	[BuiltInBehaviour]
	public sealed class MathfFloorToIntCalculator : Calculator
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
		[SerializeField] private OutputSlotInt _Result = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(Mathf.FloorToInt(_Value.value));
		}
	}
}
