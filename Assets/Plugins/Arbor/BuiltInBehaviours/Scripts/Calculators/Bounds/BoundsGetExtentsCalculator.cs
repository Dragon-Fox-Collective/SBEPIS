//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Boundsの範囲を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the extents of the Bounds.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.GetExtents")]
	[BehaviourTitle("Bounds.GetExtents")]
	[BuiltInBehaviour]
	public sealed class BoundsGetExtentsCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Bounds
		/// </summary>
		[SerializeField] private FlexibleBounds _Bounds = new FlexibleBounds();

#if ARBOR_DOC_JA
		/// <summary>
		/// Boundsの範囲の出力。
		/// </summary>
#else
		/// <summary>
		/// Result the extents of the Bounds.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Extents = new OutputSlotVector3();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Extents.SetValue(_Bounds.value.extents);
		}
	}
}
