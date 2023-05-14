//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Reflection;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なenum型を扱うクラス。
	/// </summary>
	/// <remarks>
	/// 使用可能な属性 : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeConstraintAttribute" /></description></item>
	/// <item><description><see cref="SlotTypeAttribute" /></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// Class to handle a flexible enum type reference method there is more than one.
	/// </summary>
	/// <remarks>
	/// Available Attributes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeConstraintAttribute" /></description></item>
	/// <item><description><see cref="SlotTypeAttribute" /></description></item>
	/// </list>
	/// </remarks>
#endif
	[System.Serializable]
	public sealed class FlexibleEnumAny : IFlexibleField, IValueGetter<int>, IAssignFieldReceiver, IOverrideConstraint
	{
		[SerializeField] private FlexibleType _Type = FlexibleType.Constant;

		[SerializeField] private int _Value = 0;

		[SerializeField] private AnyParameterReference _Parameter = new AnyParameterReference();

		[SerializeField] private InputSlotAny _Slot = new InputSlotAny();

#if ARBOR_DOC_JA
		/// <summary>
		/// Typeを返す
		/// </summary>
#else
		/// <summary>
		/// It returns a type
		/// </summary>
#endif
		public FlexibleType type
		{
			get
			{
				return _Type;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Parameterを返す。TypeがParameter以外の場合はnull。
		/// </summary>
#else
		/// <summary>
		/// It return a Paramter. It is null if Type is other than Parameter.
		/// </summary>
#endif
		public Parameter parameter
		{
			get
			{
				if (_Type == FlexibleType.Parameter)
				{
					return _Parameter.parameter;
				}
				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を返す
		/// </summary>
#else
		/// <summary>
		/// It returns a value
		/// </summary>
#endif
		public int value
		{
			get
			{
				int value = 0;
				switch (_Type)
				{
					case FlexibleType.Constant:
						value = _Value;
						break;
					case FlexibleType.Parameter:
						Parameter parameter = _Parameter.parameter;
						if (parameter != null)
						{
							switch (parameter.type)
							{
								case Parameter.Type.Enum:
									value = parameter.enumIntValue;
									break;
								case Parameter.Type.Variable:
									if (!parameter.TryGetVariable<int>(out value))
									{
										object valueObj = parameter.GetVariable();
										if (valueObj != null)
										{
											value = (int)valueObj;
										}
									}
									break;
							}
						}
						break;
					case FlexibleType.DataSlot:
						if (!_Slot.TryGetValue<int>(out value))
						{
							object valueObject = null;
							_Slot.GetValue(ref valueObject);
							if (valueObject != null && EnumFieldUtility.IsEnum(valueObject.GetType()))
							{
								value = (int)valueObject;
							}
						}
						break;
				}

				return value;
			}
		}

		private ClassConstraintInfo _OverrideConstraint;

#if ARBOR_DOC_JA
		/// <summary>
		/// 上書きする型制約の情報
		/// </summary>
#else
		/// <summary>
		/// override ClassConstraintInfo
		/// </summary>
#endif
		public ClassConstraintInfo overrideConstraint
		{
			get
			{
				return _OverrideConstraint;
			}
			set
			{
				if (_OverrideConstraint != value)
				{
					_OverrideConstraint = value;

					SetInternalConstraint(GetConstraint());
				}
			}
		}

		private ClassConstraintInfo _FieldConstraintInfo;

#if ARBOR_DOC_JA
		/// <summary>
		/// 型制約の情報を返す。
		/// </summary>
		/// <returns>型制約の情報</returns>
#else
		/// <summary>
		/// Return information on type constraints.
		/// </summary>
		/// <returns>Type constraint information</returns>
#endif
		public ClassConstraintInfo GetConstraint()
		{
			return _OverrideConstraint ?? _FieldConstraintInfo;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleEnumAnyデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleEnumAny default constructor
		/// </summary>
#endif
		public FlexibleEnumAny()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleEnumAnyコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleEnumAny constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleEnumAny(int value)
		{
			_Type = FlexibleType.Constant;
			_Value = value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleEnumAnyコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleEnumAny constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleEnumAny(System.Enum value) : this(System.Convert.ToInt32(value))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleEnumAnyコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleEnumAny constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleEnumAny(AnyParameterReference parameter)
		{
			_Type = FlexibleType.Parameter;
			_Parameter = parameter;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleEnumAnyコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleEnumAny constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleEnumAny(InputSlotAny slot)
		{
			_Type = FlexibleType.DataSlot;
			_Slot = slot;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleEnumAnyをintにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleEnumAny</param>
		/// <returns>intにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleEnumAny to int.
		/// </summary>
		/// <param name="flexible">FlexibleEnumAny</param>
		/// <returns>Returns the result of casting to int.</returns>
#endif
		public static explicit operator int(FlexibleEnumAny flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// intをFlexibleEnumAnyにキャスト。
		/// </summary>
		/// <param name="value">int</param>
		/// <returns>FlexibleEnumAnyにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast int to FlexibleEnumAny.
		/// </summary>
		/// <param name="value">int</param>
		/// <returns>Returns the result of casting to FlexibleEnumAny.</returns>
#endif
		public static explicit operator FlexibleEnumAny(int value)
		{
			return new FlexibleEnumAny(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// System.EnumをFlexibleEnumAnyにキャスト。
		/// </summary>
		/// <param name="value">System.Enum</param>
		/// <returns>FlexibleEnumAnyにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast System.Enum to FlexibleEnumAny.
		/// </summary>
		/// <param name="value">System.Enum</param>
		/// <returns>Returns the result of casting to FlexibleEnumAny.</returns>
#endif
		public static explicit operator FlexibleEnumAny(System.Enum value)
		{
			return new FlexibleEnumAny(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// enum型の値を返す。
		/// </summary>
		/// <typeparam name="TEnum">enumの型</typeparam>
		/// <returns>enum型の値</returns>
#else
		/// <summary>
		/// Returns the enum type value.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum</typeparam>
		/// <returns>Value of enum type</returns>
#endif
		public TEnum GetEnumValue<TEnum>() where TEnum : System.Enum
		{
			return (TEnum)EnumFieldUtility.ToEnum(typeof(TEnum), value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値をobjectで返す。
		/// </summary>
		/// <returns>値のobject</returns>
#else
		/// <summary>
		/// Return the value as object.
		/// </summary>
		/// <returns>The value object</returns>
#endif
		public object GetValueObject()
		{
			return value;
		}

		internal void SetSlot(InputSlotBase slot)
		{
			_Type = FlexibleType.DataSlot;
			_Slot.Copy(slot);
		}

		int IValueGetter<int>.GetValue()
		{
			return value;
		}

		static ClassConstraintInfo CreateFieldConstraint(FieldInfo fieldInfo)
		{
			ClassTypeConstraintAttribute constraint = AttributeHelper.GetAttribute<ClassTypeConstraintAttribute>(fieldInfo);
			System.Type constraintType = constraint != null ? constraint.GetBaseType(fieldInfo) : null;
			if (EnumFieldUtility.IsEnum(constraintType))
			{
				return new ClassConstraintInfo() { constraintAttribute = constraint, constraintFieldInfo = fieldInfo };
			}

			SlotTypeAttribute slotTypeAttribute = AttributeHelper.GetAttribute<SlotTypeAttribute>(fieldInfo);
			System.Type connectableType = slotTypeAttribute != null ? slotTypeAttribute.connectableType : null;
			if (slotTypeAttribute != null && EnumFieldUtility.IsEnum(connectableType))
			{
				return new ClassConstraintInfo() { slotTypeAttribute = slotTypeAttribute };
			}

			return new ClassConstraintInfo() { constraintAttribute = new ClassEnumFieldConstraint(), constraintFieldInfo = fieldInfo };
		}

		void SetInternalConstraint(ClassConstraintInfo constraint)
		{
			if (_Parameter != null)
			{
				_Parameter.overrideConstraint = constraint;
			}
			if (_Slot != null)
			{
				_Slot.overrideConstraint = constraint;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// データスロットの接続を切断する。
		/// </summary>
#else
		/// <summary>
		/// Disconnect the data slot.
		/// </summary>
#endif
		public void Disconnect()
		{
			switch (_Type)
			{
				case FlexibleType.DataSlot:
					_Slot.Disconnect();
					break;
				case FlexibleType.Parameter:
					_Parameter.Disconnect();
					break;
			}
		}

		void IAssignFieldReceiver.OnAssignField(Object ownerObject, FieldInfo fieldInfo)
		{
			_FieldConstraintInfo = CreateFieldConstraint(fieldInfo);

			SetInternalConstraint(GetConstraint());
		}
	}
}