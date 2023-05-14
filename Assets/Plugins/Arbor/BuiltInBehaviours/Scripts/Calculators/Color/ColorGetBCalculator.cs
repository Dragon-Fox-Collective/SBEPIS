//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ColorのB成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the B component of Color.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Color/Color.GetB")]
	[BehaviourTitle("Color.GetB")]
	[BuiltInBehaviour]
	public sealed class ColorGetBCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Color
		/// </summary>
		[SerializeField] private FlexibleColor _Color = new FlexibleColor();

#if ARBOR_DOC_JA
		/// <summary>
		/// B成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output B component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _B = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_B.SetValue(_Color.value.b);
		}
	}
}
