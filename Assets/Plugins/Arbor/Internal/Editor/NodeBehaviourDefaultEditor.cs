//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if !ARBOR_DISABLE_DEFAULT_EDITOR
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	[CustomEditor(typeof(NodeBehaviour), true)]
	internal sealed class NodeBehaviourDefaultEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
		}
	}
}
#endif