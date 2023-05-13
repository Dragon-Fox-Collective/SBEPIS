//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;
	using ArborEditor.UIElements;
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	internal sealed class RebuildElementEvent : EventBase<RebuildElementEvent>
	{
	}

	internal sealed class UndoRedoPerformedEvent : EventBase<UndoRedoPerformedEvent>
	{
	}

	public abstract class NodeEditor : ScriptableObject
	{
		public static bool HasEditor(Node node)
		{
			if (node == null)
			{
				return false;
			}

			System.Type classType = node.GetType();
			System.Type editorType = CustomAttributes<CustomNodeEditor, NodeEditor>.FindEditorType(classType);

			return editorType != null;
		}

		public static NodeEditor CreateEditors(NodeGraphEditor graphEditor, Node node)
		{
			if (node == null)
			{
				return null;
			}

			System.Type classType = node.GetType();
			System.Type editorType = CustomAttributes<CustomNodeEditor, NodeEditor>.FindEditorType(classType);

			if (editorType == null)
			{
				return null;
			}

			NodeEditor nodeEditor = CreateInstance(editorType) as NodeEditor;
			nodeEditor.hideFlags = HideFlags.HideAndDontSave;
			nodeEditor.Initialize(graphEditor, node);

			return nodeEditor;
		}

		[SerializeField]
		private int _NodeID;

		[SerializeField]
		private NodeGraphEditor _GraphEditor;

		[System.NonSerialized]
		private Node _Node;

		public int nodeID
		{
			get
			{
				return _NodeID;
			}
		}

		public Node node
		{
			get
			{
				return _Node;
			}
		}

		public NodeGraphEditor graphEditor
		{
			get
			{
				return _GraphEditor;
			}
		}

		public bool isSelection
		{
			get
			{
				return _IsSelection;
			}
		}

		public Vector2 position
		{
			get
			{
				return _Node.position.position;
			}
		}

		public Rect rect
		{
			get
			{
				return _NodeElement.rectOnGraph;
			}
			set
			{
				_Node.position = value;
			}
		}

		internal void SetSelection(bool isSelection)
		{
			if (isSelection != _IsSelection)
			{
				_IsSelection = isSelection;

				_IsFirstUpdateStyle = true;
				UpdateStyles();

				UpdateMinimapStyles();
				
				OnChangeSelection(_IsSelection);
			}
		}

		protected virtual void OnChangeSelection(bool isSelection)
		{
		}

		public enum ShowContextMenu
		{
			None,
			StopPropagation,
			Show,
		}

		public bool isRenamable = false;
		public bool isShowableComment = false;
		public bool isNormalInvisibleStyle = false;
		public bool isShowContextMenuInHeader = true;
		public ShowContextMenu showContextMenuInWindow = ShowContextMenu.StopPropagation;
		public bool isStopPropagationContectClickInWindow = true;
		public bool isUsedMouseDownOnMainGUI = true;

		[SerializeField]
		private bool _IsResizable = true;

		public bool isResizable
		{
			get
			{
				return _IsResizable;
			}
			set
			{
				if (_IsResizable != value)
				{
					_IsResizable = value;

					if (_NodeElement != null)
					{
						if (_IsResizable)
						{
							_NodeElement.resizeDirection = GetResizeDirection();
						}
						else
						{
							_NodeElement.resizeDirection = 0;
						}
					}

					_IsDirtyWidth = true;
				}
			}
		}

		private bool _IsDirtyWidth = false;

		internal void DoUndoRedoPerformed()
		{
			SetSelection(graphEditor.IsSelection(node));

			OnUndoRedoPerformed();

			if (_NodeElement != null)
			{
				using (var e = UndoRedoPerformedEvent.GetPooled())
				{
					e.target = _NodeElement;
					_NodeElement.SendEvent(e);
				}
			}
		}

		protected virtual void OnUndoRedoPerformed()
		{
		}

		public virtual void Validate(Node node, bool onInitialize)
		{
			if (SetNode(node))
			{
				RebuildElements();
			}
		}

		internal void RebuildElements()
		{
			if (_NodeElement == null)
			{
				InitializeNodeElement();
			}

			if (_MinimapNodeElement == null)
			{
				InitializeMinimapNodeElement();
			}

			using (var e = RebuildElementEvent.GetPooled())
			{
				e.target = _NodeElement;
				_NodeElement.SendEvent(e);
			}
		}

		public bool IsValidNode(Node node)
		{
			if (node == null)
			{
				return false;
			}

			System.Type classType = node.GetType();
			System.Type editorType = CustomAttributes<CustomNodeEditor, NodeEditor>.FindEditorType(classType);

			if (editorType == null)
			{
				return false;
			}

			return editorType == GetType();
		}

		private NodeElement _NodeElement;
		internal NodeElement nodeElement
		{
			get
			{
				if (_NodeElement == null)
				{
					InitializeNodeElement();
				}

				return _NodeElement;
			}
		}

		private NodeHeaderElement _HeaderElement;
		private NodeCommentElement _CommentElement;

		protected virtual VisualElement CreateContentElement()
		{
			return null;
		}

		protected virtual Color GetBackgroundColor()
		{
			return Color.white;
		}

		void InitializeNodeElement()
		{
			_NodeElement = new NodeElement(this);
			if (isResizable)
			{
				_NodeElement.resizeDirection = GetResizeDirection();
			}

			_NodeElement.UpdatePosition();

			_NodeElement.RegisterCallback<GeometryChangedEvent>(e =>
			{
				isLayouted = true;
			});

			RegisterCallbackOnElement();

			_NodeElement.AddManipulator(new ContextClickManipulator(OnWindowContextClick));
			_NodeElement.contentContainer.RegisterCallback<MouseDownEvent>(OnContentMouseDown);
			_NodeElement.contentContainer.RegisterCallback<DragUpdatedEvent>(OnContentDragUpdated);

			VisualElement preHeader = CreatePreHeaderElement();
			if (preHeader != null)
			{
				_NodeElement.headerContainer.Add(preHeader);
			}

			if (HasHeaderGUI())
			{
				_HeaderElement = new NodeHeaderElement()
				{
					color = GetStyleColor(),
				};
				_NodeElement.headerContainer.Add(_HeaderElement);

				_HeaderElement.RegisterCallback<DragEnterEvent>(OnHeaderDragEvent);
				_HeaderElement.RegisterCallback<DragLeaveEvent>(OnHeaderDragEvent);
				_HeaderElement.RegisterCallback<DragUpdatedEvent>(OnHeaderDragEvent);
				_HeaderElement.RegisterCallback<DragPerformEvent>(OnHeaderDragEvent);
				_HeaderElement.RegisterCallback<DragExitedEvent>(OnHeaderDragEvent);

				_HeaderElement.AddManipulator(new ContextClickManipulator((e) =>
				{
					if (isShowContextMenuInHeader)
					{
						DoContextMenu(new Rect(e.mousePosition, Vector2.zero), _HeaderElement.parent.LocalToWorld(_HeaderElement.layout));
					}
					e.StopPropagation();
				}));

				_HeaderElement.icon = GetIcon();
				OnCreatedHeaderIcon(_HeaderElement.iconElement);

				_HeaderElement.title = GetTitle();
				_HeaderElement.titleElement.RegisterCallback<MouseDownEvent>(e =>
				{
					if (e.button == 0 && e.clickCount == 2)
					{
						if (isRenamable && graphEditor != null && graphEditor.editable && !graphEditor.IsRenaming())
						{
							BeginRename();
							_HeaderElement.title = "";
							e.StopPropagation();
						}
					}
				});

				if (isShowContextMenuInHeader)
				{
					_HeaderElement.onSettings = (Rect popupRect) =>
					{
						Rect headerAreaRect = _HeaderElement.parent.LocalToWorld(_HeaderElement.layout);
						DoContextMenu(popupRect, headerAreaRect);
					};
				}
			}

			VisualElement contentElement = CreateContentElement();
			if (contentElement != null)
			{
				if (HasContentBackground())
				{
					VisualElement background = new VisualElement();
					background.AddToClassList("node-contents-background");
					_NodeElement.Add(background);
					background.Add(contentElement);
				}
				else
				{
					_NodeElement.Add(contentElement);
				}
			}

			VisualElement footer = CreateFooterElement();
			if (footer != null)
			{
				_NodeElement.footerContainer.Add(footer);
			}

			if (graphEditor != null)
			{
				GraphView graphView = graphEditor.graphView;

				graphView.GetLayer(IsWindow()).Add(_NodeElement);
			}

			_CommentElement = new NodeCommentElement(this);

			OnChangeNodeCommentViewMode();

			_IsFirstUpdateStyle = true;
			UpdateStyles();
		}

		private VisualElement _MinimapNodeElement;
		public VisualElement minimapNodeElement
		{
			get
			{
				return _MinimapNodeElement;
			}
		}

		protected virtual void OnCreatedMinimapNodeElement()
		{
		}

		void InitializeMinimapNodeElement()
		{
			if (minimapLayer == MinimapLayer.None || _GraphEditor == null)
			{
				return;
			}

			_MinimapNodeElement = new MinimapNodeElement(_NodeElement);

			MinimapView minimapView = _GraphEditor.minimapView;

			var nodeLayer = minimapView.GetNodeLayer(minimapLayer);
			nodeLayer?.Add(_MinimapNodeElement);

			OnCreatedMinimapNodeElement();

			UpdateMinimapStyles();
		}

		void OnChangeNodeCommentViewMode()
		{
			if (isShowableComment && NodeGraphEditor.IsShowNodeComment(node))
			{
				if (_CommentElement != null && _CommentElement.parent == null)
				{
					graphEditor.graphView.nodeCommentLayer.Add(_CommentElement);
				}
			}
			else
			{
				if (_CommentElement != null && _CommentElement.parent != null)
				{
					_CommentElement.RemoveFromHierarchy();
				}
			}
		}

		protected virtual void RegisterCallbackOnElement()
		{
		}

		protected virtual void OnCreatedHeaderIcon(VisualElement headerIcon)
		{
		}

		internal void DoRepaintedEvent()
		{
			if (_NodeElement.resolvedStyle.display == DisplayStyle.None || !_NodeElement.visible)
			{
				return;
			}

			isRepainted = true;
			
			OnRepainted();
		}

		protected virtual void OnRepainted()
		{
		}

		void OnContentMouseDown(MouseDownEvent e)
		{
			if (isUsedMouseDownOnMainGUI)
			{
#if ARBOR_DEBUG
				Debug.Log("NodeEditor : MouseDown to Used.");
#endif
				e.StopPropagation();
			}
		}

		void OnWindowContextClick(ContextClickEvent e)
		{
			switch (showContextMenuInWindow)
			{
				case ShowContextMenu.StopPropagation:
					e.StopPropagation();
					break;
				case ShowContextMenu.Show:
					{
						VisualElement contentContainer = _NodeElement.contentContainer;
						Rect contentRect = contentContainer.contentRect;
						contentRect = contentContainer.LocalToWorld(contentRect);
						DoContextMenu(new Rect(e.mousePosition, Vector2.zero), contentRect);
						e.StopPropagation();
					}
					break;
			}
		}

		void OnContentDragUpdated(DragUpdatedEvent e)
		{
			DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
			e.StopPropagation();
		}

		public void Initialize(NodeGraphEditor graphEditor, Node node)
		{
			_NodeID = node.nodeID;

			_GraphEditor = graphEditor;
			SetNode(node);

			if (_GraphEditor != null)
			{
				_IsSelection = _GraphEditor.IsSelection(node);
			}

			InitializeNodeElement();
			InitializeMinimapNodeElement();

			OnInitialize();

			Update();
		}

		bool SetNode(Node node)
		{
			if (_Node != node)
			{
				if (_Node != null)
				{
					OnDetachNode(_Node);
				}

				_Node = node;

				if (_Node != null)
				{
					OnAttachNode(_Node);
				}

				return true;
			}

			return false;
		}

		protected virtual void OnDetachNode(Node node)
		{
		}

		protected virtual void OnAttachNode(Node node)
		{
		}

		protected virtual void OnEnable()
		{
			ArborSettings.onChangeNodeCommentViewMode += OnChangeNodeCommentViewMode;
		}

		protected virtual void OnDisable()
		{
			ArborSettings.onChangeNodeCommentViewMode -= OnChangeNodeCommentViewMode;
		}

		protected virtual void OnDestroy()
		{
			if (_Node != null)
			{
				OnDetachNode(_Node);
			}

			_NodeElement?.RemoveFromHierarchy();
			_CommentElement?.RemoveFromHierarchy();
			_MinimapNodeElement?.RemoveFromHierarchy();
		}

		private bool _IsFirstUpdateStyle = false;
		private Styles.BaseColor _StyleBaseColor;
		private bool _IsSelection;
		private bool _IsActive;
		private bool _IsHover = false;

		public virtual bool IsActive()
		{
			return false;
		}

		protected virtual void OnUpdate()
		{
		}

		internal void Update()
		{
			UpdateStyles();
			UpdateDirtyWidth();
			_NodeElement?.UpdatePosition();

			UpdateMinimapStyles();

			OnUpdate();
		}

		protected virtual void OnUpdateStyles()
		{
		}

		private Styles.Color _HeaderColor;

		internal event System.Action onStyleChanged;

		void UpdateStyles()
		{
			Styles.BaseColor color = GetStyleBaseColor();
			Styles.Color headerColor = GetStyleColor();
			bool isActive = IsActive();
			bool isHover = _IsHover;

			Color backgroundColor = GetBackgroundColor();

			if (headerColor != _HeaderColor)
			{
				_HeaderColor = headerColor;

				onStyleChanged?.Invoke();
			}

			_NodeElement.background.style.unityBackgroundImageTintColor = backgroundColor;
			if (_HeaderElement != null)
			{
				_HeaderElement.color = headerColor;
				_HeaderElement.backgroundColor = backgroundColor;
				_HeaderElement.icon = GetIcon();
				if (graphEditor == null || !graphEditor.IsRenaming(node.nodeID))
				{
					_HeaderElement.title = GetTitle();
				}
				else
				{
					_HeaderElement.title = "";
				}
			}

			if (_IsFirstUpdateStyle || color != _StyleBaseColor || isActive != _IsActive || isHover != _IsHover)
			{
				if (isNormalInvisibleStyle && !_IsSelection && !isActive)
				{
					_NodeElement.background.ClearClassList();
					_NodeElement.background.AddToClassList("node-base-invisible");

					_NodeElement.frame.ClearClassList();
					_NodeElement.background.AddToClassList("node-frame-invisible");
				}
				else
				{
					_NodeElement.background.ClearClassList();
					_NodeElement.background.AddToClassList(Styles.GetNodeBaseClassName(color));

					_NodeElement.frame.ClearClassList();
					_NodeElement.frame.AddToClassList(Styles.GetNodeFrameClassName(_IsSelection, isActive));
				}

				if (_IsHover != isHover)
				{
					Repaint();
				}

				_StyleBaseColor = color;
				_IsActive = isActive;
				_IsHover = isHover;

				_IsFirstUpdateStyle = false;
			}

			OnUpdateStyles();
		}

		protected virtual void OnUpdateMinimapStyles()
		{
		}

		void UpdateMinimapStyles()
		{
			if (_MinimapNodeElement == null)
			{
				return;
			}

			_MinimapNodeElement.EnableInClassList("on", isSelection);
			_MinimapNodeElement.style.unityBackgroundImageTintColor = GetListColor();

			OnUpdateMinimapStyles();
		}

		private GUIContent _ListElementContent = new GUIContent();

		public virtual GUIContent GetListElementContent()
		{
			_ListElementContent.text = GetTitle();
			_ListElementContent.image = GetIcon();

			return _ListElementContent;
		}

		public virtual Styles.BaseColor GetStyleBaseColor()
		{
			return Styles.BaseColor.Gray;
		}

		public virtual Styles.Color GetStyleColor()
		{
			return Styles.Color.Gray;
		}

		protected virtual float GetWidth()
		{
			return Node.defaultWidth;
		}

		internal float Internal_GetWidth()
		{
			return GetWidth();
		}

		void UpdateDirtyWidth()
		{
			if (!_IsDirtyWidth)
			{
				return;
			}

			Rect position = _Node.position;

			float width = GetWidth();

			if (isResizable)
			{
				width = Mathf.Max(width, position.width);
			}

			position.width = width;

			this.rect = position;

			_IsDirtyWidth = false;
		}

		protected virtual bool HasHeaderGUI()
		{
			return true;
		}

		protected virtual VisualElement CreatePreHeaderElement()
		{
			return null;
		}

		protected virtual VisualElement CreateFooterElement()
		{
			return null;
		}

		protected virtual bool HasContentBackground()
		{
			return true;
		}

		public virtual Rect GetSelectableRect()
		{
			return this.rect;
		}

		public virtual void OnBeginDrag(bool altKey)
		{
			graphEditor.RegisterDragNode(node);
		}

		public bool IsHover(Vector2 position)
		{
			return this.rect.Contains(position);
		}

		protected virtual ResizeDirection GetResizeDirection()
		{
			return ResizeDirection.Left | ResizeDirection.Right;
		}

		public Vector2 NodeToGraphPoint(Vector2 point)
		{
			if (_GraphEditor == null)
			{
				return point;
			}

			var graphView = _GraphEditor.graphView;
			return graphView.ElementToGraph(nodeElement, point);
		}

		public Rect NodeToGraphRect(Rect rect)
		{
			if (_GraphEditor == null)
			{
				return rect;
			}

			var graphView = _GraphEditor.graphView;
			return graphView.ElementToGraph(nodeElement, rect);
		}

		public bool IsSelectPoint(Vector2 point)
		{
			Rect position = GetSelectableRect();
			return position.Contains(point);
		}

		public bool IsSelectRect(Rect rect)
		{
			Rect position = GetSelectableRect();
			return (position.xMax >= rect.x && position.x <= rect.xMax &&
				position.yMax >= rect.y && position.y <= rect.yMax);
		}

		public virtual MinimapLayer minimapLayer
		{
			get
			{
				return MinimapLayer.Middle;
			}
		}

		internal void OnStartResize()
		{
			Undo.IncrementCurrentGroup();
			graphEditor.ChangeSelectNode(node, false);
		}

		internal void OnResizing(Rect newNodePosition)
		{
			graphEditor.DoMoveNode(node, newNodePosition);

			graphEditor.UpdateLayout();
		}

		public bool isLayouted
		{
			get;
			private set;
		}

		public bool isRepainted
		{
			get;
			internal set;
		}

		protected virtual void OnInitialize()
		{
		}

		protected virtual void SetContextMenu(GenericMenu menu, Rect headerPosition, bool editable)
		{
		}

		protected virtual void SetDebugContextMenu(GenericMenu menu, Rect headerPosition, bool editable)
		{
		}

		void ChangeShowComment()
		{
			bool showComment = !NodeEditorUtility.GetShowComment(node);

			NodeEditorUtility.SetShowComment(node, showComment);

			if (graphEditor.editable)
			{
				NodeGraph nodeGraph = node.nodeGraph;

				Undo.RecordObject(nodeGraph, "Change Show Comment");

				node.showComment = showComment;

				EditorUtility.SetDirty(nodeGraph);
			}

			OnChangeNodeCommentViewMode();
		}

		void DoContextMenu(Rect popupPosition, Rect headerPosition)
		{
			GUI.UnfocusWindow();

			GenericMenu menu = new GenericMenu();

			bool editable = graphEditor.editable;

			if (isRenamable)
			{
				if (editable)
				{
					menu.AddItem(EditorContents.rename, false, BeginRename);
				}
				else
				{
					menu.AddDisabledItem(EditorContents.rename);
				}
			}

			if (isShowableComment)
			{
				switch (ArborSettings.nodeCommentViewMode)
				{
					case NodeCommentViewMode.Normal:
						menu.AddItem(EditorContents.showComment, NodeEditorUtility.GetShowComment(node), ChangeShowComment);
						break;
					case NodeCommentViewMode.ShowAll:
						menu.AddDisabledItem(EditorContents.showCommentViewModeShowAll);
						break;
					case NodeCommentViewMode.ShowCommentedOnly:
						menu.AddDisabledItem(EditorContents.showCommentViewModeShowCommentedOnly);
						break;
					case NodeCommentViewMode.HideAll:
						menu.AddDisabledItem(EditorContents.showCommentViewModeHideAll);
						break;
				}
			}

			SetContextMenu(menu, headerPosition, editable);

			if (menu.GetItemCount() > 0)
			{
				menu.AddSeparator("");
			}

			SetNodeContextMenu(menu, editable);

			SetDebugContextMenu(menu, headerPosition, editable);

			menu.DropDown(popupPosition);
		}

		private void BeginRename()
		{
			if (graphEditor != null)
			{
				graphEditor.BeginRename(node.nodeID, node.name);
			}
		}

		protected virtual void OnHeaderDragEvent(IDragAndDropEvent e)
		{
		}

		protected virtual bool IsWindow()
		{
			return true;
		}

		protected void Repaint()
		{
			if (_GraphEditor != null)
			{
				_GraphEditor.Repaint();
			}
		}

		internal Rect GetNameRect()
		{
			return graphEditor.graphView.ElementToGraph(_HeaderElement.titleElement.parent, _HeaderElement.titleElement.layout);
		}

		public Rect GetHeaderRect()
		{
			return graphEditor.graphView.ElementToGraph(_HeaderElement.parent, _HeaderElement.layout);
		}

		void CopyNode()
		{
			Clipboard.CopyNodes(new Node[] { node });
		}

		void CutNode()
		{
			Clipboard.CopyNodes(new Node[] { node });
			DeleteNode();
		}

		void DeleteNode()
		{
			_GraphEditor.DeleteNodes(new Node[] { node });
		}

		void DuplicateNode(object obj)
		{
			Vector2 position = (Vector2)obj;

			_GraphEditor.DuplicateNodes(position, new Node[] { node });
		}

		protected virtual void SetDeleteContextMenu(GenericMenu menu, bool deletable, bool editable)
		{
		}

		public virtual bool IsCopyable()
		{
			return true;
		}

		protected void SetNodeContextMenu(GenericMenu menu, bool editable)
		{
			bool isCopyable = IsCopyable();
			bool isDeletable = node.IsDeletable();

			if (isCopyable && isDeletable && editable)
			{
				menu.AddItem(EditorContents.cut, false, CutNode);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.cut);
			}
			if (isCopyable)
			{
				menu.AddItem(EditorContents.copy, false, CopyNode);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.copy);
			}

			menu.AddSeparator("");

			if (isCopyable && editable)
			{
				Rect nodePosition = this.rect;
				Vector2 duplicatePosition = new Vector2(nodePosition.xMax, nodePosition.yMin);
				if (ArborSettings.showGrid && ArborSettings.snapGrid)
				{
					float gridSizeMinor = ArborSettings.gridSize / (float)ArborSettings.gridSplitNum;

					int num1 = Mathf.CeilToInt(duplicatePosition.x / gridSizeMinor) + 1;
					int num2 = Mathf.FloorToInt(duplicatePosition.y / gridSizeMinor);
					duplicatePosition.x = num1 * gridSizeMinor;
					duplicatePosition.y = num2 * gridSizeMinor;
				}
				else
				{
					duplicatePosition.x += 10f;
				}
				menu.AddItem(EditorContents.duplicate, false, DuplicateNode, duplicatePosition);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.duplicate);
			}

			if (isDeletable && editable)
			{
				menu.AddItem(EditorContents.delete, false, DeleteNode);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.delete);
			}
			SetDeleteContextMenu(menu, isDeletable, editable);
		}

		public virtual bool IsDraggingVisible()
		{
			return false;
		}

		internal void OnRename(string name)
		{
			if (name != node.name)
			{
				NodeGraph nodeGraph = node.nodeGraph;

				Undo.RecordObject(nodeGraph, "Rename Node");

				node.name = name;

				EditorUtility.SetDirty(nodeGraph);
			}
		}

		public virtual string GetTitle()
		{
			return node.GetName();
		}

		public virtual bool IsShowNodeList()
		{
			return false;
		}

		public virtual Color GetListColor()
		{
			return Styles.GetColor(GetStyleColor());
		}

		public virtual Texture2D GetIcon()
		{
			return null;
		}

		public virtual void OnBindListElement(VisualElement element)
		{
		}

		public virtual void ExpandAll(bool expanded)
		{
		}
	}
}