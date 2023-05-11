//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 設定した位置を含むように拡大する。
	/// </summary>
#else
	/// <summary>
	/// Grows the Bounds to include the point.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.EncapsulateVector3")]
	[BehaviourTitle("Bounds.EncapsulateVector3")]
	[BuiltInBehaviour]
	public sealed class BoundsEncapsulateVector3Calculator : Calculator
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
		[SerializeField] private OutputSlotBounds _Result = new OutputSlotBounds();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Bounds bounds = _Bounds.value;
			bounds.Encapsulate(_Point.value);
			_Result.SetValue(bounds);
		}
	}
}
