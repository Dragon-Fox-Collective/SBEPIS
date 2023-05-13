//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Componentパラメータの参照。
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
	/// Reference Component parameters.
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
	[Internal.Constraintable(typeof(Component))]
	public sealed class ComponentParameterReference : ParameterReference, IValueContainer<Component>
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
		public Component value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetComponent();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetComponent(value);
				}
			}
		}

		Component IValueGetter<Component>.GetValue()
		{
			return value;
		}

		void IValueSetter<Component>.SetValue(Component value)
		{
			this.value = value;
		}
	}
}
