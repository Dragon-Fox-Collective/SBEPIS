//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace ArborEditor
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class BuiltinPath : Attribute
	{
		public Type type;
		public string path;

		public BuiltinPath(Type type, string path)
		{
			this.type = type;
			this.path = path;
		}
	}
}