//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// floatをintに変換する。
	/// </summary>
#else
	/// <summary>
	/// Convert float to int.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Float/Float.ToInt")]
	[BehaviourTitle("Float.ToInt")]
	[BuiltInBehaviour]
	public sealed class FloatToIntCalculator : Calculator
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
		[SerializeField] FlexibleFloat _Value = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] OutputSlotInt _Result = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue((int)_Value.value);
		}
	}
}
