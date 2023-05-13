//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;
namespace ArborEditor
{
	internal static class ParameterContainerInternalMenu
	{
		[MenuItem("CONTEXT/ParameterContainerInternal/Copy Component")]
		static void CopyComponent(MenuCommand command)
		{
			ParameterContainerInternal container = command.context as ParameterContainerInternal;
			if (container != null)
			{
				Clipboard.CopyParameterContainer(container);
				return;
			}

			Component component = command.context as Component;
			if (component != null)
			{
				UnityEditorInternal.ComponentUtility.CopyComponent(component);
				return;
			}

			Debug.LogError("ParameterContainerInternal : Can't Copy Component");
		}
	}
}