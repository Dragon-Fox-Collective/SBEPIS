using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArborEditor.UIElements
{
	using Arbor;

	internal class CustomGUI
	{
		private System.Action<NodeGraph, Rect> _OnGUI;

		public event System.Action<bool> onChanged;

		public event System.Action<NodeGraph, Rect> onGUI
		{
			add
			{
				bool oldUnderlayGUI = _OnGUI != null;
				_OnGUI += value;
				bool newUnderlayGUI = _OnGUI != null;
				if (oldUnderlayGUI != newUnderlayGUI)
				{
					onChanged?.Invoke(newUnderlayGUI);
				}
			}
			remove
			{
				bool oldToolbarGUI = _OnGUI != null;
				_OnGUI -= value;
				bool newToolbarGUI = _OnGUI != null;
				if (oldToolbarGUI != newToolbarGUI)
				{
					onChanged?.Invoke(newToolbarGUI);
				}
			}
		}

		public bool hasGUI => _OnGUI != null;

		public void OnGUI(NodeGraph nodeGraph, Rect rect)
		{
			if (nodeGraph == null)
			{
				return;
			}

			if (_OnGUI != null)
			{
				GUI.BeginGroup(rect);

				Rect groupRect = rect;
				groupRect.position = Vector2.zero;

				GUILayout.BeginArea(groupRect);
				_OnGUI(nodeGraph, groupRect);
				GUILayout.EndArea();

				GUI.EndGroup();
			}
		}
	}
}