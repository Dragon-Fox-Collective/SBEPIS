//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2値間のランダムなVector2値を出力する。
	/// </summary>
#else
	/// <summary>
	/// Outputs a random Vector2 value between two values.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Random/Random.RangeVector2")]
	[BehaviourTitle("Random.RangeVector2")]
	[BuiltInBehaviour]
	public sealed class RandomRangeVector2 : Calculator
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 1つ目のVector2
		/// </summary>
#else
		/// <summary>
		/// 1st Vector2
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector2 _VectorA = new FlexibleVector2(Vector2.zero);

#if ARBOR_DOC_JA
		/// <summary>
		/// 2つ目のVector2
		/// </summary>
#else
		/// <summary>
		/// 2nd Vector2
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector2 _VectorB = new FlexibleVector2(Vector2.zero);

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
		private OutputSlotVector2 _Output = new OutputSlotVector2();

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			_Output.SetValue(Vector2.Lerp(_VectorA.value, _VectorB.value, Random.value));
		}
	}
}