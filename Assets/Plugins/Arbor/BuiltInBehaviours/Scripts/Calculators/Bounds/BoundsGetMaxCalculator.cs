//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Boundsの最大コーナーの位置を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the position of the maximum corner of the Bounds.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.GetMax")]
	[BehaviourTitle("Bounds.GetMax")]
	[BuiltInBehaviour]
	public sealed class BoundsGetMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Bounds
		/// </summary>
		[SerializeField] private FlexibleBounds _Bounds = new FlexibleBounds();

#if ARBOR_DOC_JA
		/// <summary>
		/// Boundsの最大コーナーの位置の出力。
		/// </summary>
#else
		/// <summary>
		/// Result the position of the maximum corner of the Bounds.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Max = new OutputSlotVector3();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Max.SetValue(_Bounds.value.max);
		}
	}
}
