//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 入力軸の値(Input.GetAxis)を取得する。
	/// </summary>
#else
	/// <summary>
	/// Get the value of the input axis (Input.GetAxis).
	/// </summary>
#endif
	[AddBehaviourMenu("Input/Input.GetAxis")]
	[BehaviourTitle("Input.GetAxis")]
	[BuiltInBehaviour]
	[AddComponentMenu("")]
	public sealed class InputGetAxisCalculator : Calculator
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 軸の名前
		/// </summary>
#else
		/// <summary>
		/// Axis name
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _AxisName = new FlexibleString("");

#if ARBOR_DOC_JA
		/// <summary>
		/// 値の出力
		/// </summary>
#else
		/// <summary>
		/// Output value
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotFloat _Output = new OutputSlotFloat();

		public override bool OnCheckDirty()
		{
			return true;
		}

		public override void OnCalculate()
		{
			_Output.SetValue(Input.GetAxis(_AxisName.value));
		}
	}
}
#endif