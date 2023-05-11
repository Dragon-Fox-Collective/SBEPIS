//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntの位置とサイズを指定された範囲にクランプする。
	/// </summary>
#else
	/// <summary>
	/// Clamps the position and size of the BoundsInt to the given bounds.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.ClampToBounds")]
	[BehaviourTitle("BoundsInt.ClampToBounds")]
	[BuiltInBehaviour]
	public sealed class BoundsIntClampToBoundsCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsIntをクランプするための境界。
		/// </summary>
#else
		/// <summary>
		/// Bounds to clamp the BoundsInt.
		/// </summary>
#endif
		[SerializeField] private FlexibleBoundsInt _Bounds = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotBoundsInt _Result = new OutputSlotBoundsInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			BoundsInt boundsInt = _BoundsInt.value;
			boundsInt.ClampToBounds(_Bounds.value);
			_Result.SetValue(boundsInt);
		}
	}
}