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
	/// Boundsの中心座標を返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the position of the center of the Bounds.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.GetCenter")]
	[BehaviourTitle("Bounds.GetCenter")]
	[BuiltInBehaviour]
	public sealed class BoundsGetCenterCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Bounds
		/// </summary>
		[SerializeField] private FlexibleBounds _Bounds = new FlexibleBounds();

#if ARBOR_DOC_JA
		/// <summary>
		/// 中心座標の出力
		/// </summary>
#else
		/// <summary>
		/// Output the position of the center of the rectangle.
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Center = new OutputSlotVector3();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Center.SetValue(_Bounds.value.center);
		}
	}
}
