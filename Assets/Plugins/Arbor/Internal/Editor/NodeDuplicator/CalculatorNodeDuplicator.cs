//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	[CustomNodeDuplicator(typeof(CalculatorNode))]
	internal sealed class CalculatorNodeDuplicator : NodeDuplicator
	{
		protected override Node OnDuplicate()
		{
			CalculatorNode sourceCalculator = sourceNode as CalculatorNode;

			Calculator calculator = sourceCalculator.calculator;
			if (calculator == null)
			{
				return null;
			}

			if (isClip)
			{
				return targetGraph.CreateCalculator(sourceCalculator.nodeID, calculator.GetType());
			}
			else
			{
				return targetGraph.CreateCalculator(calculator.GetType());
			}
		}

		protected override void OnAfterDuplicate(List<NodeDuplicator> duplicators)
		{
			CalculatorNode sourceCalculator = sourceNode as CalculatorNode;
			CalculatorNode calculator = destNode as CalculatorNode;

			Clipboard.CopyNodeBehaviour(sourceCalculator.calculator, calculator.calculator, true);

			RegisterBehaviour(sourceCalculator.calculator, calculator.calculator);
		}
	}
}