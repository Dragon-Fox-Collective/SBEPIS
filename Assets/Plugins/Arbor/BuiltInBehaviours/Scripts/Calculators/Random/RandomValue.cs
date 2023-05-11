//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ランダムなfloat値を出力する。
	/// </summary>
#else
	/// <summary>
	/// Outputs a random float value.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Random/Random.Value")]
	[BehaviourTitle("Random.Value")]
	[BuiltInBehaviour]
	public sealed class RandomValue : Calculator
	{
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
		private OutputSlotFloat _Output = new OutputSlotFloat();

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			_Output.SetValue(Random.value);
		}
	}
}