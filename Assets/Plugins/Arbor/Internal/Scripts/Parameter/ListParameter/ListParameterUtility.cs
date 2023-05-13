//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;

namespace Arbor
{
	internal static class ListParameterUtility
	{
		public static ListParameterBase CreateInstance(System.Type elementType)
		{
			return new ListParameterAOT(elementType);
		}
	}
}