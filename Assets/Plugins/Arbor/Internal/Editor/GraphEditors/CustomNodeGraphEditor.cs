﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace ArborEditor
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class CustomNodeGraphEditor : CustomAttribute
	{
		public CustomNodeGraphEditor(Type classType) : base(classType)
		{
		}
	}
}