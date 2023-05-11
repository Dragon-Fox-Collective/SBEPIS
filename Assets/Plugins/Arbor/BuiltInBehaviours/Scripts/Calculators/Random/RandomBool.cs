//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ランダムなbool値を出力する。
	/// </summary>
#else
	/// <summary>
	/// Outputs a random bool value.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Random/Random.Bool")]
	[BehaviourTitle("Random.Bool")]
	[BuiltInBehaviour]
	public sealed class RandomBool : Calculator
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// trueになる確率
		/// </summary>
#else
		/// <summary>
		/// Probability to be true
		/// </summary>
#endif
		[SerializeField]
		[ConstantRange(0, 1)]
		private FlexibleFloat _Probability = new FlexibleFloat(0.0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// 出力スロット
		/// </summary>
#else
		/// <summary>
		/// Output slot
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotBool _Output = new OutputSlotBool();

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			_Output.SetValue(Random.value <= Mathf.Clamp01(_Probability.value));
		}
	}
}