//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ColorのG成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the G component of Color.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Color/Color.GetG")]
	[BehaviourTitle("Color.GetG")]
	[BuiltInBehaviour]
	public sealed class ColorGetGCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Color
		/// </summary>
		[SerializeField] private FlexibleColor _Color = new FlexibleColor();

#if ARBOR_DOC_JA
		/// <summary>
		/// G成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output G component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _G = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_G.SetValue(_Color.value.g);
		}
	}
}
