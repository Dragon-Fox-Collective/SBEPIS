//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsをMinとMaxに分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose Bounds into Min and Max.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.DecomposeMinMaxVec")]
	[BehaviourTitle("Bounds.DecomposeMinMaxVec")]
	[BuiltInBehaviour]
	public sealed class BoundsDecomposeMinMaxVecCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Bounds
		/// </summary>
		[SerializeField] private FlexibleBounds _Bounds = new FlexibleBounds();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最低値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the minimum value.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Min = new OutputSlotVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最高値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the maximum value.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Max = new OutputSlotVector3();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Bounds bounds = _Bounds.value;
			_Min.SetValue(bounds.min);
			_Max.SetValue(bounds.max);
		}
	}
}
