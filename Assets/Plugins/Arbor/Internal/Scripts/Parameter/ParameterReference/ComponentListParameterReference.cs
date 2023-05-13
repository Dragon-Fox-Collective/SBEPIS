//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// ComponentListパラメータの参照。
	/// </summary>
	/// <remarks>
	/// 使用可能な属性 : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeConstraintAttribute" /></description></item>
	/// <item><description><see cref="SlotTypeAttribute" /></description></item>
	/// </list>
	/// ComponentListパラメータの型はIList&lt;T&gt;として扱われているため <code>[SlotType(typeof(IList&lt;Rigidbody&gt;))]</code>というように指定してください。
	/// </remarks>
#else
	/// <summary>
	/// Reference ComponentList parameters.
	/// </summary>
	/// <remarks>
	/// Available Attributes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeConstraintAttribute" /></description></item>
	/// <item><description><see cref="SlotTypeAttribute" /></description></item>
	/// </list>
	/// Since the type of ComponentList parameter is handled as IList&lt;T&gt;, specify it as <code>[SlotType(typeof(IList&lt;Rigidbody&gt;))]</code>.
	/// </remarks>
#endif
	[System.Serializable]
	[Internal.Constraintable(typeof(Component), isList = true)]
	public sealed class ComponentListParameterReference : ParameterReference, IValueContainer<IList<Component>>
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
		public IList<Component> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetComponentList<Component>();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetComponentList(value);
				}
			}
		}

		IList<Component> IValueGetter<IList<Component>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<Component>>.SetValue(IList<Component> value)
		{
			this.value = value;
		}
	}
}
