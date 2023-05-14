//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// enumフラグが含まれているかを返す
	/// </summary>
#else
	/// <summary>
	/// Returns whether the enum flag is contained
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("EnumFlags/EnumFlags.Contains")]
	[BehaviourTitle("EnumFlags.Contains")]
	[BuiltInBehaviour]
	public sealed class EnumFlagsContainsCalculator : Calculator, INodeBehaviourSerializationCallbackReceiver
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// enumフラグの型
		/// </summary>
#else
		/// <summary>
		/// Type of enum flag
		/// </summary>
#endif
		[SerializeField]
		[ClassEnumFlagsConstraint]
		private ClassTypeReference _Type = new ClassTypeReference();

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
		/// 結果を出力。(値1に値2が含まれているならtrueを返す、そうでなければfalseを返す)
		/// </summary>
#else
		/// <summary>
		/// Output the result. (Return true if Value1 contains Value2, otherwise return false)
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotBool _Result = new OutputSlotBool();

		// Use this for calculate
		public override void OnCalculate()
		{
			System.Type type = _Type.type;
			if (type != null && EnumFieldUtility.IsEnum(type))
			{
				_Result.SetValue((_Value1.value & _Value2.value) == _Value2.value);
			}
			else
			{
				Debug.LogWarning("The type is not an enum type.");
			}
		}

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			System.Type type = _Type.type;
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