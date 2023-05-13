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
	/// LongListパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference LongList parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.LongList)]
	public sealed class LongListParameterReference : ParameterReference, IValueContainer<IList<long>>
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
		public IList<long> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetLongList();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetLongList(value);
				}
			}
		}

		IList<long> IValueGetter<IList<long>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<long>>.SetValue(IList<long> value)
		{
			this.value = value;
		}
	}
}