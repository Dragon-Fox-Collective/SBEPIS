//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntのY成分を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the Y component of the BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.GetY")]
	[BehaviourTitle("BoundsInt.GetY")]
	[BuiltInBehaviour]
	public sealed class BoundsIntGetYCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output Y component
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _Y = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Y.SetValue(_BoundsInt.value.y);
		}
	}
}