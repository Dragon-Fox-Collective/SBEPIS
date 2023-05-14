//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace ArborEditor
{
	using Arbor;
	using Arbor.DynamicReflection;
	using Arbor.Playables;
	using ArborEditor.UpdateCheck;
	using ArborEditor.IMGUI.Controls;
	using ArborEditor.UIElements;
	using ArborEditor.UnityEditorBridge;
	using ArborEditor.UnityEditorBridge.Extensions;
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	[DefaultExecutionOrder(int.MaxValue)]
	public sealed class ArborEditorWindow : EditorWindow, IHasCustomMenu, IPropertyChanged, IUpdateCallback, IHierarchyChangedCallback
	{
		#region static
		private static GUIContent s_DefaultTitleContent = null;

		private static System.Action<NodeGraph> s_ToolbarGUI = null;
		private static event System.Action<bool> onChangedCustomToolbar;

		public static event System.Action<NodeGraph> toolbarGUI
		{
			add
			{
				bool oldToolbarGUI = s_ToolbarGUI != null;
				s_ToolbarGUI += value;
				bool newToolbarGUI = s_ToolbarGUI != null;
				if (oldToolbarGUI != newToolbarGUI)
				{
					onChangedCustomToolbar?.Invoke(newToolbarGUI);
				}
			}
			remove
			{
				bool oldToolbarGUI = s_ToolbarGUI != null;
				s_ToolbarGUI -= value;
				bool newToolbarGUI = s_ToolbarGUI != null;
				if (oldToolbarGUI != newToolbarGUI)
				{
					onChangedCustomToolbar?.Invoke(newToolbarGUI);
				}
			}
		}

		public static event System.Action<NodeGraph, Rect> underlayGUI
		{
			add
			{
				GraphView.underlayGUI += value;
			}
			remove
			{
				GraphView.underlayGUI -= value;
			}
		}

		public static event System.Action<NodeGraph, Rect> overlayGUI
		{
			add
			{
				GraphView.overlayGUI += value;
			}
			remove
			{
				GraphView.overlayGUI -= value;
			}
		}

		[System.Obsolete]
		public static ISkin skin;

		public static ArborEditorWindow activeWindow
		{
			get;
			private set;
		}

		public static ArborEditorWindow actualWindow
		{
			get
			{
				return EditorWindowBridge.actualWindow as ArborEditorWindow;
			}
		}

		private static GUIContent defaultTitleContent
		{
			get
			{
				if (s_DefaultTitleContent == null)
				{
					s_DefaultTitleContent = new GUIContent("Arbor Editor", EditorGUIUtility.isProSkin ? Icons.logoIcon_DarkSkin : Icons.logoIcon_LightSkin);
				}
				return s_DefaultTitleContent;
			}
		}

		public static bool isBuildDocuments
		{
			get;
			set;
		}

		public static bool isInArborEditor
		{
			get
			{
				return isBuildDocuments || actualWindow != null;
			}
		}

		static ArborEditorWindow()
		{
			NodeGraph.onBreakNode += OnBreakNode;
		}

		static void OnBreakNode(Node node)
		{
			if (!ArborSettings.openBreakNode)
			{
				return;
			}

			{
				var n = node;

				EditorApplication.delayCall += () =>
				{
					var nodeGraph = n.nodeGraph;

					ArborEditorWindow window = Open();
					window.OpenInternal(nodeGraph);

					window.graphEditor.BeginFrameSelected(n, false);
				};
			}
		}

		static ArborEditorWindow Open()
		{
			ArborEditorWindow window = ArborSettings.dockingOpen ? EditorWindow.GetWindow<ArborEditorWindow>(typeof(SceneView)) : EditorWindow.GetWindow<ArborEditorWindow>();
			window.titleContent = defaultTitleContent;
			return window;
		}

		[MenuItem("Window/Arbor/Arbor Editor")]
		public static void OpenFromMenu()
		{
			Open();
		}

		public static void Open(NodeGraph nodeGraph)
		{
			ArborEditorWindow window = Open();
			window.OpenInternal(nodeGraph);
		}

		#endregion // static

		#region Serialize fields
		[SerializeField]
		private NodeGraph _NodeGraphRoot = null;

		[SerializeField]
		private int _NodeGraphRootInstanceID = 0;

		[SerializeField]
		private NodeGraph _NodeGraphRootPrev = null;

		[SerializeField]
		private int _NodeGraphRootPrevInstanceID = 0;

		[SerializeField]
		private NodeGraph _NodeGraphCurrent = null;

		[SerializeField]
		private int _NodeGraphCurrentInstanceID = 0;

		[SerializeField]
		private bool _IsLocked = false;

		[SerializeField]
		private TreeViewState _TreeViewState = new TreeViewState();

		#endregion // Serialize fields

		#region fields

		private NodeGraphEditor _GraphEditor = null;

		private GraphTreeViewItem _SelectedGraphItem = null;

		private bool _IsRepaint = false;

		private bool _IsUpdateLiveTracking = false;

		private bool _IsWindowVisible = false;

		private GraphMainLayout _MainLayout;
		private ResizableElement _MinimapElement;
		private MinimapView _MinimapView;
		private VisualElement _GraphPanel;
		private GraphView _GraphView;
		private Toolbar _Toolbar;
		private Toggle _SidePanelToggle;
		private ObjectField _ToolbarObjectField;
		private VisualElement _ToolbarGraphEditor;
		private VisualElement _CustomToolbarGUI;
		private ToolbarToggle _ToolbarLiveTracking;
		private Button _ToolbarNotificationButton;
		private VisualElement _NoGraphUI;
		private VisualElement _Breadcrumbs;
		private VisualElement _GraphTabElement;
		private Button _GraphTabHeaderElement;
		private TreeViewElement _GraphTreeElement;

		internal TabPanel<SidePanelTab> sidePanel
		{
			get;
			private set;
		}

		private GraphSettingsWindow _GraphSettingsWindow = null;

		private TreeView _TreeView = new TreeView();

		#endregion // fields

		#region properties

		public NodeGraphEditor graphEditor
		{
			get
			{
				return _GraphEditor;
			}
		}

		internal GraphView graphView
		{
			get
			{
				return _GraphView;
			}
		}

		internal MinimapView minimapView
		{
			get
			{
				return _MinimapView;
			}
		}

		#endregion // properties

		#region Unity methods

		void OnEnable()
		{
			titleContent = defaultTitleContent;

			// Conflict between window resizing drag and lock button control ID.
			// Secure a control ID for the lock button in advance to avoid conflicts.
			// Via SearchField to get the controlID with GUIUtility.GetPermanentControlID().
			var searchField = new UnityEditor.IMGUI.Controls.SearchField();
			_LockButtonControlID = searchField.searchFieldControlID;

			if (activeWindow == null)
			{
				activeWindow = this;
			}

			if (_GraphEditor != null)
			{
				_GraphEditor.hostWindow = this;
			}

			SetupElements();

			if (_NodeGraphRoot != null)
			{
				RegisterRootGraphCallback();
			}

			if (_NodeGraphCurrent != null)
			{
				RegisterCurrentGraphCallback();
			}

			if (_NodeGraphRoot != null)
			{
				BuildTree();
			}

			_GraphView.OnChangedShowLogo();
			
			if (_GraphEditor != null)
			{
				_GraphEditor.ValidateNodes();
				_GraphEditor.SetupElements();
				_GraphEditor.Update();
			}

			DoRepaint();

			EditorCallbackUtility.RegisterUpdateCallback(this);
			EditorCallbackUtility.RegisterPropertyChanged(this);
			EditorCallbackUtility.RegisterHierarchyChangedCallback(this);

			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
			EditorSceneManager.sceneLoaded += OnSceneLoaded;
			EditorSceneManager.sceneClosed += OnSceneClosed;

			onChangedCustomToolbar += EnableCustomToolbar;

			if (ArborUpdateCheck.instance.isDone)
			{
				SetupNotificationButton();
			}
			else
			{
				ArborUpdateCheck.instance.onDone += SetupNotificationButton;
			}

#if ARBOR_DEBUG
			ArborUpdateCheck updateCheck = ArborUpdateCheck.instance;
			updateCheck.CheckStart(true);
#else
			if (ArborVersion.isUpdateCheck)
			{
				ArborUpdateCheck updateCheck = ArborUpdateCheck.instance;
				updateCheck.CheckStart();
			}
#endif
		}

		void SelectGraphTreeItem(GraphTreeViewItem graphItem)
		{
			if (graphItem == null)
			{
				return;
			}

			if (graphItem != null && !_TreeViewState.IsSelected(graphItem))
			{
				List<int> expandedIDs = _TreeViewState.currentExpandedIDs;
				var parent = graphItem.parent;
				while (parent != null)
				{
					bool expanded = expandedIDs.BinarySearch(parent.id) >= 0;
					if (!expanded)
					{
						expandedIDs.Add(parent.id);
						expandedIDs.Sort();
					}
					parent = parent.parent;
				}

				_GraphTreeElement.UpdateViewTree();
				_GraphTreeElement.SetSelectedItem(graphItem, true);
			}
		}

		void OnSubmitItem(TreeViewItem item)
		{
			var graphItem = item as GraphTreeViewItem;
			if (graphItem == null)
			{
				return;
			}

			ChangeCurrentNodeGraph(graphItem);
		}

		void OnRenameEndedItem(string name, int id)
		{
			GraphTreeViewItem valueItem = _TreeView.FindItem(id) as GraphTreeViewItem;
			NodeGraph nodeGraph = valueItem?.nodeGraph;
			if (nodeGraph != null)
			{
				SetGraphName(nodeGraph, name);
			}

			_GraphTreeElement.ListViewRefresh();
		}

		void SetGraphName(NodeGraph nodeGraph, string graphName)
		{
			Undo.RecordObject(nodeGraph, "Change Graph Name");

			nodeGraph.graphName = graphName;

			EditorUtility.SetDirty(nodeGraph);
		}

		private bool _ChangingPlayMode = false;

		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (_ChangingPlayMode)
			{
				// Repair the selected graph if the scene is loaded while switching play modes
				// If called in EnteredPlayMode, the display will be disrupted for a moment between loading the scene and entering play mode, so it must be called here.
				// In multi-scene edit mode, do not clear the instanceID if it cannot be repaired because the scene may not have been loaded yet.
				RepairNodeGraphReferences(true);
			}
		}

		void OnSceneClosed(Scene scene)
		{
			if (!_ChangingPlayMode)
			{
				ReatachIfNecessary();
			}
		}

		void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			switch (state)
			{
				case PlayModeStateChange.ExitingPlayMode:
					_ChangingPlayMode = true;
					if (_NodeGraphRoot != null)
					{
						UnregisterRootGraphCallback();
					}
					if (_NodeGraphCurrent != null)
					{
						UnregisterCurrentGraphCallback();
					}
					break;
				case PlayModeStateChange.EnteredPlayMode:
					{
						_ChangingPlayMode = false;

						// Called in case OnSceneLoaded could not repair.
						RepairNodeGraphReferences();
						if (_GraphEditor != null)
						{
							_IsUpdateLiveTracking = true;
						}

						SetupToolbarObjectField();
					}
					break;
				case PlayModeStateChange.ExitingEditMode:
					{
						_ChangingPlayMode = true;
					}
					break;
				case PlayModeStateChange.EnteredEditMode:
					_ChangingPlayMode = false;

					// Repair the graph that was selected during play when the play ends.
					// * Repair if the edit scene is open at the end of play and the graph saved in the scene is displayed.
					// * Clear the reference if the scene at the end of play is different from the edit scene, or if it shows a graph that has not been saved in the scene.
					RepairNodeGraphReferences();

					if (_NodeGraphRoot != null)
					{
						RegisterRootGraphCallback();
					}
					if (_NodeGraphCurrent != null)
					{
						RegisterCurrentGraphCallback();
					}
					if (_GraphEditor != null)
					{
						_GraphEditor.RaiseOnChangedNodes();
					}

					SetupToolbarObjectField();
					break;
			}
		}

		private void OnDisable()
		{
			if (activeWindow == this)
			{
				activeWindow = null;
			}

			if (_NodeGraphRoot != null)
			{
				UnregisterRootGraphCallback();
			}

			if (_NodeGraphCurrent != null)
			{
				UnregisterCurrentGraphCallback();
			}

			if (!_IsWindowVisible)
			{
				DestroyGraphEditor();
			}

			EditorCallbackUtility.UnregisterUpdateCallback(this);
			EditorCallbackUtility.UnregisterPropertyChanged(this);
			EditorCallbackUtility.UnregisterHierarchyChangedCallback(this);

			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			EditorSceneManager.sceneLoaded -= OnSceneLoaded;
			EditorSceneManager.sceneClosed -= OnSceneClosed;

			onChangedCustomToolbar -= EnableCustomToolbar;

			ArborUpdateCheck.instance.onDone -= SetupNotificationButton;

			_GraphView.OnDestroy();
		}

		private void OnSelectionChange()
		{
			if (_IsLocked)
			{
				return;
			}

			GameObject gameObject = Selection.activeGameObject;
			if (gameObject == null)
			{
				return;
			}

			NodeGraph[] nodeGraphs = gameObject.GetComponents<NodeGraph>();
			if (nodeGraphs != null)
			{
				int graphCount = nodeGraphs.Length;
				for (int graphIndex = 0; graphIndex < graphCount; graphIndex++)
				{
					NodeGraph graph = nodeGraphs[graphIndex];
					if ((graph.hideFlags & HideFlags.HideInInspector) == HideFlags.None)
					{
						OpenInternal(graph);
						break;
					}
				}
			}
		}

		void IPropertyChanged.OnPropertyChanged(PropertyChangedType propertyChangedType)
		{
			if (propertyChangedType != PropertyChangedType.UndoRedoPerformed)
			{
				return;
			}

			// Repair the reference when the graph is restored by Undo/Redo the deletion of the selected graph.
			RepairNodeGraphReferences();

			SetupToolbarObjectField();

			bool rebuitGraphEditor = false;

			if (ReselectIfNecessary())
			{
				rebuitGraphEditor = true;
			}
			else
			{
				RegisterRootGraphCallback();
				RegisterCurrentGraphCallback();

				if (_NodeGraphRoot != null)
				{
					if (_SelectedGraphItem != null && _SelectedGraphItem.nodeGraph != _NodeGraphCurrent)
					{
						// Undo/Redo Graph Selection
						// Set null to search again from _NodeGraphCurrent in BuildTree().
						_SelectedGraphItem = null;
					}

					BuildTree();

					if (_GraphEditor != null && _GraphEditor.nodeGraph != _NodeGraphCurrent)
					{
						RebuildGraphEditor();
						rebuitGraphEditor = true;
					}
				}
			}

			if (!rebuitGraphEditor && _GraphEditor != null)
			{
				_GraphEditor.OnUndoRedoPerformed();
			}

			ShowGraphView(_GraphEditor != null);

			DoRepaint();
		}

		private Stack<TreeViewItem> _TreeStackCache = null;

		bool IsDirtyGraphTree()
		{
			var root = _TreeView?.root;
			if (root == null)
			{
				return false;
			}

			if (_TreeStackCache == null)
			{
				_TreeStackCache = new Stack<TreeViewItem>();
			}

			try
			{
				_TreeStackCache.Push(root);

				while (_TreeStackCache.Count > 0)
				{
					var item = _TreeStackCache.Pop();

					if (item is GraphTreeViewItem graphTreeViewItem &&
						graphTreeViewItem.nodeGraph == null)
					{
						return true;
					}

					foreach (var child in item.children)
					{
						_TreeStackCache.Push(child);
					}
				}

				return false;
			}
			finally
			{
				_TreeStackCache.Clear();
			}
		}

		void IHierarchyChangedCallback.OnHierarchyChanged()
		{
			if (!ReatachIfNecessary() && IsDirtyGraphTree())
			{
				BuildTree();
			}
		}

		void IUpdateCallback.OnUpdate()
		{
			if (_GraphEditor != null)
			{
				_GraphEditor.RebuildIfNecessary();
			}

			if (_IsWindowVisible)
			{
				if (_GraphView != null && _GraphView.panel != null)
				{
					_GraphView.Update();
				}

				if (_GraphEditor != null && _GraphEditor.nodeGraph != null)
				{
					if (_IsUpdateLiveTracking)
					{
						if (_GraphEditor.LiveTracking())
						{
							// Change GraphEditor
							_IsUpdateLiveTracking = true;
						}
						else
						{
							_IsUpdateLiveTracking = false;
						}
					}

					if (_GraphView.isLayoutSetup)
					{
						UpdateScrollbar();
						_GraphEditor.Update();
						_GraphEditor.UpdateVisibleNodes();
					}

					if (_IsRepaint)
					{
						DoRepaint();
					}
				}
			}
		}

		void OnBecameVisible()
		{
			_IsWindowVisible = true;
		}

		void OnBecameInvisible()
		{
			_IsWindowVisible = false;
		}

		private int _LockButtonControlID;

		private void ShowButton(Rect r)
		{
			bool flag = GUI.Toggle(r, _LockButtonControlID, _IsLocked, GUIContent.none, BuiltInStyles.lockButton);
			if (flag == _IsLocked)
			{
				return;
			}
			_IsLocked = flag;
		}

		private void OnLostFocus()
		{
			var renameOverlayElement = _CurrentFocusedElement as RenameOverlayElement;
			renameOverlayElement?.EndRename(true);
		}

		void OnDestroy()
		{
			ClearSelectGraph();

			_GraphView.OnDestroy();
		}

		#endregion // Unity methods

		void RepairNodeGraphReferences(bool repairOnly = false)
		{
			bool repaired = false;
			if (_NodeGraphRootPrev == null && _NodeGraphRootPrevInstanceID != 0)
			{
				_NodeGraphRootPrev = EditorUtility.InstanceIDToObject(_NodeGraphRootPrevInstanceID) as NodeGraph;
				if (!repairOnly)
				{
					if (_NodeGraphRootPrev == null)
					{
						_NodeGraphRootPrevInstanceID = 0;
					}
					ShowGraphTabHeader(_NodeGraphRootPrev != null);
					SetupToolbarObjectField();
				}
				repaired = true;
			}
			if (_NodeGraphRoot == null && _NodeGraphRootInstanceID != 0)
			{
				_NodeGraphRoot = EditorUtility.InstanceIDToObject(_NodeGraphRootInstanceID) as NodeGraph;
				if (_NodeGraphRoot != null)
				{
					RegisterRootGraphCallback();
				}

				if (!repairOnly)
				{
					if (_NodeGraphRoot == null)
					{
						_NodeGraphRootInstanceID = 0;
					}
					ShowGraphTab(_NodeGraphRoot != null);
					if (_NodeGraphRoot != null)
					{
						_GraphTabHeaderElement.text = _NodeGraphRoot.graphName;
					}
					SetupToolbarObjectField();
				}
				repaired = true;
			}
			if (_NodeGraphCurrent == null && _NodeGraphCurrentInstanceID != 0)
			{
				_NodeGraphCurrent = EditorUtility.InstanceIDToObject(_NodeGraphCurrentInstanceID) as NodeGraph;
				if (_NodeGraphCurrent != null)
				{
					RegisterCurrentGraphCallback();
				}

				if (!repairOnly)
				{
					if (_NodeGraphCurrent == null)
					{
						_NodeGraphCurrentInstanceID = 0;
					}
				}
				repaired = true;
			}

			if (repaired)
			{
				BuildTree();
			}

			if (_GraphEditor != null)
			{
				if (_GraphEditor.RepairReferences())
				{
					if (_GraphEditor.nodeGraph != null)
					{
						_GraphEditor.RebuildIfNecessary();
						_GraphEditor.Update();
					}
				}
			}
		}

		private bool _InPostLayout = false;

		void OnPostLayout()
		{
			ApplyStoredTransform(_SelectedGraphItem);

			if (!_InPostLayout)
			{
				_InPostLayout = true;

				try
				{
					if (_GraphEditor != null && _GraphEditor.nodeGraph != null)
					{
						UpdateScrollbar();
						_GraphEditor.Update();
					}
				}
				finally
				{
					_InPostLayout = false;
				}
			}
		}

		void DestroyGraphEditor()
		{
			if (_GraphEditor == null)
			{
				return;
			}

			Object.DestroyImmediate(_GraphEditor);
			_GraphEditor = null;

			if (_GraphView != null)
			{
				_GraphView.graphEditor = null;
			}

			SetupToolbarGraphEditor();

			ShowGraphView(false);
		}

		void ShowGraphView(bool show)
		{
			if (show)
			{
				if (_NoGraphUI.parent != null)
				{
					_NoGraphUI.RemoveFromHierarchy();
				}
				if (_GraphView.parent == null)
				{
					_GraphPanel.Add(_GraphView);
					_MainLayout.leftPanel.Add(_MinimapElement);
				}
				sidePanel.contentContainer.visible = true;
			}
			else
			{
				if (_GraphView.parent != null)
				{
					_GraphView.RemoveFromHierarchy();
					_MinimapElement.RemoveFromHierarchy();
				}
				if (_NoGraphUI.parent == null)
				{
					_GraphPanel.Add(_NoGraphUI);
				}

				sidePanel.contentContainer.visible = false;
			}
		}

		void InternalSelectRootGraph(NodeGraph rootGraph, bool isExternal)
		{
			int undoGroup = Undo.GetCurrentGroup();

			Undo.RecordObject(this, "Select NodeGraph");

			if (isExternal)
			{
				if (_NodeGraphRootPrev == null)
				{
					_NodeGraphRootPrev = _NodeGraphRoot;
					_NodeGraphRootPrevInstanceID = _NodeGraphRootPrev.GetInstanceID();
				}
			}
			else
			{
				_NodeGraphRootPrev = null;
				_NodeGraphRootPrevInstanceID = 0;
			}

			ShowGraphTabHeader(_NodeGraphRootPrev != null);

			SetupToolbarObjectField();

			SetNodeGraphRoot(rootGraph);

			var rootItem = FindTreeViewItem(rootGraph) as GraphTreeViewItem;
			SetNodeGraphCurrent(rootItem);

			Undo.CollapseUndoOperations(undoGroup);

			EditorUtility.SetDirty(this);

			RebuildGraphEditor();

			_GraphView.OnChangedShowLogo(true);

			DoRepaint();
		}

		void OpenInternal(NodeGraph nodeGraph)
		{
			NodeGraph rootGraph = nodeGraph.rootGraph;
			SelectRootGraph(rootGraph);
			if (rootGraph != nodeGraph)
			{
				var ownerBehaviour = nodeGraph.ownerBehaviourObject;
				int instanceID = ownerBehaviour != null ? ownerBehaviour.GetInstanceID() : nodeGraph.GetInstanceID();
				ChangeCurrentNodeGraph(instanceID);
			}
		}

		public void SelectRootGraph(NodeGraph nodeGraph)
		{
			if (_NodeGraphRootPrev == null && _NodeGraphRoot == nodeGraph && _NodeGraphCurrent == nodeGraph)
			{
				return;
			}

			InternalSelectRootGraph(nodeGraph, false);
		}

		public void SelectExternalGraph(GraphTreeViewItem graphItem)
		{
			NodeGraph rootGraph = graphItem.nodeGraph.rootGraph;

			InternalSelectRootGraph(rootGraph, true);
		}

		internal void OnChangedShowGrid()
		{
			_GraphView.SetShowGridBackground(ArborSettings.showGrid);
		}

		void RebuildGraphEditor()
		{
			NodeGraph nodeGraphCurrent = _SelectedGraphItem?.nodeGraph;

			DestroyGraphEditor();

			bool nextHasGraphEditor = nodeGraphCurrent != null;

			if (!nextHasGraphEditor)
			{
				return;
			}

			_GraphEditor = NodeGraphEditor.CreateEditor(this, nodeGraphCurrent, _SelectedGraphItem.isExternal);
			
			if (_GraphView != null)
			{
				_GraphView.graphEditor = _GraphEditor;
			}

			SetupToolbarGraphEditor();

			ShowGraphView(true);

			_IsRepaint = true;

			_GraphEditor.Update();

			_GraphEditor.DirtyGraphExtents();
		}

		public void ChangeCurrentNodeGraph(int instanceId, bool liveTracking = false)
		{
			var graphItem = _TreeView.FindItem(instanceId) as GraphTreeViewItem;
			ChangeCurrentNodeGraph(graphItem, liveTracking);
		}

		private void ChangeCurrentNodeGraph(GraphTreeViewItem graphItem, bool liveTracking = false)
		{
			if (graphItem == null)
			{
				return;
			}

			if (_SelectedGraphItem == graphItem)
			{
				return;
			}

			if (!liveTracking && Application.isPlaying &&
				ArborSettings.liveTracking && ArborSettings.liveTrackingHierarchy &&
				_GraphEditor != null && _GraphEditor.GetPlayState() != PlayState.Stopping)
			{
				ArborSettings.liveTracking = false;
				_ToolbarLiveTracking?.SetValueWithoutNotify(ArborSettings.liveTracking);
			}

			int undoGroup = Undo.GetCurrentGroup();

			Undo.RecordObject(this, "Select NodeGraph");

			SetNodeGraphCurrent(graphItem);

			Undo.CollapseUndoOperations(undoGroup);

			EditorUtility.SetDirty(this);

			RebuildGraphEditor();

			_GraphView.OnChangedShowLogo(true);

			DoRepaint();
		}

		internal void DoRepaint()
		{
			_GraphView.contentContainer.MarkDirtyRepaint();

			Repaint();
			_IsRepaint = false;
		}

		internal void OpenCreateMenu(Rect buttonRect)
		{
			buttonRect = GUIUtility.GUIToScreenRect(buttonRect);
			GraphMenuWindow.instance.Init(buttonRect, CreateGraphByType);
		}

		void CreateGraphByType(System.Type type)
		{
			int undoGroup = Undo.GetCurrentGroup();

			NodeGraph nodeGraph = NodeGraphUtility.CreateGraphObject(type, type.Name, null);

			SelectRootGraph(nodeGraph);

			Undo.CollapseUndoOperations(undoGroup);
		}

		void SetHelpMenu(GenericMenu menu)
		{
			menu.AddItem(EditorContents.assetStore, false, () =>
			{
				ArborVersion.OpenAssetStore();
			});
			menu.AddSeparator("");
			menu.AddItem(EditorContents.officialSite, false, () =>
			{
				Help.BrowseURL(Localization.GetWord("SiteURL"));
			});
			menu.AddItem(EditorContents.manual, false, () =>
			{
				Help.BrowseURL(Localization.GetWord("ManualURL"));
			});
			menu.AddItem(EditorContents.inspectorReference, false, () =>
			{
				Help.BrowseURL(Localization.GetWord("InspectorReferenceURL"));
			});
			menu.AddItem(EditorContents.scriptReference, false, () =>
			{
				Help.BrowseURL(Localization.GetWord("ScriptReferenceURL"));
			});
			menu.AddSeparator("");
			menu.AddItem(EditorContents.releaseNotes, false, () =>
			{
				Help.BrowseURL(Localization.GetWord("ReleaseNotesURL"));
			});
			menu.AddItem(EditorContents.forum, false, () =>
			{
				Help.BrowseURL(Localization.GetWord("ForumURL"));
			});
		}

		void OnDestroyRootGraph(NodeGraph nodeGraph)
		{
			if (ReferenceEquals(_NodeGraphRoot, nodeGraph) && !ReferenceEquals(_NodeGraphRoot, _NodeGraphCurrent))
			{
				OnDestroyNodeGraph(nodeGraph);
			}
		}

		void ClearSelectGraph()
		{
			SetNodeGraphCurrent(null);
			SetNodeGraphRoot(null);

			DestroyGraphEditor();
		}

		void OnDestroyNodeGraph(NodeGraph nodeGraph)
		{
			if (EditorApplication.isPlaying != EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}

			ReselectIfNecessary(nodeGraph);
		}

		public void OnChangedGraphTree()
		{
			BuildTree();

			if (_GraphEditor != null && _SelectedGraphItem != null && _GraphEditor.nodeGraph != _SelectedGraphItem.nodeGraph)
			{
				RebuildGraphEditor();
			}
		}

		static void AddGraphItem(TreeViewItem parent, NodeGraph nodeGraph)
		{
			if (nodeGraph == null)
			{
				return;
			}

			int nodeCount = nodeGraph.nodeCount;
			for (int nodeIndex = 0; nodeIndex < nodeCount; nodeIndex++)
			{
				INodeBehaviourContainer behaviours = nodeGraph.GetNodeFromIndex(nodeIndex) as INodeBehaviourContainer;
				if (behaviours == null)
				{
					continue;
				}

				int behaviourCount = behaviours.GetNodeBehaviourCount();
				for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
				{
					NodeBehaviour behaviour = behaviours.GetNodeBehaviour<NodeBehaviour>(behaviourIndex);
					ISubGraphBehaviour subGraphBehaviour = behaviour as ISubGraphBehaviour;
					if (subGraphBehaviour != null)
					{
						var referenceGraph = subGraphBehaviour.GetSubGraph();
						// Unity bug: Undo of object deletion does not return reference
						// https://forum.unity.com/threads/monobehaviour-references-are-lost-on-undo.587011/
						// It is repaired when reloading is performed by compiling, starting play, etc.
						if (referenceGraph != null)
						{
							var newItem = new SubGraphTreeViewItem(behaviour.GetInstanceID(), subGraphBehaviour);
							parent.AddChild(newItem);

							if (!subGraphBehaviour.isExternal)
							{
								AddGraphItem(newItem, referenceGraph);
							}
						}
					}
				}
			}
		}

		void BuildTree()
		{
			int currentId = _SelectedGraphItem != null ? _SelectedGraphItem.id : 0;
			_TreeView.ClearTree();

			if (_NodeGraphRoot != null)
			{
				var item = new GraphTreeViewItem(_NodeGraphRoot);
				_TreeView.root.AddChild(item);

				AddGraphItem(item, _NodeGraphRoot);
			}

			_TreeView.SetupDepths();

			_GraphTreeElement.UpdateViewTree();

			GraphTreeViewItem nextSelectedGraphItem = null;

			if (currentId != 0)
			{
				nextSelectedGraphItem = _TreeView.FindItem(currentId) as GraphTreeViewItem;
			}
			if (nextSelectedGraphItem == null && _NodeGraphCurrent != null)
			{
				nextSelectedGraphItem = FindTreeViewItem(_NodeGraphCurrent) as GraphTreeViewItem;
			}

			SetNodeGraphCurrent(nextSelectedGraphItem);

			Repaint();
		}

		TreeViewItem FindTreeViewItem(NodeGraph nodeGraph)
		{
			if (nodeGraph == null)
			{
				return null;
			}

			return _TreeView.FindItem((item) =>
			{
				GraphTreeViewItem graphItem = item as GraphTreeViewItem;
				return graphItem != null && graphItem.nodeGraph == nodeGraph;
			});
		}

		void RegisterRootGraphCallback()
		{
			UnregisterRootGraphCallback();

			_NodeGraphRoot.destroyCallback += OnDestroyRootGraph;
			NodeGraphCallback.RegisterStateChangedCallback(_NodeGraphRoot, OnStateChanged);
			NodeGraphCallback.RegisterChangedGraphTreeCallback(_NodeGraphRoot, OnChangedGraphTree);
			_NodeGraphRoot.onChangedGraphName += OnChangedGraphName;
		}

		void OnChangedGraphName()
		{
			_GraphTabHeaderElement.text = _NodeGraphRoot.graphName;
		}

		void UnregisterRootGraphCallback()
		{
			_NodeGraphRoot.destroyCallback -= OnDestroyRootGraph;
			NodeGraphCallback.UnregisterStateChangedCallback(_NodeGraphRoot, OnStateChanged);
			NodeGraphCallback.UnregisterChangedGraphTreeCallback(_NodeGraphRoot, OnChangedGraphTree);
			_NodeGraphRoot.onChangedGraphName -= OnChangedGraphName;
		}

		void SetNodeGraphRoot(NodeGraph nodeGraph)
		{
			if (_NodeGraphRoot is object)
			{
				UnregisterRootGraphCallback();
				_NodeGraphRootInstanceID = 0;
			}

			if (_NodeGraphRoot != nodeGraph)
			{
				_TreeViewState.Clear();
			}

			_NodeGraphRoot = nodeGraph;

			ShowGraphTab(_NodeGraphRoot != null);
			if (_NodeGraphRoot != null)
			{
				_GraphTabHeaderElement.text = _NodeGraphRoot.graphName;
			}

			SetupToolbarObjectField();

			BuildTree();

			if (_NodeGraphRoot != null)
			{
				RegisterRootGraphCallback();
				_NodeGraphRootInstanceID = _NodeGraphRoot.GetInstanceID();
			}
			else
			{
				_NodeGraphRootInstanceID = 0;
			}
		}

		void RegisterCurrentGraphCallback()
		{
			UnregisterCurrentGraphCallback();

			_NodeGraphCurrent.destroyCallback += OnDestroyNodeGraph;
			NodeGraphCallback.RegisterStateChangedCallback(_NodeGraphCurrent, OnStateChanged);
		}

		void UnregisterCurrentGraphCallback()
		{
			_NodeGraphCurrent.destroyCallback -= OnDestroyNodeGraph;
			NodeGraphCallback.UnregisterStateChangedCallback(_NodeGraphCurrent, OnStateChanged);
		}

		void BuildBreadcrumbs()
		{
			List<TreeViewItem> items = new List<TreeViewItem>();

			NodeGraph currentGraph = _NodeGraphCurrent;
			if (currentGraph == null && _NodeGraphCurrentInstanceID != 0)
			{
				currentGraph = EditorUtility.InstanceIDToObject(_NodeGraphCurrentInstanceID) as NodeGraph;
			}
			var currentItem = FindTreeViewItem(currentGraph);
			while (currentItem != null && currentItem.id != 0)
			{
				items.Insert(0, currentItem);
				currentItem = currentItem.parent;
			}

			_Breadcrumbs.Clear();
			for (int i = 0; i < items.Count; i++)
			{
				var item = items[i];
				var graphItem = item as GraphTreeViewItem;
				ToolbarButton button = new ToolbarButton(() =>
				{
					ChangeCurrentNodeGraph(graphItem);
				});
				button.text = item.displayName;
				button.RemoveFromClassList(ToolbarButton.ussClassName);
				button.AddToClassList("breadcrumbs-item");
				if (i == 0)
				{
					button.AddToClassList("breadcrumbs-first-item");
				}
				if (graphItem.isExternal)
				{
					button.AddToClassList("external");
				}
				if (i == items.Count - 1)
				{
					button.AddToClassList("on");
				}
				item.onChangedDisplayName += _ =>
				{
					button.text = item.displayName;
				};
				_Breadcrumbs.Add(button);
			}
		}

		void SetNodeGraphCurrent(GraphTreeViewItem graphItem)
		{
			NodeGraph nodeGraph = graphItem?.nodeGraph;

			StoreCurrentTransform();

			if (_NodeGraphCurrent is object)
			{
				UnregisterCurrentGraphCallback();
				_NodeGraphCurrentInstanceID = 0;
			}

			_NodeGraphCurrent = nodeGraph;
			_SelectedGraphItem = graphItem;
			SelectGraphTreeItem(graphItem);

			if (_NodeGraphCurrent != null)
			{
				RegisterCurrentGraphCallback();
				_NodeGraphCurrentInstanceID = _NodeGraphCurrent.GetInstanceID();
			}
			else
			{
				_NodeGraphCurrentInstanceID = 0;
			}

			BuildBreadcrumbs();
		}

		bool ReselectIfNecessary(NodeGraph destroyTarget = null)
		{
			if (destroyTarget == null && _NodeGraphRoot != null && _NodeGraphCurrent != null)
			{
				return false;
			}

			Undo.RecordObject(this, "Reselect NodeGraph");

			if (_NodeGraphRoot == null || _NodeGraphRoot == destroyTarget)
			{
				ClearSelectGraph();
			}
			else if (_NodeGraphCurrent == null || _NodeGraphCurrent == destroyTarget)
			{
				BuildTree();

				NodeGraph parentGraph = null;
				if (_NodeGraphCurrent != null)
				{
					parentGraph = _NodeGraphCurrent.parentGraph;
				}
				if (parentGraph == null)
				{
					parentGraph = _NodeGraphRoot;
				}
				var parentItem = FindTreeViewItem(parentGraph) as GraphTreeViewItem;
				SetNodeGraphCurrent(parentItem);

				RebuildGraphEditor();
			}

			EditorUtility.SetDirty(this);

			DoRepaint();

			return true;
		}

		private bool ReatachIfNecessary()
		{
			if (ReselectIfNecessary())
			{
				return true;
			}

			RegisterRootGraphCallback();
			RegisterCurrentGraphCallback();
				
			if (_GraphEditor == null || _GraphEditor.nodeGraph != _NodeGraphCurrent)
			{
				RebuildGraphEditor();
			}

			return false;
		}

		void UpdateScrollbar()
		{
			if (_GraphView.UpdateFrameSelected())
			{
				DoRepaint();
			}
		}

		void OnStateChanged(NodeGraph nodeGraph)
		{
			_IsRepaint = true;

			if (_GraphEditor != null && _GraphEditor.nodeGraph == nodeGraph)
			{
				_IsUpdateLiveTracking = true;
			}
		}

		private void FlipLocked()
		{
			_IsLocked = !_IsLocked;
		}

		public void AddItemsToMenu(GenericMenu menu)
		{
			menu.AddItem(GUIContentCaches.Get("Lock"), _IsLocked, FlipLocked);
		}

		void OnChangedScroll()
		{
			StoreCurrentTransform();
		}

		private void ApplyStoredTransform(GraphTreeViewItem graphItem)
		{
			Vector3 scrollPos;
			float scale;

			if (graphItem != null && TransformCache.instance.TryGet(graphItem.nodeGraph, out var transform))
			{
				scrollPos = transform.position;
				scale = transform.scale.x;
			}
			else
			{
				scrollPos = _GraphView.graphExtents.center - _GraphView.graphViewRect.size * 0.5f;
				scrollPos.x = Mathf.Floor(scrollPos.x);
				scrollPos.y = Mathf.Floor(scrollPos.y);

				scale = 1f;
			}

			_GraphView.SetZoom(Vector2.zero, scale, false, false);
			_GraphView.SetScroll(scrollPos, true, false);
		}

		private void StoreCurrentTransform()
		{
			if (_SelectedGraphItem == null)
			{
				return;
			}

			if (!_GraphView.isLayoutSetup)
			{
				return;
			}

			if (!TransformCache.instance.TryGet(_SelectedGraphItem.nodeGraph, out var transform))
			{
				transform = new TransformData();
				TransformCache.instance.Set(_SelectedGraphItem.nodeGraph, transform);
			}

			transform.position = _GraphView.scrollPos;
			transform.scale = _GraphView.graphScale;
		}

