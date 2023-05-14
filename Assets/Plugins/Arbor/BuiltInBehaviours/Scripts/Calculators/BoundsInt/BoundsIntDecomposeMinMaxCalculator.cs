//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsIntをMinとMaxに分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose BoundsInt into Min and Max.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("BoundsInt/BoundsInt.DecomposeMinMax")]
	[BehaviourTitle("BoundsInt.DecomposeMinMax")]
	[BuiltInBehaviour]
	public sealed class BoundsIntDecomposeMinMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// BoundsInt
		/// </summary>
		[SerializeField] private FlexibleBoundsInt _BoundsInt = new FlexibleBoundsInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// X 座標の最低値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the minimum X coordinate.
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _XMin = new OutputSlotInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y 座標の最低値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the minimum Y coordinate.
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _YMin = new OutputSlotInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// Z 座標の最低値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the minimum Z coordinate.
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _ZMin = new OutputSlotInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// X 座標の最高値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the maximum X coordinate.
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _XMax = new OutputSlotInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y 座標の最低値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the maximum Y coordinate.
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _YMax = new OutputSlotInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// Z 座標の最低値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the maximum Z coordinate.
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _ZMax = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			BoundsInt bounds = _BoundsInt.value;
			_XMin.SetValue(bounds.min.x);
			_YMin.SetValue(bounds.min.y);
			_ZMin.SetValue(bounds.min.z);
			_XMax.SetValue(bounds.max.x);
			_YMax.SetValue(bounds.max.y);
			_ZMax.SetValue(bounds.max.z);
		}
	}
}