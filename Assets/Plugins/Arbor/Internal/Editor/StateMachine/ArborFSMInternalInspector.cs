//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;
	[CustomEditor(typeof(ArborFSMInternal), true)]
	internal sealed class ArborFSMInternalInspector : NodeGraphInspector
	{
	}
}
