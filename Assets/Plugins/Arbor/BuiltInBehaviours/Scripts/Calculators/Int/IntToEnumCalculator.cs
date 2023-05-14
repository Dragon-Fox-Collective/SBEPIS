//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// intをenumに変換する。
	/// </summary>
#else
	/// <summary>
	/// Convert int to enum.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Int/Int.ToEnum")]
	[BehaviourTitle("Int.ToEnum")]
	[BuiltInBehaviour]
	public sealed class IntToEnumCalculator : Calculator
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// int値
		/// </summary>
#else
		/// <summary>
		/// int value
		/// </summary>
#endif
		[SerializeField]
		private FlexibleInt _Value = new FlexibleInt();

#if ARBOR_DOC_JA
		/// <summary>
		/// 変換したenum値を出力する。
		/// </summary>
#else
		/// <summary>
		/// Output the converted enum value.
		/// </summary>
#endif
		[SerializeField]
		[ClassEnumFieldConstraint]
		[HideSlotFields]
		private OutputSlotTypable _Output = new OutputSlotTypable();

		// Use this for calculate
		public override void OnCalculate()
		{
			System.Type type = _Output.dataType;
			if (type != null && EnumFieldUtility.IsEnum(type))
			{
				int intValue = _Value.value;
				_Output.SetValue(EnumFieldUtility.ToEnum(type, intValue));
			}
			else
			{
				Debug.LogWarning("The type is not an enum type.");
			}
		}
	}
}