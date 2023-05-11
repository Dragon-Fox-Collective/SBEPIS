//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.BehaviourTree
{
	using Arbor.BehaviourTree;

	internal static class BehaviourTreeCreator
	{
		[MenuItem("GameObject/Arbor/BehaviourTree", false, 11)]
		static void CreateBehaviourTree(MenuCommand menuCommand)
		{
			NodeGraphUtility.CreateGraphObject(typeof(BehaviourTree), "BehaviourTree", menuCommand.context as GameObject);
		}
	}
}