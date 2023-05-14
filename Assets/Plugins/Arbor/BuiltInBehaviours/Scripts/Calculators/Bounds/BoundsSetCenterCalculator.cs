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
	/// Boundsの中心座標を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the position of the center of the Bounds.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.SetCenter")]
	[BehaviourTitle("Bounds.SetCenter")]
	[BuiltInBehaviour]
	public sealed class BoundsSetCenterCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Bounds
		/// </summary>
		[SerializeField] private FlexibleBounds _Bounds = new FlexibleBounds();

#if ARBOR_DOC_JA
		/// <summary>
		/// 中心座標
		/// </summary>
#else
		/// <summary>
		/// The position of the center of the rectangle.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Center = new FlexibleVector3();

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
			value.center = _Center.value;
			_Result.SetValue(value);
		}
	}
}
