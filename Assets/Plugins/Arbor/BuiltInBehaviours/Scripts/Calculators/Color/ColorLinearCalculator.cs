//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Colorのリニアカラーを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the linear color of Color.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Color/Color.Linear")]
	[BehaviourTitle("Color.Linear")]
	[BuiltInBehaviour]
	public sealed class ColorLinearCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Color
		/// </summary>
		[SerializeField] private FlexibleColor _Color = new FlexibleColor();

#if ARBOR_DOC_JA
		/// <summary>
		/// リニアカラーの出力
		/// </summary>
#else
		/// <summary>
		/// Output linear color
		/// </summary>
#endif
		[SerializeField] private OutputSlotColor _Linear = new OutputSlotColor();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Linear.SetValue(_Color.value.linear);
		}
	}
}
