//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Colorのグレースケールを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the grayscale of Color.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Color/Color.Grayscale")]
	[BehaviourTitle("Color.Grayscale")]
	[BuiltInBehaviour]
	public sealed class ColorGrayscaleCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Color
		/// </summary>
		[SerializeField] private FlexibleColor _Color = new FlexibleColor();

#if ARBOR_DOC_JA
		/// <summary>
		/// グレースケールの出力
		/// </summary>
#else
		/// <summary>
		/// Output grayscale
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Grayscale = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Grayscale.SetValue(_Color.value.grayscale);
		}
	}
}
