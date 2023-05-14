﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ColorのA成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the A component of Color.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Color/Color.GetA")]
	[BehaviourTitle("Color.GetA")]
	[BuiltInBehaviour]
	public sealed class ColorGetACalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Color
		/// </summary>
		[SerializeField] private FlexibleColor _Color = new FlexibleColor();

#if ARBOR_DOC_JA
		/// <summary>
		/// A成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output A component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _A = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_A.SetValue(_Color.value.a);
		}
	}
}
