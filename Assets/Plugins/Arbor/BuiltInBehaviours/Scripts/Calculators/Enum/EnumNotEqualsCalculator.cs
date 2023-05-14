//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2つのEnumが等しくない場合にtrueを返す。
	/// </summary>
#else
	/// <summary>
	/// Returns true if the two Enums are not equal.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Enum/Enum.NotEquals")]
	[BehaviourTitle("Enum.NotEquals")]
	[BuiltInBehaviour]
	public class EnumNotEqualsCalculator : Calculator, INodeBehaviourSerializationCallbackReceiver
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型
		/// </summary>
#else
		/// <summary>
		/// Enum type
		/// </summary>
#endif
		[SerializeField]
		[ClassEnumFieldConstraint]
		private ClassTypeReference _Type = new ClassTypeReference();

#if ARBOR_DOC_JA
		/// <summary>
		/// 値1
		/// </summary>
#else
		/// <summary>
		/// Value 1
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
		/// Value 2
		/// </summary>
#endif
		[SerializeField]
		private FlexibleEnumAny _Value2 = new FlexibleEnumAny();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotBool _Result = new OutputSlotBool();

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(_Value1.value != _Value2.value);
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
			else
			{
				_Value1.overrideConstraint = null;
				_Value2.overrideConstraint = null;
			}
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
		}
	}
}