//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2値間のランダムなint値を出力する。
	/// </summary>
#else
	/// <summary>
	/// Outputs a random int value between two values.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Random/Random.RangeInt")]
	[BehaviourTitle("Random.RangeInt")]
	[BuiltInBehaviour]
	public sealed class RandomRangeInt : Calculator
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
		private FlexibleInt _Min = new FlexibleInt(0);

#if ARBOR_DOC_JA
		/// <summary>
		/// 最大値 [含まれない]
		/// </summary>
#else
		/// <summary>
		/// Max value [Exclusive]
		/// </summary>
#endif
		[SerializeField]
		private FlexibleInt _Max = new FlexibleInt(0);

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
		private OutputSlotInt _Output = new OutputSlotInt();

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