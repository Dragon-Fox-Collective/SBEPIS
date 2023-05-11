//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Enumをintに変換する。
	/// </summary>
#else
	/// <summary>
	/// Convert Enum to int.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Enum/Enum.ToInt")]
	[BehaviourTitle("Enum.ToInt")]
	[BuiltInBehaviour]
	public class EnumToIntCalculator : Calculator, INodeBehaviourSerializationCallbackReceiver
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
		/// 値
		/// </summary>
#else
		/// <summary>
		/// Value
		/// </summary>
#endif
		[SerializeField]
		private FlexibleEnumAny _Value = new FlexibleEnumAny();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotInt _Output = new OutputSlotInt();

		// Use this for calculate
		public override void OnCalculate()
		{
			_Output.SetValue(_Value.value);
		}

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			System.Type type = _Type.type;
			if (type != null && EnumFieldUtility.IsEnum(type))
			{
				_Value.overrideConstraint = new ClassConstraintInfo() { baseType = type };
			}
			else
			{
				_Value.overrideConstraint = null;
			}
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
		}
	}
}