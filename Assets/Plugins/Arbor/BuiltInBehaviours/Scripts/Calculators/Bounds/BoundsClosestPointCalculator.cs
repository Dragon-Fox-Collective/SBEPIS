//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 指定した位置に近接しているバウンディングボックス上の位置を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the position on the bounding box that is close to the specified position.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.ClosestPoint")]
	[BehaviourTitle("Bounds.ClosestPoint")]
	[BuiltInBehaviour]
	public sealed class BoundsClosestPointCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Bounds
		/// </summary>
		[SerializeField] private FlexibleBounds _Bounds = new FlexibleBounds();

#if ARBOR_DOC_JA
		/// <summary>
		/// 位置
		/// </summary>
#else
		/// <summary>
		/// Point
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Point = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Result = new OutputSlotVector3();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(_Bounds.value.ClosestPoint(_Point.value));
		}
	}
}
