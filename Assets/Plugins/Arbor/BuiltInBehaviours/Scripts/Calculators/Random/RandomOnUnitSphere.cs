//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 単球上のランダムな点を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output a random point on the unit sphere.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Random/Random.OnUnitSphere")]
	[BehaviourTitle("Random.OnUnitSphere")]
	[BuiltInBehaviour]
	public sealed class RandomOnUnitSphere : Calculator
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
		private OutputSlotVector3 _Output = new OutputSlotVector3();

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			_Output.SetValue(Random.onUnitSphere);
		}
	}
}