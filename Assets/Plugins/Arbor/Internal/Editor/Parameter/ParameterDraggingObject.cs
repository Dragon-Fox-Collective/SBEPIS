//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace ArborEditor
{
	using Arbor;

	public sealed class ParameterDraggingObject : ScriptableSingleton<ParameterDraggingObject>
	{
		public Parameter parameter
		{
			get;
			internal set;
		}
	}
}