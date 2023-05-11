//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Colorの最大成分の値を出力する。
	/// </summary>
#else
	/// <summary>
	/// Outputs the value of the maximum component of Color.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Color/Color.MaxColorComponent")]
	[BehaviourTitle("Color.MaxColorComponent")]
	[BuiltInBehaviour]
	public sealed class ColorMaxColorComponentCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Color
		/// </summary>
		[SerializeField] private FlexibleColor _Color = new FlexibleColor();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最大成分の値の出力
		/// </summary>
#else
		/// <summary>
		/// Output of maximum component value
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _MaxColorComponent = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_MaxColorComponent.SetValue(_Color.value.maxColorComponent);
		}
	}
}
