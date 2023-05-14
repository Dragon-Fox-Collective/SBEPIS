//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2 つのColorの各成分を乗算する。
	/// </summary>
#else
	/// <summary>
	/// Multiplies two Color component-wise.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Color/Color.Scale")]
	[BehaviourTitle("Color.Scale")]
	[BuiltInBehaviour]
	public sealed class ColorScaleCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Color
		/// </summary>
		[SerializeField] private FlexibleColor _Color = new FlexibleColor();

#if ARBOR_DOC_JA
		/// <summary>
		/// 乗算するColor
		/// </summary>
#else
		/// <summary>
		/// Color to multiply
		/// </summary>
#endif
		[SerializeField] private FlexibleColor _Scale = new FlexibleColor();

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
			_Result.SetValue(_Color.value * _Scale.value);
		}
	}
}
