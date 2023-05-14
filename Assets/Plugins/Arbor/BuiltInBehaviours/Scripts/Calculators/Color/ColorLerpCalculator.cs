﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// FromとToのColorの間を補間パラメータTで線形補間する。
	/// </summary>
#else
	/// <summary>
	/// Linear interpolation is performed between the colors of From and To with the interpolation parameter T.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Color/Color.Lerp")]
	[BehaviourTitle("Color.Lerp")]
	[BuiltInBehaviour]
	public sealed class ColorLerpCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始Color
		/// </summary>
#else
		/// <summary>
		/// Starting Color
		/// </summary>
#endif
		[SerializeField] private FlexibleColor _From = new FlexibleColor();

#if ARBOR_DOC_JA
		/// <summary>
		/// 終了Color
		/// </summary>
#else
		/// <summary>
		/// End Color
		/// </summary>
#endif
		[SerializeField] private FlexibleColor _To = new FlexibleColor();

#if ARBOR_DOC_JA
		/// <summary>
		/// 補間パラメータ<br />
		/// [0, 1]の間にクランプされる。
		/// </summary>
#else
		/// <summary>
		/// The interpolation parameter<br />
		/// Clamped between [0, 1].
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _T = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotColor _Result = new OutputSlotColor();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Result.SetValue(Color.Lerp(_From.value, _To.value, _T.value));
		}
	}
}
