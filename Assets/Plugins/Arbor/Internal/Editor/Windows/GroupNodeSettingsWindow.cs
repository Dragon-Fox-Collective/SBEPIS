//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	internal sealed class GroupNodeSettingsWindow : PopupWindowContent
	{
		private GroupNode _GroupNode;
		private EditorWindow _HostWindow;

		private LayoutArea _LayoutArea = new LayoutArea();

		public GroupNodeSettingsWindow(EditorWindow hostWindow, GroupNode groupNode)
		{
			_HostWindow = hostWindow;
			_GroupNode = groupNode;
		}

		void DoGUI()
		{
			EditorGUI.BeginChangeCheck();

			Color color = _LayoutArea.ColorField(GUIContentCaches.Get("Color"), _GroupNode.color, true, false, false);
			GroupNode.AutoAlignment autoAlignment = (GroupNode.AutoAlignment)_LayoutArea.EnumPopup(GUIContentCaches.Get("Auto Alignment"), _GroupNode.autoAlignment);

			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(_GroupNode.nodeGraph, "Change Color");

				_GroupNode.color = color;
				_GroupNode.autoAlignment = autoAlignment;

				EditorUtility.SetDirty(_GroupNode.nodeGraph);

				_HostWindow.Repaint();
			}

			Event current = Event.current;
			if (!_LayoutArea.isLayout && current.type == EventType.ValidateCommand && current.commandName == "UndoRedoPerformed")
			{
				HandleUtility.Repaint();
			}
		}

		public override void OnGUI(Rect rect)
		{
			_LayoutArea.Begin(rect, false);

			DoGUI();

			_LayoutArea.End();
		}

		public override Vector2 GetWindowSize()
		{
			Rect rect = new Rect(0, 0, 300, 0);

			_LayoutArea.Begin(rect, true);

			DoGUI();

			_LayoutArea.End();

			return _LayoutArea.rect.size;
		}
	}
}