#if ARBOR_TRIAL
		private class TrialButton : Button
		{
			public TrialButton(System.Action clickEvent) : base(clickEvent)
			{
			}
		}
#endif

		internal void ChangedSidePanel()
		{
			_MainLayout.ShowLeftPanel(ArborSettings.openSidePanel);
			if (ArborSettings.openSidePanel)
			{
				sidePanel.toolbar.Add(_SidePanelToggle);
			}
			else
			{
				_Toolbar.Insert(0, _SidePanelToggle);
			}
		}

		private static readonly string s_ItemExternalButtonName = "arbor-tree-view__item-external_button";

		VisualElement MakeTreeItem()
		{
			var itemContainer = new VisualElement()
			{
				style =
					{
						flexDirection = FlexDirection.Row,
					}
			};

			Button externalButton = null;
			System.Action clicked = () =>
			{
				var graphTreeViewItem = externalButton.userData as GraphTreeViewItem;
				SelectExternalGraph(graphTreeViewItem);
			};
			externalButton = new Button(clicked)
			{
				name = s_ItemExternalButtonName
			};
			externalButton.RemoveFromClassList(Button.ussClassName);
			externalButton.AddToClassList("arrow-navigation-right");

			itemContainer.hierarchy.Add(externalButton);

			return itemContainer;
		}

		void BindTreeItem(VisualElement element, TreeViewItem item)
		{
			var graphTreeItem = item as GraphTreeViewItem;
			var itemContainer = UIElementsUtility.GetFirstAncestorWithClass(element, TreeViewElement.itemUssClassName);
			itemContainer.EnableInClassList("external", graphTreeItem != null && graphTreeItem.isExternal);

			var externalButton = element.Q<Button>(s_ItemExternalButtonName);
			if (graphTreeItem != null && graphTreeItem.isExternal)
			{
				externalButton.userData = graphTreeItem;
				externalButton.style.display = DisplayStyle.Flex;
			}
			else
			{
				externalButton.style.display = DisplayStyle.None;
			}
		}

		void SetupElements()
		{
			_MainLayout = new GraphMainLayout() {
				name = "MainLayout"
			};

			sidePanel = new TabPanel<SidePanelTab>()
			{
				style =
				{
					minHeight = 100f,
				}
			};

			sidePanel.CreateTab("Graph", SidePanelTab.Graph);
			sidePanel.CreateTab("NodeList", SidePanelTab.NodeList);			
			sidePanel.CreateTab("Parameters", SidePanelTab.Parameters);			

			sidePanel.SetValueWithoutNotify(ArborSettings.sidePanelTab);

			sidePanel.RegisterValueChangedCallback(e =>
			{
				ArborSettings.sidePanelTab = e.newValue;
			});

			_MainLayout.leftPanel.Add(sidePanel);

			_GraphTabElement = new VisualElement()
			{
				style =
				{
					flexGrow = 1f,
				}
			};

			_GraphTabHeaderElement = new Button(() =>
			{
				EditorGUIUtility.PingObject(_NodeGraphRoot.gameObject);
			})
			{
				style =
				{
					height = 25f,
				}
			};
			_GraphTabHeaderElement.RemoveFromClassList(Button.ussClassName);
			_GraphTabHeaderElement.AddToClassList("header-bar");
			_GraphTabHeaderElement.AddToClassList("graph-tree-header");
			UIElementsUtility.SetBoldFont(_GraphTabHeaderElement);
			_GraphTabElement.Add(_GraphTabHeaderElement);

			Button externalPrevButton = new Button(() =>
			{
				SelectRootGraph(_NodeGraphRootPrev);
			});
			externalPrevButton.RemoveFromClassList(Button.ussClassName);
			externalPrevButton.AddToClassList("arrow-navigation-left");
			_GraphTabHeaderElement.Add(externalPrevButton);

			_GraphTreeElement = new TreeViewElement(_TreeView, _TreeViewState, MakeTreeItem, BindTreeItem)
			{
				selectSubmit = true,
				renamable = true,
			};
			_GraphTreeElement.AddToClassList("graph-tree");
			_GraphTreeElement.onSubmit += OnSubmitItem;
			_GraphTreeElement.onRenameEnded += OnRenameEndedItem;
			_GraphTabElement.Add(_GraphTreeElement);

			ShowGraphTab(_NodeGraphRoot != null);
			ShowGraphTabHeader(_NodeGraphRootPrev != null);
			if (_NodeGraphRoot != null)
			{
				_GraphTabHeaderElement.text = _NodeGraphRoot.graphName;
			}

			_MinimapElement = new ResizableElement()
			{
				minSize = 100f,
			};

			_MinimapElement.SetValueWithoutNotify(ArborSettings.minimapSize);

			var resizerLabel = new Label()
			{
				pickingMode = PickingMode.Ignore,
			};
			UIElementsUtility.SetBoldFont(resizerLabel);
			resizerLabel.AddManipulator(new LocalizationManipulator("Minimap", LocalizationManipulator.TargetText.Text));
			_MinimapElement.header.Add(resizerLabel);

			_MinimapElement.RegisterValueChangedCallback(e =>
			{
				ArborSettings.minimapSize = e.newValue;
			});

			_MinimapElement.contentContainer.AddToClassList("graphview-background");

			_MinimapView = new MinimapView(this)
			{
				style =
				{
					flexGrow = 1f,
				}
			};
			_MinimapElement.Add(_MinimapView);

			var sliderGUI = new IMGUIContainer(OnZoomSliderGUI)
			{
				style =
				{
					marginLeft = 2f,
					marginRight = 2f,
					marginBottom = 4f,
				}
			};
			_MinimapElement.Add(sliderGUI);

			_Toolbar = new Toolbar()
			{
				style =
				{
					paddingLeft = 6f,
					paddingRight = 6f,
					flexShrink = 0f,
				}
			};
			_MainLayout.rightPanel.Add(_Toolbar);
			
			_SidePanelToggle = new Toggle();
			_SidePanelToggle.AddToClassList("visibility");
			_SidePanelToggle.SetValueWithoutNotify(ArborSettings.openSidePanel);
			_SidePanelToggle.AddManipulator(new LocalizationManipulator("Side Panel", LocalizationManipulator.TargetText.Tooltip));

			_SidePanelToggle.RegisterValueChangedCallback(e =>
			{
				ArborSettings.openSidePanel = e.newValue;
				ChangedSidePanel();
			});

			ChangedSidePanel();

			ToolbarDropdown toolbarCreateDropdown = null;
			toolbarCreateDropdown = new ToolbarDropdown(() =>
			{
				OpenCreateMenu(toolbarCreateDropdown.worldBound);
			})
			{
				style =
				{
					flexShrink = 0f,
				}
			};
			toolbarCreateDropdown.AddManipulator(new LocalizationManipulator("Create", LocalizationManipulator.TargetText.Text));
			_Toolbar.Add(toolbarCreateDropdown);

			_ToolbarObjectField = new ObjectField()
			{
				objectType = typeof(NodeGraph),
				allowSceneObjects = true,
				style =
				{
					width = 200f,
					flexShrink = 1f,
				}
			};
			SetupToolbarObjectField();
			_ToolbarObjectField.RegisterValueChangedCallback(e =>
			{
				SelectRootGraph(e.newValue as NodeGraph);
			});

			_Toolbar.Add(_ToolbarObjectField);

			_Toolbar.Add(new ToolbarSpacer()
			{
				flex = true,
			});

			_ToolbarGraphEditor = new VisualElement()
			{
				style =
				{
					flexDirection = FlexDirection.Row,
					flexShrink = 0f,
				}
			};
			_Toolbar.Add(_ToolbarGraphEditor);

			SetupToolbarGraphEditor();

			EnableCustomToolbar(s_ToolbarGUI != null);

			_ToolbarLiveTracking = new ToolbarToggle();
			_ToolbarLiveTracking.AddManipulator(new LocalizationManipulator("Live Tracking", LocalizationManipulator.TargetText.Text));
			_ToolbarLiveTracking.SetValueWithoutNotify(ArborSettings.liveTracking);

			_ToolbarLiveTracking.RegisterValueChangedCallback(e =>
			{
				ArborSettings.liveTracking = e.newValue;
				if (EditorApplication.isPlaying)
				{
					_IsUpdateLiveTracking = true;
				}
			});

			_ToolbarGraphEditor.Add(_ToolbarLiveTracking);

			ToolbarDropdown toolbarViewDropdown = null;
			toolbarViewDropdown = new ToolbarDropdown(() =>
			{
				GenericMenu menu = new GenericMenu();

				_GraphEditor.SetViewMenu(menu);

				menu.DropDown(toolbarViewDropdown.worldBound);
			});
			toolbarViewDropdown.AddManipulator(new LocalizationManipulator("View", LocalizationManipulator.TargetText.Text));
			_ToolbarGraphEditor.Add(toolbarViewDropdown);

			ToolbarDropdown toolbarDebugDropdown = null;
			toolbarDebugDropdown = new ToolbarDropdown(() =>
			{
				GenericMenu menu = new GenericMenu();

				_GraphEditor.SetDenugMenu(menu);

				menu.DropDown(toolbarDebugDropdown.worldBound);
			});
			toolbarDebugDropdown.AddManipulator(new LocalizationManipulator("Debug", LocalizationManipulator.TargetText.Text));
			_ToolbarGraphEditor.Add(toolbarDebugDropdown);

			Button captureButton = new Button(() =>
			{
				_GraphView.Capture();
			});
			captureButton.RemoveFromClassList(Button.ussClassName);
			captureButton.AddToClassList("toolbar-icon-button");
			captureButton.Add(new Image()
			{
				image = Icons.captureIcon,
				tintColor = EditorGUITools.GetIconColor(),
			});
			captureButton.AddManipulator(new LocalizationManipulator("Screen Shot", LocalizationManipulator.TargetText.Tooltip));
			_ToolbarGraphEditor.Add(captureButton);

			_ToolbarNotificationButton = new Button(UpdateNotificationWindow.Open)
			{
				style =
				{
					flexShrink = 0f,
				}
			};
			_ToolbarNotificationButton.RemoveFromClassList(Button.ussClassName);
			_ToolbarNotificationButton.AddToClassList("toolbar-icon-button");
			_ToolbarNotificationButton.Add(new Image()
			{
				image = Icons.notificationIcon,
				tintColor = EditorGUITools.GetIconColor(),
			});
			_ToolbarNotificationButton.AddManipulator(new LocalizationManipulator("Notification", LocalizationManipulator.TargetText.Tooltip));
			_ToolbarNotificationButton.RegisterCallback<AttachToPanelEvent>(e =>
			{
				ArborVersion.instance.onLoaded += SetupNotificationButton;
			});
			_ToolbarNotificationButton.RegisterCallback<DetachFromPanelEvent>(e =>
			{
				ArborVersion.instance.onLoaded -= SetupNotificationButton;
			});
			_Toolbar.Add(_ToolbarNotificationButton);

			Button helpButton = null;
			helpButton = new Button(() =>
			{
				GenericMenu menu = new GenericMenu();

				SetHelpMenu(menu);

				menu.DropDown(helpButton.worldBound);
			})
			{
				style =
				{
					flexShrink = 0f,
				}
			};
			helpButton.RemoveFromClassList(Button.ussClassName);
			helpButton.AddToClassList("toolbar-icon-button");
			helpButton.Add(new Image()
			{
				image = Icons.helpIcon,
			});
			helpButton.AddManipulator(new LocalizationManipulator("Help", LocalizationManipulator.TargetText.Tooltip));
			_Toolbar.Add(helpButton);

			Button settingsButton = null;
			settingsButton = new Button(() =>
			{
				if (_GraphSettingsWindow == null)
				{
					_GraphSettingsWindow = new GraphSettingsWindow(this);
				}
				PopupWindowUtility.Show(settingsButton.worldBound, _GraphSettingsWindow, true);
			})
			{
				style =
				{
					flexShrink = 0f,
				}
			};
			settingsButton.RemoveFromClassList(Button.ussClassName);
			settingsButton.AddToClassList("toolbar-icon-button");
			settingsButton.AddManipulator(new LocalizationManipulator("Settings", LocalizationManipulator.TargetText.Tooltip));
			settingsButton.Add(new Image()
			{
				image = Icons.popupIcon,
			});
			_Toolbar.Add(settingsButton);

			_GraphPanel = new VisualElement() {
				name = "GraphPanel",
				style =
				{
					flexGrow = 1f,
				}
			};

			_GraphView = new GraphView() {
				name = "GraphView",
			};
			_GraphView.graphEditor = _GraphEditor;

			_GraphView.onChangedScroll += OnChangedScroll;
			_GraphView.onPostLayout += OnPostLayout;

			OnChangedShowGrid();

#if ARBOR_TRIAL
			TrialButton assetStoreButton = new TrialButton(() =>
			{
				ArborVersion.OpenAssetStore();
			})
			{
				text = "Open Asset Store",
				style =
				{
					position = Position.Absolute,
					bottom = 16,
					left = 16,
				}
			};

			_GraphView.contentOverlay.Add(assetStoreButton);
#endif

			_NoGraphUI = new NoGraphElement(this);

			ShowGraphView(_GraphEditor != null);

			_MainLayout.rightPanel.Add(_GraphPanel);

			_Breadcrumbs = new Toolbar();
			_Breadcrumbs.AddToClassList("breadcrumbs");
			_MainLayout.rightPanel.Add(_Breadcrumbs);

			rootVisualElement.Add(_MainLayout);

			ArborStyleSheets.Setup(rootVisualElement);

			rootVisualElement.RegisterCallback<FocusEvent>(OnFocusElement, TrickleDown.TrickleDown);
			rootVisualElement.RegisterCallback<BlurEvent>(OnBlurElement, TrickleDown.TrickleDown);
		}

		private VisualElement _CurrentFocusedElement;

		void OnFocusElement(FocusEvent e)
		{
			_CurrentFocusedElement = e.target as VisualElement;
		}

		void OnBlurElement(BlurEvent e)
		{
			if (_CurrentFocusedElement == e.target)
			{
				_CurrentFocusedElement = null;
			}
		}

		void ShowGraphTab(bool show)
		{
			if (show)
			{
				if (_GraphTabElement.parent == null)
				{
					sidePanel.GetTab(SidePanelTab.Graph).Add(_GraphTabElement);
				}
			}
			else if (_GraphTabElement.parent != null)
			{
				_GraphTabElement.RemoveFromHierarchy();
			}
		}

		void ShowGraphTabHeader(bool show)
		{
			if (show)
			{
				_GraphTabHeaderElement.style.display = DisplayStyle.Flex;
			}
			else
			{
				_GraphTabHeaderElement.style.display = DisplayStyle.None;
			}
		}

		void OnZoomSliderGUI()
		{
			if (_GraphEditor == null || _GraphEditor.nodeGraph == null)
			{
				return;
			}

			float zoomLevel = _GraphView.zoomLevel * 100f;
			EditorGUI.BeginChangeCheck();
			zoomLevel = EditorGUILayout.Slider(GUIContent.none, zoomLevel, GraphView.k_ZoomMin, GraphView.k_ZoomMax) / 100f;
			if (EditorGUI.EndChangeCheck())
			{
				_GraphView.SetZoom(_GraphView.graphViewport.center, zoomLevel, true);
			}
		}

		void EnableCustomToolbar(bool enable)
		{
			if (enable)
			{
				if (_CustomToolbarGUI == null)
				{
					System.Action onGUIHandler = () =>
					{
						if (_GraphEditor != null && s_ToolbarGUI != null)
						{
							EditorGUILayout.BeginHorizontal();

							s_ToolbarGUI(_GraphEditor.nodeGraph);

							EditorGUILayout.EndHorizontal();
						}
					};
					var imguiContainer = new IMGUIContainer(onGUIHandler);
					_CustomToolbarGUI = imguiContainer;
				}
				if (_CustomToolbarGUI.parent == null)
				{
					_ToolbarGraphEditor.Insert(0, _CustomToolbarGUI);
				}
			}
			else
			{
				if (_CustomToolbarGUI != null && _CustomToolbarGUI.parent != null)
				{
					_CustomToolbarGUI.RemoveFromHierarchy();
				}
			}
		}

		void SetupToolbarObjectField()
		{
			_ToolbarObjectField?.SetValueWithoutNotify(_NodeGraphRootPrev != null? _NodeGraphRootPrev : _NodeGraphRoot);
		}

		void SetupToolbarGraphEditor()
		{
			if (_GraphEditor != null)
			{
				_ToolbarGraphEditor.style.display = StyleKeyword.Null;
			}
			else
			{
				_ToolbarGraphEditor.style.display = DisplayStyle.None;
			}
		}

		void SetupNotificationButton()
		{
			ArborUpdateCheck updateCheck = ArborUpdateCheck.instance;
			if (updateCheck.isUpdated || updateCheck.isUpgrade)
			{
				_ToolbarNotificationButton.style.display = StyleKeyword.Null;
			}
			else
			{
				_ToolbarNotificationButton.style.display = DisplayStyle.None;
			}
		}

