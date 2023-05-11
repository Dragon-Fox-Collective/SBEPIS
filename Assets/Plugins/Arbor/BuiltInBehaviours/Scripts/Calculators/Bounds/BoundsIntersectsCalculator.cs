//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// バウンディングボックス同士の交差を判定する。
	/// </summary>
#else
	/// <summary>
	/// Determine the intersection of bounding boxes.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.Intersects")]
	[BehaviourTitle("Bounds.Intersects")]
	[BuiltInBehaviour]
	public sealed class BoundsIntersectsCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Bounds 1
		/// </summary>
		[SerializeField] private FlexibleBounds _Bounds1 = new FlexibleBounds();

		/// <summary>
		/// Bounds 2
		/// </summary>
		[SerializeField] private FlexibleBounds _Bounds2 = new FlexibleBounds();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotBool _Result = new OutputSlotBool();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(_Bounds1.value.Intersects(_Bounds2.value));
		}
	}
}
