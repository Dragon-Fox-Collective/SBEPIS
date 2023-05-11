//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntのZMax成分を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the ZMax component of the BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.GetZMax")]
	[BehaviourTitle("BoundsInt.GetZMax")]
	[BuiltInBehaviour]
	public sealed class BoundsIntGetZMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// ZMax成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output ZMax component
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _ZMax = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_ZMax.SetValue(_BoundsInt.value.zMax);
		}
	}
}