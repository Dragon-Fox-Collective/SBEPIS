//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntの位置を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the position of the BoundsInt.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.GetPosition")]
	[BehaviourTitle("BoundsInt.GetPosition")]
	[BuiltInBehaviour]
	public sealed class BoundsIntGetPositionCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsIntの位置の出力。
		/// </summary>
#else
		/// <summary>
		/// Result the position of the minimum corner of the BoundsInt.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3Int _Position = new OutputSlotVector3Int();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Position.SetValue(_BoundsInt.value.position);
		}
	}
}