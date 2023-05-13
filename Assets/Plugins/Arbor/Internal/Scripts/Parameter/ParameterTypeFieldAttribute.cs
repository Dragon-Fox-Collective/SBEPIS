//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor
{
	[AttributeUsage(AttributeTargets.Field)]
	internal sealed class ParameterTypeFieldAttribute : Attribute
	{
		public readonly Type valueType;
		public readonly string menuName;

		public bool useReferenceType = false;
		public bool toList = false;

		public ParameterTypeFieldAttribute(Type valueType, string menuName)
		{
			this.valueType = valueType;
			this.menuName = menuName;
		}
	}
}