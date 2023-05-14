//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2つのint値から最小値を計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculate the minimum value from two int values.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Mathf/Mathf.MinInt")]
	[BehaviourTitle("Mathf.MinInt")]
	[BuiltInBehaviour]
	public sealed class MathfMinIntCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 値A
		/// </summary>
#else
		/// <summary>
		/// Value A
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _A = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 値B
		/// </summary>
#else
		/// <summary>
		/// Value B
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _B = new FlexibleInt();

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
			_Result.SetValue(Mathf.Min(_A.value, _B.value));
		}
	}
}
