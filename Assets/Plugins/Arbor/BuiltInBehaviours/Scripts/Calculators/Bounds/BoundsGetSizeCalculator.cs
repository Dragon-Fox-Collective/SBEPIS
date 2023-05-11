//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Boundsのサイズを返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the size of the Bounds.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.GetSize")]
	[BehaviourTitle("Bounds.GetSize")]
	[BuiltInBehaviour]
	public sealed class BoundsGetSizeCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Bounds
		/// </summary>
		[SerializeField] private FlexibleBounds _Bounds = new FlexibleBounds();

#if ARBOR_DOC_JA
		/// <summary>
		/// Boundsのサイズの出力。
		/// </summary>
#else
		/// <summary>
		/// Result the size of the Bounds.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Size = new OutputSlotVector3();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Size.SetValue(_Bounds.value.size);
		}
	}
}
