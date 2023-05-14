//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	[InitializeOnLoad]
	static class HierarchyExtensionsGUI
	{
		private static List<Component> s_Pool;

		static HierarchyExtensionsGUI()
		{
			ArborSettings.onChangedHierarchyExtensions += OnChangedHierarchyExtensions;

			EnableExtensionsGUI();
		}

		static void OnChangedHierarchyExtensions()
		{
			EnableExtensionsGUI();
		}

		static void EnableExtensionsGUI()
		{
			EditorApplication.hierarchyWindowItemOnGUI -= HierarchyWindowItemOnGUI;
			if (ArborSettings.showHierarchyIcons)
			{
				EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
			}

			EditorApplication.RepaintHierarchyWindow();
		}

		private static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
		{
			var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
			if (gameObject == null)
			{
				return;
			}

			if (s_Pool == null)
			{
				s_Pool = new List<Component>();
			}
			else
			{
				s_Pool.Clear();
			}

			gameObject.GetComponents(s_Pool);

			selectionRect.xMin = selectionRect.xMax - selectionRect.height;
			selectionRect.height = selectionRect.height;

			for (int i = s_Pool.Count - 1; i >= 0; i--)
			{
				var component = s_Pool[i];
				if (component == null || (component.hideFlags & HideFlags.HideInInspector) == HideFlags.HideInInspector)
				{
					continue;
				}
				var type = component.GetType();
				if (!AttributeHelper.HasAttribute<BuiltInComponent>(type))
				{
					continue;
				}

				Texture icon = AssetPreview.GetMiniThumbnail(component);

				var behaviour = component as Behaviour;

				bool enabled = behaviour != null? behaviour.enabled : true;

				var savedColor = GUI.color;
				if (!enabled)
				{
					GUI.color = new Color(1f, 1f, 1f, 0.5f);
				}

				var nodeGraph = component as NodeGraph;
				if (nodeGraph != null)
				{
					if (GUI.Button(selectionRect, icon, Defaults.iconButton))
					{
						ArborEditorWindow.Open(nodeGraph);
					}
				}
				else
				{
					GUI.DrawTexture(selectionRect, icon, ScaleMode.ScaleToFit);
				}

				GUI.color = savedColor;

				selectionRect.x -= selectionRect.width;
			}
		}

		static class Defaults
		{
			public static readonly GUIStyle iconButton = (GUIStyle)"IconButton";
		}
	}
}