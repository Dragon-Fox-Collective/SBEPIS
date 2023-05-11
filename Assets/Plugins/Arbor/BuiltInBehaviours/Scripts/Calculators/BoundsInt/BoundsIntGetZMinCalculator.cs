//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntのZMin成分を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the ZMin component of the BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.GetZMin")]
	[BehaviourTitle("BoundsInt.GetZMin")]
	[BuiltInBehaviour]
	public sealed class BoundsIntGetZMinCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// ZMin成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output ZMin component
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _ZMin = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_ZMin.SetValue(_BoundsInt.value.zMin);
		}
	}
}