#if !UNITY_2021_1_OR_NEWER
		// Worked around a Unity issue that wasn't reflected when renamed.
		sealed class ObjectField : UnityEditor.UIElements.ObjectField
		{
			private readonly System.Action _AsyncOnProjectOrHierarchyChangedCallback;
			private readonly System.Action _OnProjectOrHierarchyChangedCallback;

			public ObjectField() : base()
			{
				_AsyncOnProjectOrHierarchyChangedCallback = () => schedule.Execute(_OnProjectOrHierarchyChangedCallback);
				_OnProjectOrHierarchyChangedCallback = UpdateContent;
				RegisterCallback<AttachToPanelEvent>((evt) =>
				{
					EditorApplication.projectChanged += _AsyncOnProjectOrHierarchyChangedCallback;
					EditorApplication.hierarchyChanged += _OnProjectOrHierarchyChangedCallback;
				});
				RegisterCallback<DetachFromPanelEvent>((evt) =>
				{
					EditorApplication.projectChanged -= _AsyncOnProjectOrHierarchyChangedCallback;
					EditorApplication.hierarchyChanged -= _OnProjectOrHierarchyChangedCallback;
				});
			}

			void UpdateContent()
			{
				// Call ObjectFieldDisplay.Update ().
				// Since it cannot be called internally, change the objectType and call it indirectly.
				var tmpType = objectType;
				objectType = typeof(Object);
				objectType = tmpType;
			}
		}
#endif
	}
}