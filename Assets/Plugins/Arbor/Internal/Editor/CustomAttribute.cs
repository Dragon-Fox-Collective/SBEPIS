//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace ArborEditor
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public abstract class CustomAttribute : Attribute
	{
		public Type classType
		{
			get;
			private set;
		}

		public CustomAttribute(Type classType)
		{
			this.classType = classType;
		}
	}
}