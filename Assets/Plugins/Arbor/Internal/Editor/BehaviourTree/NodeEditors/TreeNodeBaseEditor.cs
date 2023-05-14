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
	using ArborEditor.UIElements;
	using ArborEditor.BehaviourTree.UIElements;

	public abstract class TreeNodeBaseEditor : NodeEditor
	{
		private const Styles.Color kDragColor = Styles.Color.Red;
		private const Styles.Color kCurrentColor = Styles.Color.Orange;
		private const Styles.Color kNormalColor = Styles.Color.Gray;

		public BehaviourTreeGraphEditor behaviourTreeGraphEditor
		{
			get
			{
				return graphEditor as BehaviourTreeGraphEditor;
			}
		}

		public TreeNodeBase treeNode
		{
			get
			{
				return node as TreeNodeBase;
			}
		}

		internal NodeLinkSlotElement _ParentLinkSlotElement;

		public bool hasParentLinkSlot
		{
			get
			{
				return _ParentLinkSlotElement != null;
			}
		}

		public Rect parentLinkSlotPosition
		{
			get
			{
				if (_ParentLinkSlotElement != null)
				{
					return graphEditor.graphView.ElementToGraph(_ParentLinkSlotElement.parent, _ParentLinkSlotElement.layout);
				}
				return Rect.zero;
			}
		}

		internal NodeLinkSlotElement _ChildLinkSlotElement;

		public bool hasChildLinkSlot
		{
			get
			{
				return _ChildLinkSlotElement != null;
			}
		}

		public Rect childLinkSlotPosition
		{
			get
			{
				if (_ChildLinkSlotElement != null)
				{
					return graphEditor.graphView.ElementToGraph(_ChildLinkSlotElement.parent, _ChildLinkSlotElement.layout);
				}
				return Rect.zero;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			isRenamable = true;
		}

		protected override void OnAttachNode(Node node)
		{
			base.OnAttachNode(node);

			var treeNode = node as TreeNodeBase;
			treeNode.onChangedPriority += OnChangedPriority;
		}

		protected override void OnDetachNode(Node node)
		{
			base.OnDetachNode(node);

			var treeNode = node as TreeNodeBase;
			treeNode.onChangedPriority -= OnChangedPriority;
		}

		protected override void RegisterCallbackOnElement()
		{
			base.RegisterCallbackOnElement();

			nodeElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);

			OnChangedPriority();
		}

		void OnChangedPriority()
		{
			if (treeNode.enablePriority)
			{
				if (_PriorityCountElement == null)
				{
					_PriorityCountElement = new CountBadgeElement();
				}

				if (_PriorityCountElement.parent == null)
				{
					nodeElement.overlayLayer.Add(_PriorityCountElement);
					UpdatePriorityPosition();
				}

				_PriorityCountElement.count = treeNode.priority;
			}
			else
			{
				if (_PriorityCountElement != null && _PriorityCountElement.parent != null)
				{
					_PriorityCountElement.RemoveFromHierarchy();
				}
			}
		}

		void OnGeometryChanged(GeometryChangedEvent e)
		{
			UpdatePriorityPosition();
		}

		void UpdatePriorityPosition()
		{
			if (_PriorityCountElement != null)
			{
				_PriorityCountElement.attachPoint = new Vector2(nodeElement.layout.width, 0f);
			}
		}

		public override bool IsActive()
		{
			return treeNode.isActive;
		}

		protected virtual Styles.Color GetNormalStyleColor()
		{
			return kNormalColor;
		}

		public override Styles.Color GetStyleColor()
		{
			BehaviourTreeGraphEditor behaviourTreeGraphEditor = graphEditor as BehaviourTreeGraphEditor;
			if (behaviourTreeGraphEditor != null && behaviourTreeGraphEditor.IsDragBranchHover(node))
			{
				return kDragColor;
			}
			else if (IsActive())
			{
				return kCurrentColor;
			}

			return GetNormalStyleColor();
		}

		public override bool IsShowNodeList()
		{
			return true;
		}

		public override void OnBindListElement(VisualElement element)
		{
			base.OnBindListElement(element);

			var userContentContainer = element.Q(NodeListElement.itemContentContainerUssClassName);
			var priorityElement = userContentContainer.Q<NodeListPriorityElement>(NodeListPriorityElement.ussClassName);
			if (priorityElement == null)
			{
				priorityElement = new NodeListPriorityElement()
				{
					name = NodeListPriorityElement.ussClassName,
				};
			}

			priorityElement.nodeEditor = this;
			userContentContainer.Clear();
			userContentContainer.Add(priorityElement);
		}

		private CountBadgeElement _PriorityCountElement;

		protected void RegisterDragChild(Node childNode)
		{
			if (childNode != null)
			{
				graphEditor.RegisterDragNode(childNode);

				TreeNodeBaseEditor childNodeEditor = graphEditor.GetNodeEditor(childNode) as TreeNodeBaseEditor;
				if (childNodeEditor != null)
				{
					childNodeEditor.RegisterDragChildren();
				}
			}
		}

		protected void RegisterDragChild(NodeBranch branch)
		{
			if (branch == null)
			{
				return;
			}

			BehaviourTreeInternal behaviourTree = graphEditor.nodeGraph as BehaviourTreeInternal;
			Node childNode = behaviourTree.GetNodeFromID(branch.childNodeID);
			RegisterDragChild(childNode);
		}

		protected void RegisterDragChild(int childBranchID)
		{
			BehaviourTreeInternal behaviourTree = graphEditor.nodeGraph as BehaviourTreeInternal;

			NodeBranch branch = behaviourTree.nodeBranchies.GetFromID(childBranchID);
			RegisterDragChild(branch);
		}

		public virtual void RegisterDragChildren()
		{
		}

		public override void OnBeginDrag(bool altKey)
		{
			base.OnBeginDrag(altKey);

			if (altKey)
			{
				RegisterDragChildren();
			}
		}

		sealed class NodeListPriorityElement : TextElement
		{
			public static readonly new string ussClassName = "node-list-priority";
			private TreeNodeBaseEditor _NodeEditor;
			public TreeNodeBaseEditor nodeEditor
			{
				get
				{
					return _NodeEditor;
				}
				set
				{
					if (_NodeEditor != value)
					{
						if (_NodeEditor != null)
						{
							UnregisterCallbackFromNodeEditor();
						}

						_NodeEditor = value;

						if (_NodeEditor != null)
						{
							RegisterCallbackToNodeEditor();
						}
					}
				}
			}

			public NodeListPriorityElement()
			{
				AddToClassList("count-badge");
				AddToClassList(ussClassName);

				RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
				RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
			}

			void OnAttachToPanel(AttachToPanelEvent e)
			{
				if (_NodeEditor != null)
				{
					RegisterCallbackToNodeEditor();

					UpdatePriority();
				}
			}

			void OnDetachFromPanel(DetachFromPanelEvent e)
			{
				if (_NodeEditor != null)
				{
					UnregisterCallbackFromNodeEditor();
				}
			}

			void RegisterCallbackToNodeEditor()
			{
				_NodeEditor.treeNode.onChangedPriority += UpdatePriority;
			}

			void UnregisterCallbackFromNodeEditor()
			{
				_NodeEditor.treeNode.onChangedPriority -= UpdatePriority;
			}

			void UpdatePriority()
			{
				var treeNode = _NodeEditor.treeNode;
				if (treeNode.enablePriority)
				{
					style.display = DisplayStyle.Flex;

					int transitionCount = treeNode.priority;
					text = transitionCount.ToString();
				}
				else
				{
					style.display = DisplayStyle.None;
				}
			}
		}
	}
}