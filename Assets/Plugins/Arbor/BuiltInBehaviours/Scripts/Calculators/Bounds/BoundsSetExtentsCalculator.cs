//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Boundsの範囲を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the extents of the Bounds.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.SetExtents")]
	[BehaviourTitle("Bounds.SetExtents")]
	[BuiltInBehaviour]
	public sealed class BoundsSetExtentsCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Bounds
		/// </summary>
		[SerializeField] private FlexibleBounds _Bounds = new FlexibleBounds();

#if ARBOR_DOC_JA
		/// <summary>
		/// Boundsの範囲
		/// </summary>
#else
		/// <summary>
		/// the extents of the Bounds.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Extents = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotBounds _Result = new OutputSlotBounds();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Bounds value = _Bounds.value;
			value.extents = _Extents.value;
			_Result.SetValue(value);
		}
	}
}
