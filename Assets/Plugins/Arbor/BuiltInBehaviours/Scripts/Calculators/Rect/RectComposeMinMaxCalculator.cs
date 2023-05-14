//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// MinとMaxからRectを作成する。
	/// </summary>
#else
	/// <summary>
	/// Create Rect from Min and Max.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.ComposeMinMax")]
	[BehaviourTitle("Rect.ComposeMinMax")]
	[BuiltInBehaviour]
	public sealed class RectComposeMinMaxCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// X 座標の最低値
		/// </summary>
#else
		/// <summary>
		/// The minimum X coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _XMin = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y 座標の最低値
		/// </summary>
#else
		/// <summary>
		/// The minimum Y coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _YMin = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// X 座標の最高値
		/// </summary>
#else
		/// <summary>
		/// The maximum X coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _XMax = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y 座標の最高値
		/// </summary>
#else
		/// <summary>
		/// The maximum Y coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _YMax = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotRect _Result = new OutputSlotRect();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(Rect.MinMaxRect(_XMin.value, _YMin.value, _XMax.value, _YMax.value));
		}
	}
}
