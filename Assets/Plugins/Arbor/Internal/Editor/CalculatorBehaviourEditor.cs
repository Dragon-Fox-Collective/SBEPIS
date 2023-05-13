//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace ArborEditor
{
	using Arbor;

	public interface ICalculatorBehaviourEditor
	{
		bool IsResizableNode();

		float GetNodeWidth();
	}

	public class CalculatorBehaviourEditor : NodeBehaviourEditor, ICalculatorBehaviourEditor
	{
		public virtual bool IsResizableNode()
		{
			return true;
		}

		public virtual float GetNodeWidth()
		{
			return Node.defaultWidth;
		}
	}
}