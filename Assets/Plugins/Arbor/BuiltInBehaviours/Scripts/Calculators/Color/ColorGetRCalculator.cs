//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ColorのR成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the R component of Color.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Color/Color.GetR")]
	[BehaviourTitle("Color.GetR")]
	[BuiltInBehaviour]
	public sealed class ColorGetRCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Color
		/// </summary>
		[SerializeField] private FlexibleColor _Color = new FlexibleColor();

#if ARBOR_DOC_JA
		/// <summary>
		/// R成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output R component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _R = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_R.SetValue(_Color.value.r);
		}
	}
}
