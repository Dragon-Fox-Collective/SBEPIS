//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Enumパラメータの参照。
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
	/// Reference Enum parameters.
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
	[Internal.ConstraintableEnum]
	public sealed class EnumParameterReference : ParameterReference, IValueContainer<System.Enum>, IValueContainer<int>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの値。
		/// </summary>
#else
		/// <summary>
		/// Value of the parameter
		/// </summary>
#endif
		public System.Enum value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetEnum();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetEnum(value);
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータのint値。
		/// </summary>
#else
		/// <summary>
		/// int value of the parameter
		/// </summary>
#endif
		public int intValue
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetEnumInt();
				}

				return 0;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetEnumInt(value);
				}
			}
		}

		System.Enum IValueGetter<System.Enum>.GetValue()
		{
			return value;
		}

		void IValueSetter<System.Enum>.SetValue(System.Enum value)
		{
			this.value = value;
		}

		int IValueGetter<int>.GetValue()
		{
			return intValue;
		}

		void IValueSetter<int>.SetValue(int value)
		{
			this.intValue = value;
		}
	}
}
