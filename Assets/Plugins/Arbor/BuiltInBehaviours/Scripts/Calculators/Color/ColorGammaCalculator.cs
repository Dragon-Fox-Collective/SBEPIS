//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Colorのガンマを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the gamma of Color.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Color/Color.Gamma")]
	[BehaviourTitle("Color.Gamma")]
	[BuiltInBehaviour]
	public sealed class ColorGammaCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Color
		/// </summary>
		[SerializeField] private FlexibleColor _Color = new FlexibleColor();

#if ARBOR_DOC_JA
		/// <summary>
		/// ガンマの出力
		/// </summary>
#else
		/// <summary>
		/// Output gamma
		/// </summary>
#endif
		[SerializeField] private OutputSlotColor _Gamma = new OutputSlotColor();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Gamma.SetValue(_Color.value.gamma);
		}
	}
}
