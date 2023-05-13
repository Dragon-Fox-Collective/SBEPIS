//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// StringListパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference StringList parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.StringList)]
	public sealed class StringListParameterReference : ParameterReference, IValueContainer<IList<string>>
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
		public IList<string> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetStringList();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetStringList(value);
				}
			}
		}

		IList<string> IValueGetter<IList<string>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<string>>.SetValue(IList<string> value)
		{
			this.value = value;
		}
	}
}