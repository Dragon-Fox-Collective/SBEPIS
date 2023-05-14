//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// RectIntをMinとMaxに分解する。
	/// </summary>
#else
	/// <summary>
	/// Decompose RectInt into Min and Max.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectInt/RectInt.DecomposeMinMax")]
	[BehaviourTitle("RectInt.DecomposeMinMax")]
	[BuiltInBehaviour]
	public sealed class RectIntDecomposeMinMaxCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RectInt
		/// </summary>
		[SerializeField] private FlexibleRectInt _RectInt = new FlexibleRectInt();

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

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RectInt rect = _RectInt.value;
			_XMin.SetValue(rect.xMin);
			_YMin.SetValue(rect.yMin);
			_XMax.SetValue(rect.xMax);
			_YMax.SetValue(rect.yMax);
		}
	}
}