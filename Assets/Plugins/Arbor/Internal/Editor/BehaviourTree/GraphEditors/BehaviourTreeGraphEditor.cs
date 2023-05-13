//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.Collections.Generic;

namespace ArborEditor.BehaviourTree
{
	using Arbor;
	using Arbor.BehaviourTree;
	using Arbor.Playables;
	using ArborEditor.UIElements;
	using ArborEditor.BehaviourTree.UIElements;

	[CustomNodeGraphEditor(typeof(BehaviourTreeInternal))]
	public sealed class BehaviourTreeGraphEditor : NodeGraphEditor
	{
		private static class Types
		{
			public static readonly System.Type SetParameterActionType;

			static Types()
			{
				SetParameterActionType = AssemblyHelper.GetTypeByName("Arbor.ParameterBehaviours.SetParameterAction");
			}
		}

		BehaviourTreeInternal behaviourTree
		{
			get
			{
				return nodeGraph as BehaviourTreeInternal;
			}
		}

		internal bool _DragBranchEnable = false;
		private int _DragBranchNodeID = 0;
		private int _DragBranchHoverID = 0;
		private bool _IsDragParentSlot = false;

		protected override bool HasDebugMenu()
		{
			return true;
		}

		void SetBreakPoints()
		{
			Undo.RecordObject(nodeGraph, "BreakPoint On");

			for (int i = 0; i < selectionCount; i++)
			{
				Node node = GetSelectionNode(i);
				TreeBehaviourNode treeBehaviourNode = node as TreeBehaviourNode;
				if (treeBehaviourNode != null)
				{
					treeBehaviourNode.breakPoint = true;

					TreeBehaviourNodeEditor nodeEditor = GetNodeEditorFromID(treeBehaviourNode.nodeID) as TreeBehaviourNodeEditor;
					if (nodeEditor != null)
					{
						nodeEditor.ShowBreakPoint(treeBehaviourNode.breakPoint);
					}
				}
			}

			EditorUtility.SetDirty(nodeGraph);

			RaiseOnChangedNodes();
		}

		void ReleaseBreakPoints()
		{
			Undo.RecordObject(nodeGraph, "BreakPoint Off");

			for (int i = 0; i < selectionCount; i++)
			{
				Node node = GetSelectionNode(i);
				TreeBehaviourNode treeBehaviourNode = node as TreeBehaviourNode;
				if (treeBehaviourNode != null)
				{
					treeBehaviourNode.breakPoint = false;

					TreeBehaviourNodeEditor nodeEditor = GetNodeEditorFromID(treeBehaviourNode.nodeID) as TreeBehaviourNodeEditor;
					if (nodeEditor != null)
					{
						nodeEditor.ShowBreakPoint(treeBehaviourNode.breakPoint);
					}
				}
			}

			EditorUtility.SetDirty(nodeGraph);

			RaiseOnChangedNodes();
		}

		void ReleaseAllBreakPoints()
		{
			Undo.RecordObject(nodeGraph, "Delete All BreakPoint");

			for (int nodeIndex = 0, nodeCount = behaviourTree.nodeCount; nodeIndex < nodeCount; nodeIndex++)
			{
				TreeBehaviourNode treeBehaviourNode = behaviourTree.GetNodeFromIndex(nodeIndex) as TreeBehaviourNode;

				if (treeBehaviourNode != null)
				{
					treeBehaviourNode.breakPoint = false;

					TreeBehaviourNodeEditor nodeEditor = GetNodeEditorFromID(treeBehaviourNode.nodeID) as TreeBehaviourNodeEditor;
					if (nodeEditor != null)
					{
						nodeEditor.ShowBreakPoint(treeBehaviourNode.breakPoint);
					}
				}
			}

			EditorUtility.SetDirty(nodeGraph);

			RaiseOnChangedNodes();
		}

		protected override void OnSetDebugMenu(GenericMenu menu)
		{
			bool isSelectionBehaviourNode = false;
			for (int i = 0; i < selectionCount; i++)
			{
				Node node = GetSelectionNode(i);
				if (node is TreeBehaviourNode)
				{
					isSelectionBehaviourNode = true;
					break;
				}
			}

			bool editable = this.editable;

			if (isSelectionBehaviourNode && editable)
			{
				menu.AddItem(EditorContents.setBreakPoints, false, SetBreakPoints);
				menu.AddItem(EditorContents.releaseBreakPoints, false, ReleaseBreakPoints);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.setBreakPoints);
				menu.AddDisabledItem(EditorContents.releaseBreakPoints);
			}

