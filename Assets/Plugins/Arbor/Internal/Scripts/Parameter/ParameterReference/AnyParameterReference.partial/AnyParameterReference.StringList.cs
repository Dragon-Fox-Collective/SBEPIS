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
		/// StringList型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of StringList type.
		/// </summary>
#endif
		public IList<string> stringListValue
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
	}
}