//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// RectをMinとMaxに分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose Rect into Min and Max.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.DecomposeMinMax")]
	[BehaviourTitle("Rect.DecomposeMinMax")]
	[BuiltInBehaviour]
	public sealed class RectDecomposeMinMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

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

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Rect rect = _Rect.value;
			_XMin.SetValue(rect.xMin);
			_YMin.SetValue(rect.yMin);
			_XMax.SetValue(rect.xMax);
			_YMax.SetValue(rect.yMax);
		}
	}
}
