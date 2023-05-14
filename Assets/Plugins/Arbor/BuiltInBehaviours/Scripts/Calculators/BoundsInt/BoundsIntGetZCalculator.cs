//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntのZ成分を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the Z component of the BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.GetZ")]
	[BehaviourTitle("BoundsInt.GetZ")]
	[BuiltInBehaviour]
	public sealed class BoundsIntGetZCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// Z成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output Z component
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _Z = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Z.SetValue(_BoundsInt.value.z);
		}
	}
}