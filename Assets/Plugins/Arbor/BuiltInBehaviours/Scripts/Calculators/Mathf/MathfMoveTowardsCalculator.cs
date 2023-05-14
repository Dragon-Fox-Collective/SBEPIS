//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Current から Target まで、MaxDelta の速さで移動する移動量を計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculate the amount of movement to move from Current to Target at the speed of MaxDelta.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Mathf/Mathf.MoveTowards")]
	[BehaviourTitle("Mathf.MoveTowards")]
	[BuiltInBehaviour]
	public sealed class MathfMoveTowardsCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 現在値
		/// </summary>
#else
		/// <summary>
		/// The current value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Current = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動先となる値
		/// </summary>
#else
		/// <summary>
		/// The value to move towards.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Target = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 値に適用される最大の変更
		/// </summary>
#else
		/// <summary>
		/// The maximum change that should be applied to the value.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _MaxDelta = new FlexibleFloat();

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
			_Result.SetValue(Mathf.MoveTowards(_Current.value, _Target.value, _MaxDelta.value));
		}
	}
}