			if (editable)
			{
				menu.AddItem(EditorContents.releaseAllBreakPoints, false, ReleaseAllBreakPoints);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.releaseAllBreakPoints);
			}
		}

		public override GUIContent GetGraphLabel()
		{
			return EditorContents.behaviourTree;
		}

		public override bool HasPlayState()
		{
			return true;
		}

		public override PlayState GetPlayState()
		{
			return behaviourTree.playState;
		}

		void OnSelectComposite(Vector2 position, System.Type classType)
		{
			CreateComposite(position, classType);
			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
		}

		void CreateComposite(object obj)
		{
			MousePosition mousePosition = (MousePosition)obj;

			Rect buttonRect = new Rect(mousePosition.screenPoint, Vector2.zero);

			CompositeBehaviourMenuWindow.instance.Init(mousePosition.guiPoint, buttonRect, OnSelectComposite, null, null);
		}

		public CompositeNode CreateComposite(Vector2 position, System.Type classType)
		{
			CompositeNode compositeNode = behaviourTree.CreateComposite(EditorGUITools.SnapToGrid(position), classType);

			if (compositeNode != null)
			{
				Undo.RecordObject(behaviourTree, "Created CompositeNode");

				BehaviourInfo behaviourInfo = BehaviourInfoUtility.GetBehaviourInfo(classType);
				compositeNode.name = behaviourInfo.titleContent.text;

				EditorUtility.SetDirty(behaviourTree);

				OnCreatedNode(compositeNode);
			}

			return compositeNode;
		}

		void OnSelectAction(Vector2 position, System.Type classType)
		{
			CreateAction(position, classType);
			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
		}

		void CreateAction(object obj)
		{
			MousePosition mousePosition = (MousePosition)obj;

			Rect buttonRect = new Rect(mousePosition.screenPoint, Vector2.zero);

			ActionBehaviourMenuWindow.instance.Init(mousePosition.guiPoint, buttonRect, OnSelectAction, null, null);
		}

		public ActionNode CreateAction(Vector2 position, System.Type classType)
		{
			ActionNode actionNode = behaviourTree.CreateAction(EditorGUITools.SnapToGrid(position), classType);

			if (actionNode != null)
			{
				Undo.RecordObject(behaviourTree, "Created ActionNode");

				BehaviourInfo behaviourInfo = BehaviourInfoUtility.GetBehaviourInfo(classType);
				actionNode.name = behaviourInfo.titleContent.text;

				EditorUtility.SetDirty(behaviourTree);

				OnCreatedNode(actionNode);
			}

			return actionNode;
		}

		protected override void SetCreateNodeContextMenu(GenericMenu menu, MousePosition mousePosition, bool editable)
		{
			if (editable)
			{
				menu.AddItem(EditorContents.createComposite, false, CreateComposite, mousePosition);
				menu.AddItem(EditorContents.createAction, false, CreateAction, mousePosition);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.createComposite);
				menu.AddDisabledItem(EditorContents.createAction);
			}
		}

		private Dictionary<NodeLinkSlotElement, HighlightElement> _DragHighlights = new Dictionary<NodeLinkSlotElement, HighlightElement>();
		
		private BezierElement _DragBezierElement;

		private BezierElement GetDragBezierElement()
		{
			if (_DragBezierElement == null)
			{
				_DragBezierElement = new BezierElement(graphView.contentContainer)
				{
					pickingMode = UnityEngine.UIElements.PickingMode.Ignore,
					lineColor = dragBezierColor,
					shadow = true,
					shadowColor = bezierShadowColor,
					edgeWidth = 5.0f,
					arrow = false,
					hoverable = false,
				};
			}
			return _DragBezierElement;
		}

		internal void ShowDragBranch()
		{
			var dragBezierElement = GetDragBezierElement();
			graphView.branchOverlayLayer.Add(dragBezierElement);
		}

		public event System.Action<int> onChangedDragBranchHover;

		public void BeginDragBranch(int nodeID, bool isDragParentSlot)
		{
			ShowDragBranch();

			_DragBranchEnable = true;
			_DragBranchNodeID = nodeID;
			_DragBranchHoverID = 0;
			_IsDragParentSlot = isDragParentSlot;

			onChangedDragBranchHover?.Invoke(_DragBranchHoverID);

			graphView.autoScroll = true;

			for (int i = 0; i < nodeEditorCount; i++)
			{
				var nodeEditor = GetNodeEditor(i) as TreeNodeBaseEditor;
				if (nodeEditor == null)
				{
					continue;
				}

				if (_IsDragParentSlot)
				{
					if (nodeEditor.hasChildLinkSlot && IsDragBranchConnectable(nodeEditor.treeNode, false))
					{
						ShowLinkSlotHightlight(nodeEditor._ChildLinkSlotElement);
					}
				}
				else
				{
					if (nodeEditor.hasParentLinkSlot && IsDragBranchConnectable(nodeEditor.treeNode, true))
					{
						ShowLinkSlotHightlight(nodeEditor._ParentLinkSlotElement);
					}
				}
			}
		}

		public bool IsDragBranchConnectable(TreeNodeBase node, bool isParentSlot)
		{
			if (!_DragBranchEnable || _IsDragParentSlot == isParentSlot || _DragBranchNodeID == node.nodeID)
			{
				return false;
			}

			TreeNodeBase draggingNode = behaviourTree.GetNodeFromID(_DragBranchNodeID) as TreeNodeBase;

			TreeNodeBase parentNode = isParentSlot ? draggingNode : node;
			TreeNodeBase childNode = isParentSlot ? node : draggingNode;

			return !behaviourTree.CheckLoop(parentNode, childNode);
		}

		void ShowLinkSlotHightlight(NodeLinkSlotElement slotElement)
		{
			HighlightElement highlightElement = new HighlightElement(graphView)
			{
				style =
				{
					position = Position.Absolute,
				}
			};

			UpdateHighlight(slotElement, highlightElement);

			graphView.highlightLayer.Add(highlightElement);

			_DragHighlights.Add(slotElement, highlightElement);
		}

		void UpdateHighlight(NodeLinkSlotElement slotElement, HighlightElement highlightElement)
		{
			Rect position = graphView.ElementToGraph(slotElement.parent, slotElement.layout);
			highlightElement.position = position;
		}

		internal void UpdateHighlight(NodeLinkSlotElement slotElement)
		{
			if (_DragHighlights.TryGetValue(slotElement, out var highlightElement))
			{
				UpdateHighlight(slotElement, highlightElement);
			}
		}

		public void EndDragBranch()
		{
			if (_DragBezierElement != null)
			{
				_DragBezierElement.RemoveFromHierarchy();
			}

			_DragBranchEnable = false;
			_DragBranchNodeID = 0;
			_DragBranchHoverID = 0;

			onChangedDragBranchHover?.Invoke(_DragBranchHoverID);

			graphView.autoScroll = false;

			foreach (var pair in _DragHighlights)
			{
				pair.Value.RemoveFromHierarchy();
			}
			_DragHighlights.Clear();

			//hostWindow.Repaint();
		}

		public void DragBranchBezie(Bezier2D bezier)
		{
			var dragBezierElement = GetDragBezierElement();
			dragBezierElement.startPosition = bezier.startPosition;
			dragBezierElement.startControl = bezier.startControl;
			dragBezierElement.endPosition = bezier.endPosition;
			dragBezierElement.endControl = bezier.endControl;

			dragBezierElement.UpdateLayout();
		}

		public int GetDragBranchHoverID()
		{
			return _DragBranchHoverID;
		}

		public void DragBranchHoverID(int nodeID)
		{
			if (_DragBranchHoverID != nodeID)
			{
				_DragBranchHoverID = nodeID;
				if (_DragBranchEnable)
				{
					onChangedDragBranchHover?.Invoke(_DragBranchHoverID);
				}
			}
		}

		public bool IsDragBranchHover(Node node)
		{
			return _DragBranchEnable && _DragBranchHoverID == node.nodeID;
		}

		public bool IsDragParentSlot()
		{
			return _IsDragParentSlot;
		}

		public TreeNodeBase GetHoverChildNode(TreeNodeBase node, Vector2 position)
		{
			int nodeCount = nodeGraph.nodeCount;
			for (int nodeIndex = 0; nodeIndex < nodeCount; nodeIndex++)
			{
				TreeNodeBase n = nodeGraph.GetNodeFromIndex(nodeIndex) as TreeNodeBase;

				if (n != null && node != n && n is IParentLinkSlotHolder)
				{
					TreeNodeBaseEditor nodeEditor = GetNodeEditor(n) as TreeNodeBaseEditor;
					if (nodeEditor != null && nodeEditor.parentLinkSlotPosition.Contains(position) && !behaviourTree.CheckLoop(node, n))
					{
						return n;
					}
				}
			}

			return null;
		}

		public TreeNodeBase GetHoverParentNode(TreeNodeBase node, Vector2 position)
		{
			int nodeCount = nodeGraph.nodeCount;
			for (int nodeIndex = 0; nodeIndex < nodeCount; nodeIndex++)
			{
				TreeNodeBase n = nodeGraph.GetNodeFromIndex(nodeIndex) as TreeNodeBase;

				if (n != null && node != n && n is IChildLinkSlotHolder)
				{
					TreeNodeBaseEditor nodeEditor = GetNodeEditor(n) as TreeNodeBaseEditor;
					if (nodeEditor != null && nodeEditor.childLinkSlotPosition.Contains(position) && !behaviourTree.CheckLoop(n, node))
					{
						return n;
					}
				}
			}

			return null;
		}

		internal static readonly Color s_BranchBezierColor = Color.white;
		internal static readonly Color s_BranchBezierActiveColor = new Color(1.0f, 0.5f, 0.0f);
		internal static readonly Color s_BranchBezierInactiveColor = new Color(0.5f, 0.5f, 0.5f);
		internal static readonly Color s_BranchBezierShadowColor = new Color(0, 0, 0, 1.0f);

		private Dictionary<int, NodeBranchElement> _NodeBranchElements = new Dictionary<int, NodeBranchElement>();
		
		void UpdateNodeBranchies()
		{
			GraphView graphView = this.graphView;
			for (int i = graphView.branchUnderlayLayer.childCount - 1; i >= 0; i--)
			{
				NodeBranchElement branchElement = graphView.branchUnderlayLayer[i] as NodeBranchElement;
				if (branchElement != null)
				{
					var branch = branchElement.nodeBranch;
					if (branch == null || !_NodeBranchElements.ContainsKey(branch.branchID) && behaviourTree.nodeBranchies.GetFromID(branch.branchID) != branch)
					{
						branchElement.RemoveFromHierarchy();
					}
				}
			}

			int branchCount = behaviourTree.nodeBranchies.count;
			for (int branchIndex = 0; branchIndex < branchCount; branchIndex++)
			{
				NodeBranch branch = behaviourTree.nodeBranchies[branchIndex];
				if (branch == null)
				{
					continue;
				}

				if (!_NodeBranchElements.TryGetValue(branch.branchID, out var branchElement))
				{
					branchElement = new NodeBranchElement(this);
					graphView.branchUnderlayLayer.Add(branchElement);
					_NodeBranchElements.Add(branch.branchID, branchElement);
				}

				branchElement.Update(branch);
			}

			using (Arbor.Pool.ListPool<int>.Get(out var removeList))
			{
				foreach (var pair in _NodeBranchElements)
				{
					int branchID = pair.Key;
					var branchElement = pair.Value;
					if (behaviourTree.nodeBranchies.GetFromID(pair.Key) == null)
					{
						removeList.Add(branchID);

						branchElement.RemoveFromHierarchy();
					}
				}

				foreach (var branchID in removeList)
				{
					_NodeBranchElements.Remove(branchID);
				}
			}
		}

		Dictionary<int, BezierElement> _MinimapNodeBranchElements = new Dictionary<int, BezierElement>();

		void UpdateMinimapNodeBranchies()
		{
			MinimapView minimapView = this.minimapView;
			for (int i = minimapView.branchLayer.childCount - 1; i >= 0; i--)
			{
				BezierElement branchElement = minimapView.branchLayer[i] as BezierElement;
				if (branchElement != null)
				{
					var branch = branchElement.userData as NodeBranch;
					if (branch == null || !_NodeBranchElements.ContainsKey(branch.branchID) && behaviourTree.nodeBranchies.GetFromID(branch.branchID) != branch)
					{
						branchElement.RemoveFromHierarchy();
					}
				}
			}

			int branchCount = behaviourTree.nodeBranchies.count;
			for (int branchIndex = 0; branchIndex < branchCount; branchIndex++)
			{
				NodeBranch branch = behaviourTree.nodeBranchies[branchIndex];
				if (branch == null)
				{
					continue;
				}

				if (!_MinimapNodeBranchElements.TryGetValue(branch.branchID, out var branchElement))
				{
					branchElement = new BezierElement(minimapView.contentContainer)
					{
						edgeWidth = 4f,
					};
					branchElement.AddManipulator(new MinimapTransformManipulator(minimapView, () =>
					{
						UpdateMinimapNodeBranch(branchElement, branchElement.userData as NodeBranch);
					}));
					minimapView.branchLayer.Add(branchElement);
					_MinimapNodeBranchElements.Add(branch.branchID, branchElement);
				}

				branchElement.userData = branch;
				UpdateMinimapNodeBranch(branchElement, branch);
			}

			List<int> removeList = new List<int>();
			foreach (var pair in _MinimapNodeBranchElements)
			{
				int branchID = pair.Key;
				var branchElement = pair.Value;
				if (behaviourTree.nodeBranchies.GetFromID(pair.Key) == null)
				{
					removeList.Add(branchID);

					branchElement.RemoveFromHierarchy();
				}
			}

			foreach (var branchID in removeList)
			{
				_MinimapNodeBranchElements.Remove(branchID);
			}
		}

		void UpdateMinimapNodeBranch(BezierElement branchElement, NodeBranch branch)
		{
			Color color = s_BranchBezierColor;
			if (Application.isPlaying)
			{
				if (branch.isActive)
				{
					color = s_BranchBezierActiveColor;
				}
				else
				{
					color = s_BranchBezierInactiveColor;
				}
			}

			Bezier2D bezier = minimapView.GraphToMinimap(branch.bezier);

			branchElement.startPosition = bezier.startPosition;
			branchElement.startControl = bezier.startControl;
			branchElement.endPosition = bezier.endPosition;
			branchElement.endControl = bezier.endControl;

			branchElement.UpdateLayout();
		}
		
		protected override void OnUpdate()
		{
			UpdateNodeBranchies();
			UpdateMinimapNodeBranchies();
		}

		protected override void OnFinalizeGraph()
		{
			foreach (var pair in _NodeBranchElements)
			{
				pair.Value.RemoveFromHierarchy();
			}
			_NodeBranchElements.Clear();

			foreach (var pair in _MinimapNodeBranchElements)
			{
				pair.Value.RemoveFromHierarchy();
			}
			_MinimapNodeBranchElements.Clear();
		}

		public override bool IsDraggingBranch(Node node)
		{
			return base.IsDraggingBranch(node) ||
				_DragBranchEnable && _DragBranchNodeID == node.nodeID;
		}

		public override bool IsDragBranch()
		{
			return base.IsDragBranch() || _DragBranchEnable;
		}

		protected override void OnDragNodes()
		{
			behaviourTree.CalculatePriority();

			RaiseOnChangedNodes();
		}

		internal static int InternalNodeListSortComparison(NodeEditor a, NodeEditor b)
		{
			TreeNodeBaseEditor treeNodeA = a as TreeNodeBaseEditor;
			TreeNodeBaseEditor treeNodeB = b as TreeNodeBaseEditor;
			if (treeNodeA == null || treeNodeB == null)
			{
				return NodeListElement.Defaults.SortComparison(a, b);
			}

			bool enablePriorityA = treeNodeA.treeNode.enablePriority;
			bool enablePriorityB = treeNodeB.treeNode.enablePriority;
			if (enablePriorityA != enablePriorityB)
			{
				return enablePriorityB.CompareTo(enablePriorityA);
			}
			if (!enablePriorityA && !enablePriorityB)
			{
				bool isActionA = treeNodeA.treeNode is ActionNode;
				bool isActionB = treeNodeB.treeNode is ActionNode;
				if (isActionA != isActionB)
				{
					return isActionA.CompareTo(isActionB);
				}
				return NodeListElement.Defaults.SortComparison(a, b);
			}

			return treeNodeA.treeNode.priority.CompareTo(treeNodeB.treeNode.priority);
		}

		protected override int NodeListSortComparison(NodeEditor a, NodeEditor b)
		{
			return InternalNodeListSortComparison(a, b);
		}

		protected override Node GetActiveNode()
		{
			return behaviourTree.currentNode;
		}

		protected override void OnCreateSetParameter(Vector2 position, Parameter parameter)
		{
			Undo.IncrementCurrentGroup();
			int undoGroup = Undo.GetCurrentGroup();

			ActionNode actionNode = CreateAction(position, Types.SetParameterActionType);

			Arbor.ParameterBehaviours.SetParameterActionInternal setParameterAction = actionNode.GetBehaviourObject() as Arbor.ParameterBehaviours.SetParameterActionInternal;

			Undo.RecordObject(setParameterAction, "Created ActionNode");

			setParameterAction.SetParameter(parameter);

			Undo.CollapseUndoOperations(undoGroup);

			EditorUtility.SetDirty(setParameterAction);
			EditorUtility.SetDirty(nodeGraph);
		}
	}
}