//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using ArborEditor.UpdateCheck;
	using ArborEditor.UIElements;

	internal sealed class GraphSettingsWindow : PopupWindowContent
	{
		private ArborEditorWindow _HostWindow;

		private LayoutArea _LayoutArea = new LayoutArea();

		public GraphSettingsWindow(ArborEditorWindow hostWindow)
		{
			_HostWindow = hostWindow;
		}

		void DoGUI()
		{
			EditorGUI.BeginChangeCheck();

			_LayoutArea.LanguagePopup(EditorContents.language, EditorStyles.popup);

#if ARBOR_TRIAL
			using (new EditorGUI.DisabledScope(true))
#endif
			{
				EditorGUI.BeginChangeCheck();
				ArborSettings.showLogo = (LogoShowMode)_LayoutArea.EnumPopup(EditorContents.showLogo, ArborSettings.showLogo);
				if (EditorGUI.EndChangeCheck())
				{
					_HostWindow.graphView.OnChangedShowLogo(true);
				}
			}

			float zoomLevel = _HostWindow.graphView.zoomLevel * 100f;
			EditorGUI.BeginChangeCheck();
			zoomLevel = _LayoutArea.Slider(EditorContents.zoom, zoomLevel, GraphView.k_ZoomMin, GraphView.k_ZoomMax) / 100f;
			if (EditorGUI.EndChangeCheck())
			{
				Vector2 center = _HostWindow.graphView.isLayoutSetup ? _HostWindow.graphView.graphViewport.center : Vector2.zero;
				_HostWindow.graphView.SetZoom(center, zoomLevel, true);
			}
			
			ArborSettings.mouseWheelMode = (MouseWheelMode)_LayoutArea.EnumPopup(EditorContents.mouseWheelMode, ArborSettings.mouseWheelMode);

			EditorGUI.BeginChangeCheck();
			ArborSettings.nodeCommentAffectsZoom = _LayoutArea.Toggle(EditorContents.nodeCommentAffectsZoom, ArborSettings.nodeCommentAffectsZoom);
			if (EditorGUI.EndChangeCheck())
			{
				_HostWindow.graphView.UpdateNodeCommentLayer();
			}

			ArborSettings.dockingOpen = _LayoutArea.Toggle(EditorContents.dockingOpen, ArborSettings.dockingOpen);

			_LayoutArea.Space();

			ArborSettings.liveTrackingHierarchy = _LayoutArea.Toggle(EditorContents.liveTrackingHierarchy, ArborSettings.liveTrackingHierarchy);

			_LayoutArea.Space();

			ArborSettings.showHierarchyIcons = _LayoutArea.Toggle(EditorContents.showHierarhcyIcons, ArborSettings.showHierarchyIcons);
			ArborSettings.openBreakNode = _LayoutArea.Toggle(EditorContents.openBreakNode, ArborSettings.openBreakNode);

			_LayoutArea.Space();

			EditorGUI.BeginChangeCheck();
			ArborSettings.showGrid = _LayoutArea.Toggle(EditorContents.showGrid, ArborSettings.showGrid);
			if (EditorGUI.EndChangeCheck())
			{
				_HostWindow.OnChangedShowGrid();
			}

			EditorGUI.BeginDisabledGroup(!ArborSettings.showGrid);

			ArborSettings.snapGrid = _LayoutArea.Toggle(EditorContents.snapGrid, ArborSettings.snapGrid);

			_LayoutArea.Space();

			ArborSettings.gridSize = _LayoutArea.Slider(EditorContents.gridSize, ArborSettings.gridSize, 1.0f, 1000.0f);
			ArborSettings.gridSplitNum = _LayoutArea.IntSlider(EditorContents.gridSplitNum, ArborSettings.gridSplitNum, 1, 100);

			EditorGUI.EndDisabledGroup();

			if (_LayoutArea.Button(EditorContents.restoreDefaultGridSettings))
			{
				GUIUtility.keyboardControl = 0;
				ArborSettings.ResetGrid();
			}

			_LayoutArea.LabelField(EditorContents.version, GUIContentCaches.Get(ArborVersion.fullVersion));

			if (!_LayoutArea.isLayout && Event.current.type == EventType.MouseDown)
			{
				GUIUtility.keyboardControl = 0;
				Event.current.Use();
			}

			if (EditorGUI.EndChangeCheck())
			{
				if (_HostWindow != null)
				{
					_HostWindow.Repaint();
				}
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
