//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	internal static class GlobalParameterContainerCreator
	{
		[MenuItem("GameObject/Arbor/GlobalParameterContainer", false, 13)]
		static void CreateGlobalParameterContainer(MenuCommand menuCommand)
		{
			GameObject gameObject = new GameObject("GlobalParameterContainer", typeof(GlobalParameterContainer));
			GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
			Undo.RegisterCreatedObjectUndo(gameObject, "Create GlobalParameterContainer");
			Selection.activeGameObject = gameObject;
		}
	}
}
