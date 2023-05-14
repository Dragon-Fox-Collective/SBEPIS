//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 位置やバウンズを含むように、拡大・縮小する。
	/// </summary>
#else
	/// <summary>
	/// Grow the bounds to encapsulate the bounds.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.EncapsulateBounds")]
	[BehaviourTitle("Bounds.EncapsulateBounds")]
	[BuiltInBehaviour]
	public sealed class BoundsEncapsulateBoundsCalculator : Calculator
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
		[SerializeField] private OutputSlotBounds _Result = new OutputSlotBounds();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Bounds bounds = _Bounds1.value;
			bounds.Encapsulate(_Bounds2.value);
			_Result.SetValue(bounds);
		}
	}
}
