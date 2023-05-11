//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	internal static class ArborFSMCreator
	{
		[MenuItem("GameObject/Arbor/ArborFSM", false, 10)]
		static void CreateArborFSM(MenuCommand menuCommand)
		{
			NodeGraphUtility.CreateGraphObject(typeof(ArborFSM), "ArborFSM", menuCommand.context as GameObject);
		}
	}
}