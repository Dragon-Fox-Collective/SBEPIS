//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Boundsを中心とサイズに分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose Bounds to size and size.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.Decompose")]
	[BehaviourTitle("Bounds.Decompose")]
	[BuiltInBehaviour]
	public sealed class BoundsDecomposeCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Bounds
		/// </summary>
		[SerializeField] private FlexibleBounds _Bounds = new FlexibleBounds();

#if ARBOR_DOC_JA
		/// <summary>
		/// 中心を出力
		/// </summary>
#else
		/// <summary>
		/// Center output
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Center = new OutputSlotVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// サイズを出力
		/// </summary>
#else
		/// <summary>
		/// Size output
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Size = new OutputSlotVector3();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Bounds bounds = _Bounds.value;

			_Center.SetValue(bounds.center);
			_Size.SetValue(bounds.size);
		}
	}
}
