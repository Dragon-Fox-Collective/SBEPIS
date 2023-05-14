//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2つのBoolが等しくない場合にtrueを返す。
	/// </summary>
#else
	/// <summary>
	/// Returns true if the Bool are not the same.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bool/Bool.NotEquals")]
	[BehaviourTitle("Bool.NotEquals")]
	[BuiltInBehaviour]
	public sealed class BoolNotEqualsCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 値1
		/// </summary>
#else
		/// <summary>
		/// Value 1
		/// </summary>
#endif
		[SerializeField] private FlexibleBool _Value1 = new FlexibleBool();

#if ARBOR_DOC_JA
		/// <summary>
		/// 値2
		/// </summary>
#else
		/// <summary>
		/// Value 2
		/// </summary>
#endif
		[SerializeField] private FlexibleBool _Value2 = new FlexibleBool();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotBool _Result = new OutputSlotBool();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			bool value1 = _Value1.value;
			bool value2 = _Value2.value;
			_Result.SetValue(value1 != value2);
		}
	}
}
