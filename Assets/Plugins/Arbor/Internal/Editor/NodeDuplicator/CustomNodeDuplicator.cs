//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace ArborEditor
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class CustomNodeDuplicator : CustomAttribute
	{
		public readonly Type graphType;

		public CustomNodeDuplicator(Type classType, Type graphType) : base(classType)
		{
			this.graphType = graphType;
		}

		public CustomNodeDuplicator(Type classType) : this(classType, typeof(Arbor.NodeGraph))
		{
		}
	}
}