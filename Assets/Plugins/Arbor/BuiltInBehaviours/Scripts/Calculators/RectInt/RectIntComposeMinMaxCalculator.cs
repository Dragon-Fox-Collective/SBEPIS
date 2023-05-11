//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// MinとMaxからRectIntを作成する。
	/// </summary>
#else
	/// <summary>
	/// Create RectInt from Min and Max.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RectInt/RectInt.ComposeMinMax")]
	[BehaviourTitle("RectInt.ComposeMinMax")]
	[BuiltInBehaviour]
	public sealed class RectIntComposeMinMaxCalculator : Calculator
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
		[SerializeField] private FlexibleInt _XMin = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y 座標の最低値
		/// </summary>
#else
		/// <summary>
		/// The minimum Y coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _YMin = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// X 座標の最高値
		/// </summary>
#else
		/// <summary>
		/// The maximum X coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _XMax = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y 座標の最高値
		/// </summary>
#else
		/// <summary>
		/// The maximum Y coordinate.
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _YMax = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotRectInt _Result = new OutputSlotRectInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RectInt rectInt = new RectInt();
			rectInt.SetMinMax(new Vector2Int(_XMin.value, _YMin.value), new Vector2Int(_XMax.value, _YMax.value));
			_Result.SetValue(rectInt);
		}
	}
}