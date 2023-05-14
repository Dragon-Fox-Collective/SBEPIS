﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 矩形の高さを設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the height of the rectangle.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Rect/Rect.SetHeight")]
	[BehaviourTitle("Rect.SetHeight")]
	[BuiltInBehaviour]
	public sealed class RectSetHeightCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Rect
		/// </summary>
		[SerializeField] private FlexibleRect _Rect = new FlexibleRect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 高さ
		/// </summary>
#else
		/// <summary>
		/// Height
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Height = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotRect _Result = new OutputSlotRect();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			Rect value = _Rect.value;
			value.height = _Height.value;
			_Result.SetValue(value);
		}
	}
}
