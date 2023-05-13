//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using Arbor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArborEditor
{
	[CustomClipboardProcessor(typeof(Calculator))]
	public class CalculatorClipboardProcessor : ClipboardProcessor
	{
		public override void MoveBehaviour(Node node, NodeBehaviour sourceBehaviour)
		{
			CalculatorNode calculatorNode = node as CalculatorNode;
			Calculator sourceCalculator = sourceBehaviour as Calculator;
			if (calculatorNode == null || sourceCalculator == null)
			{
				return;
			}

			bool cachedEnabled = ComponentUtility.useEditorProcessor;
			ComponentUtility.useEditorProcessor = false;

			Calculator destCalculator = calculatorNode.CreateCalculator(sourceCalculator.GetType());

			if (destCalculator != null)
			{
				Clipboard.DoCopyNodeBehaviour(sourceCalculator, destCalculator, false);
			}

			ComponentUtility.useEditorProcessor = cachedEnabled;
		}
	}
}