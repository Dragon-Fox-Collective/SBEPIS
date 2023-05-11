//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Graient内のランダムな時間の色を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the color of random time in Gradient.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Random/Random.RangeColor")]
	[BehaviourTitle("Random.RangeColor")]
	[BuiltInBehaviour]
	public sealed class RandomRangeColor : Calculator
	{
		/// <summary>
		/// Gradient
		/// </summary>
		[SerializeField]
		private FlexibleGradient _Gradient = new FlexibleGradient(new Gradient());

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
			Gradient gradient = _Gradient.value;
			if (gradient == null)
			{
				gradient = new Gradient();
			}
			_Output.SetValue(gradient.Evaluate(Random.value));
		}
	}
}