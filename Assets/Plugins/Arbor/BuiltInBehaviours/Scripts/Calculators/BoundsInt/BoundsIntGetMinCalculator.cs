//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntの最小コーナーの位置を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the position of the minimum corner of the BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.GetMin")]
	[BehaviourTitle("BoundsInt.GetMin")]
	[BuiltInBehaviour]
	public sealed class BoundsIntGetMinCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsIntの最小コーナーの位置の出力。
		/// </summary>
#else
		/// <summary>
		/// Result the position of the minimum corner of the BoundsInt.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3Int _Min = new OutputSlotVector3Int();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Min.SetValue(_BoundsInt.value.min);
		}
	}
}