//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 値を文字列に変換する。
	/// </summary>
#else
	/// <summary>
	/// Convert the value to a string.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("String/ToString")]
	[BehaviourTitle("ToString")]
	[BuiltInBehaviour]
	public sealed class ToStringCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 値の入力
		/// </summary>
#else
		/// <summary>
		/// Inpout value
		/// </summary>
#endif
		[SerializeField]
		private InputSlotAny _Input = new InputSlotAny();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotString _Output = new OutputSlotString();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			object value = null;
			if (!_Input.GetValue(ref value))
			{
				return;
			}

			if (value == null)
			{
				return;
			}

			_Output.SetValue(value.ToString());
		}
	}
}