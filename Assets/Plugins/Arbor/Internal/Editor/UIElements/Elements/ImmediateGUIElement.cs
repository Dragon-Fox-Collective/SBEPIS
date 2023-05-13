//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor.UIElements
{
	using ArborEditor.UnityEditorBridge.UIElements;

	internal abstract class ImmediateGUIElement : ImmediateModeElement
	{
		protected sealed override void ImmediateRepaint()
		{
			Color savedColor = GUI.color;
			var savedMatrix = GUI.matrix;

			GUI.color = UIElementsUtilityBridge.editorPlayModeTintColor;

			if (RenderTexture.active != null)
			{
				float scaling = 1f / EditorGUIUtility.pixelsPerPoint;
				Vector2 min = worldBound.min;
				Vector2 pos = -(min - min * scaling);
				GUI.matrix = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one * scaling);
			}

			try
			{
				OnImmediateGUI();
			}
			finally
			{
				GUI.color = savedColor;
				GUI.matrix = savedMatrix;
			}
		}

		protected abstract void OnImmediateGUI();
	}
}