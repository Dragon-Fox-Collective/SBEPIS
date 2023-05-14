//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	internal static class ParameterContainerCreator
	{
		[MenuItem("GameObject/Arbor/ParameterContainer", false, 12)]
		static void CreateParameterContainer(MenuCommand menuCommand)
		{
			GameObject gameObject = new GameObject("ParameterContainer", typeof(ParameterContainer));
			GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			Undo.RegisterCreatedObjectUndo(gameObject, "Create ParameterContainer");
			Selection.activeGameObject = gameObject;
		}
	}
}
