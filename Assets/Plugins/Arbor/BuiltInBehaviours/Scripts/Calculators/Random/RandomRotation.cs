//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ランダムな回転を出力する。
	/// </summary>
#else
	/// <summary>
	/// Outputs a random rotation.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Random/Random.Rotation")]
	[BehaviourTitle("Random.Rotation")]
	[BuiltInBehaviour]
	public sealed class RandomRotation : Calculator
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
		private OutputSlotQuaternion _Output = new OutputSlotQuaternion();

		public override bool OnCheckDirty()
		{
			return true;
		}

		// Use this for calculate
		public override void OnCalculate()
		{
			_Output.SetValue(Random.rotation);
		}
	}
}