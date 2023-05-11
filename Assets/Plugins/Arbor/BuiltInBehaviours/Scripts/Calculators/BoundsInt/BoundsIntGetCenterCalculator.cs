//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 矩形の中心座標
	/// </summary>
#else
	/// <summary>
	/// The position of the center of the rectangle.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.GetCenter")]
	[BehaviourTitle("BoundsInt.GetCenter")]
	[BuiltInBehaviour]
	public sealed class BoundsIntGetCenterCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Center = new OutputSlotVector3();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Center.SetValue(_BoundsInt.value.center);
		}
	}
}