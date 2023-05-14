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
	/// AssetObjectListパラメータの参照。
	/// </summary>
	/// <remarks>
	/// 使用可能な属性 : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeConstraintAttribute" /></description></item>
	/// <item><description><see cref="SlotTypeAttribute" /></description></item>
	/// </list>
	/// AssetObjectListパラメータの型はIList&lt;T&gt;として扱われているため <code>[SlotType(typeof(IList&lt;AudioClip&gt;))]</code>というように指定してください。
	/// </remarks>
#else
	/// <summary>
	/// Reference AssetObjectList parameters.
	/// </summary>
	/// <remarks>
	/// Available Attributes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeConstraintAttribute" /></description></item>
	/// <item><description><see cref="SlotTypeAttribute" /></description></item>
	/// </list>
	/// Since the type of AssetObjectList parameter is handled as IList&lt;T&gt;, specify it as <code>[SlotType(typeof(IList&lt;AudioClip&gt;))]</code>.
	/// </remarks>
#endif
	[System.Serializable]
	[Internal.Constraintable(typeof(Object), isList = true)]
	[Internal.ParameterType(Parameter.Type.AssetObjectList)]
	public sealed class AssetObjectListParameterReference : ParameterReference, IValueContainer<IList<Object>>
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
		public IList<Object> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetAssetObjectList<Object>();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetAssetObjectList(value);
				}
			}
		}

		IList<Object> IValueGetter<IList<Object>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<Object>>.SetValue(IList<Object> value)
		{
			this.value = value;
		}
	}
}
