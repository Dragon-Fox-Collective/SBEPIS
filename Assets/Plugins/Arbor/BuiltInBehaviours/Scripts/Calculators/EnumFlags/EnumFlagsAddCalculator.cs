//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// enumフラグを加算(ビット単位OR)
	/// </summary>
	/// <param name="Type">
	/// enumフラグの型
	/// </param>
#else
	/// <summary>
	/// Add enum flags(bitwise OR)
	/// </summary>
	/// <param name="Type">
	/// Type of enum flag
	/// </param>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("EnumFlags/EnumFlags.Add")]
	[BehaviourTitle("EnumFlags.Add")]
	[BuiltInBehaviour]
	public sealed class EnumFlagsAddCalculator : Calculator, INodeBehaviourSerializationCallbackReceiver
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 値1
		/// </summary>
#else
		/// <summary>
		/// Value1
		/// </summary>
#endif
		[SerializeField]
		private FlexibleEnumAny _Value1 = new FlexibleEnumAny();

#if ARBOR_DOC_JA
		/// <summary>
		/// 値2
		/// </summary>
#else
		/// <summary>
		/// Value2
		/// </summary>
#endif
		[SerializeField]
		private FlexibleEnumAny _Value2 = new FlexibleEnumAny();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果を出力。(結果 = 値1 | 値2)
		/// </summary>
#else
		/// <summary>
		/// Output the result. (Result = Value1 | Value2)
		/// </summary>
#endif
		[SerializeField]
		[HideSlotFields]
		private OutputSlotTypable _Result = new OutputSlotTypable();

		// Use this for calculate
		public override void OnCalculate()
		{
			System.Type type = _Result.dataType;
			if (type != null && EnumFieldUtility.IsEnum(type))
			{
				int intValue = _Value1.value | _Value2.value;
				_Result.SetValue(EnumFieldUtility.ToEnum(type, intValue));
			}
			else
			{
				Debug.LogWarning("The type is not an enum type.");
			}
		}

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			System.Type type = _Result.dataType;
			if (type != null && EnumFieldUtility.IsEnum(type))
			{
				var typeConstraint = new ClassConstraintInfo() { baseType = type };
				_Value1.overrideConstraint = typeConstraint;
				_Value2.overrideConstraint = typeConstraint;
			}
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
		}
	}
}