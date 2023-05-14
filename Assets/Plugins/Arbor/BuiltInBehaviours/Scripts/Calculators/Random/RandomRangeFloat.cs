//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2値間のランダムなfloat値を出力する。
	/// </summary>
#else
	/// <summary>
	/// Outputs a random float value between two values.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Random/Random.RangeFloat")]
	[BehaviourTitle("Random.RangeFloat")]
	[BuiltInBehaviour]
	public sealed class RandomRangeFloat : Calculator
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 最小値
		/// </summary>
#else
		/// <summary>
		/// Min value
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Min = new FlexibleFloat(0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// 最大値
		/// </summary>
#else
		/// <summary>
		/// Max value
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Max = new FlexibleFloat(0f);

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
			_Output.SetValue(Random.Range(_Min.value, _Max.value));
		}
	}
}