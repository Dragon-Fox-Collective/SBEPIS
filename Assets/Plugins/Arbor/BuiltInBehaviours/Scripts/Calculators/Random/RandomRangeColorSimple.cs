//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2色間のランダムな色を出力する。
	/// </summary>
#else
	/// <summary>
	/// Outputs a random color between two colors.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Random/Random.RangeColorSimple")]
	[BehaviourTitle("Random.RangeColorSimple")]
	[BuiltInBehaviour]
	public sealed class RandomRangeColorSimple : Calculator
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 1つ目の色
		/// </summary>
#else
		/// <summary>
		/// 1st color
		/// </summary>
#endif
		[SerializeField]
		private FlexibleColor _ColorA = new FlexibleColor(Color.white);

#if ARBOR_DOC_JA
		/// <summary>
		/// 2つ目の色
		/// </summary>
#else
		/// <summary>
		/// 2nd color
		/// </summary>
#endif
		[SerializeField]
		private FlexibleColor _ColorB = new FlexibleColor(Color.white);

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
		private OutputSlotColor _Output = new OutputSlotColor();

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			_Output.SetValue(Color.Lerp(_ColorA.value, _ColorB.value, Random.value));
		}
	}
}