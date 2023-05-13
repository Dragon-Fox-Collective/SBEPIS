//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;

namespace Arbor
{
	public sealed partial class AnyParameterReference
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FloatList型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of FloatList type.
		/// </summary>
#endif
		public IList<float> floatListValue
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetFloatList();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetFloatList(value);
				}
			}
		}
	}
}