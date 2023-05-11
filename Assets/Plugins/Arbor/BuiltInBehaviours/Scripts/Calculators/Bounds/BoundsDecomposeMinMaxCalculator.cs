//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BoundsをMinとMaxに分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose Bounds into Min and Max.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Bounds/Bounds.DecomposeMinMax")]
	[BehaviourTitle("Bounds.DecomposeMinMax")]
	[BuiltInBehaviour]
	public sealed class BoundsDecomposeMinMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Bounds
		/// </summary>
		[SerializeField] private FlexibleBounds _Bounds = new FlexibleBounds();

#if ARBOR_DOC_JA
		/// <summary>
		/// X 座標の最低値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the minimum X coordinate.
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _XMin = new OutputSlotFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y 座標の最低値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the minimum Y coordinate.
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _YMin = new OutputSlotFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Z 座標の最低値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the minimum Z coordinate.
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _ZMin = new OutputSlotFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// X 座標の最高値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the maximum X coordinate.
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _XMax = new OutputSlotFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y 座標の最低値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the maximum Y coordinate.
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _YMax = new OutputSlotFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Z 座標の最低値を出力。
		/// </summary>
#else
		/// <summary>
		/// Output the maximum Z coordinate.
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _ZMax = new OutputSlotFloat();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Bounds bounds = _Bounds.value;
			_XMin.SetValue(bounds.min.x);
			_YMin.SetValue(bounds.min.y);
			_ZMin.SetValue(bounds.min.z);
			_XMax.SetValue(bounds.max.x);
			_YMax.SetValue(bounds.max.y);
			_ZMax.SetValue(bounds.max.z);
		}
	}
}
