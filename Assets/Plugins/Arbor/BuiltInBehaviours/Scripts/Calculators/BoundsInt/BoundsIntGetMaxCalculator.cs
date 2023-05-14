//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntの最大コーナーの位置を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the position of the maximum corner of the BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.GetMax")]
	[BehaviourTitle("BoundsInt.GetMax")]
	[BuiltInBehaviour]
	public sealed class BoundsIntGetMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsIntの最大コーナーの位置の出力。
		/// </summary>
#else
		/// <summary>
		/// Result the position of the maximum corner of the BoundsInt.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3Int _Max = new OutputSlotVector3Int();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Max.SetValue(_BoundsInt.value.max);
		}
	}
}