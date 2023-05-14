//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor.UIElements
{
	using Arbor;

	internal sealed class NodeListElement : VisualElement, IPropertyChanged
	{
		private static readonly string s_ListViewName = "arbor-node-list__list-view";
		public static readonly string ussClassName = "arbor-node-list";
		public static readonly string itemUssClassName = ussClassName + "__item";
		public static readonly string itemIconUssClassName = ussClassName + "__item-icon";
		public static readonly string itemLabelUssClassName = ussClassName + "__item-label";
		public static readonly string itemContentContainerUssClassName = ussClassName + "__item-content";
		public static readonly string itemOverlayClassName = ussClassName + "__item-overlay";
		public static readonly string itemPreStatusUssClassName = ussClassName + "__item-pre-status";

		public enum SearchMode
		{
			All,
			Name,
			Type
		};

		[Serializable]
		public sealed class SearchState
		{
			public SearchMode searchMode = SearchMode.All;
			public string searchText = "";
		}

		public Func<NodeEditor, bool> isShowCallback;
		public Comparison<NodeEditor> sortComparisonCallback;
		public Action<IEnumerable<NodeEditor>> onSelectionChange;
		public Func<NodeEditor, bool> isSelectionCallback;

		private ListViewElement _ListView;

		private NodeGraphEditor _GraphEditor;

		private SearchState _SearchState = null;
		public SearchState searchState
		{
			get
			{
				if (_SearchState == null)
				{
					_SearchState = new SearchState();
				}
				return _SearchState;
			}
			set
			{
				if (_SearchState != value)
				{
					_SearchState = value;
				}
			}
		}

		public NodeGraphEditor graphEditor
		{
			get
			{
				return _GraphEditor;
			}
			set
			{
				if (_GraphEditor != value)
				{
					UnregisterCallbackFromGraphEditor();

					_GraphEditor = value;

					RegisterCallbackOnGraphEditor();

					if (_GraphEditor != null)
					{
						RebuildViewNodes();
					}
				}
			}
		}

		public NodeGraph nodeGraph
		{
			get
			{
				return _GraphEditor != null ? _GraphEditor.nodeGraph : null;
			}
		}

		private List<NodeEditor> _ViewNodes = new List<NodeEditor>();
		public List<NodeEditor> viewNodes
		{
			get
			{
				return _ViewNodes;
			}
		}

		public SelectionType selectionType
		{
			get
			{
				return _ListView.selectionType;
			}
			set
			{
				_ListView.selectionType = value;
			}
		}

		public NodeListElement()
		{
			name = ussClassName;
			viewDataKey = ussClassName;
			AddToClassList(ussClassName);

			var searchBarGUI = new IMGUIContainer(OnSearchBarGUI);
			hierarchy.Add(searchBarGUI);

			_ListView = new ListViewElement(_ViewNodes, 26, MakeItem, BindItem);
			_ListView.name = s_ListViewName;
			_ListView.viewDataKey = s_ListViewName;
			_ListView.AddToClassList(s_ListViewName);
			_ListView.onAfterDeserialize += OnAfterDeserialize;

			_ListView.RegisterCallbackSelectionChange(OnSelectionChange);

			hierarchy.Add(_ListView);

			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
		}

		void OnSearchBarGUI()
		{
			SearchState searchState = this.searchState;

			Rect searchRect = GUILayoutUtility.GetRect(0.0f, 23.0f);
			searchRect.y += 4f;
			searchRect.x += 8f;
			searchRect.width -= 16f;

			string[] names = Enum.GetNames(typeof(SearchMode));
			int searchMode = (int)searchState.searchMode;
			EditorGUI.BeginChangeCheck();
			searchState.searchText = UnityEditorBridge.EditorGUIBridge.ToolbarSearchField(searchRect, names, ref searchMode, searchState.searchText);
			searchState.searchMode = (SearchMode)searchMode;
			if (EditorGUI.EndChangeCheck())
			{
				RebuildViewNodes();
			}
		}

		void OnAfterDeserialize()
		{
			if (_GraphEditor == null)
			{
				return;
			}

			UpdateSelection();
		}

		void OnAttachToPanel(AttachToPanelEvent e)
		{
			EditorCallbackUtility.RegisterPropertyChanged(this);
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			EditorCallbackUtility.UnregisterPropertyChanged(this);
		}

		VisualElement MakeItem()
		{
			var itemContainer = new ItemElement()
			{
				name = itemUssClassName,
			};
			itemContainer.AddToClassList(itemUssClassName);
			itemContainer.AddToClassList("node-element");

			var itemPreStatus = new VisualElement()
			{
				name = itemPreStatusUssClassName,
			};
			itemPreStatus.AddToClassList(itemPreStatusUssClassName);
			itemContainer.hierarchy.Add(itemPreStatus);

			Image icon = new Image()
			{
				name = itemIconUssClassName,
				scaleMode = ScaleMode.ScaleToFit,
			};
			icon.AddToClassList(itemIconUssClassName);
			itemContainer.hierarchy.Add(icon);

			TextElement label = new TextElement()
			{
				name = itemLabelUssClassName,
			};
			label.AddToClassList(itemLabelUssClassName);
			itemContainer.hierarchy.Add(label);

			var userContentContainer = new VisualElement()
			{
				name = itemContentContainerUssClassName,
			};
			userContentContainer.AddToClassList(itemContentContainerUssClassName);
			itemContainer.hierarchy.Add(userContentContainer);

			var overlay = new VisualElement()
			{
				pickingMode = PickingMode.Ignore,
				name = itemOverlayClassName,
			};
			overlay.AddToClassList(itemOverlayClassName);
			overlay.StretchToParentSize();
			itemContainer.hierarchy.Add(overlay);

			return itemContainer;
		}

		void BindItem(VisualElement element, int index)
		{
			var nodeEditor = _ViewNodes[index];

			var itemElement = element as ItemElement;
			itemElement.nodeEditor = nodeEditor;

			var label = element.Q<TextElement>(itemLabelUssClassName);
			label.text = nodeEditor.GetTitle();

			nodeEditor.OnBindListElement(element);
		}

		sealed class ItemElement : VisualElement
		{
			private NodeEditor _NodeEditor;

			public NodeEditor nodeEditor
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

							OnStyleChanged();
						}
					}
				}
			}

			public ItemElement()
			{
				RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
				RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
			}

			void OnAttachToPanel(AttachToPanelEvent e)
			{
				if (_NodeEditor != null)
				{
					RegisterCallbackToNodeEditor();
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
				_NodeEditor.onStyleChanged += OnStyleChanged;
			}

			void UnregisterCallbackFromNodeEditor()
			{
				_NodeEditor.onStyleChanged -= OnStyleChanged;
			}

			void OnStyleChanged()
			{
				style.unityBackgroundImageTintColor = _NodeEditor.GetListColor();

				var icon = this.Q<Image>(itemIconUssClassName);
				icon.image = _NodeEditor.GetIcon();
			}
		}

		public void AddToSelection(NodeEditor nodeEditor)
		{
			var index = _ViewNodes.IndexOf(nodeEditor);
			if (index < 0)
			{
				return;
			}

			var onSelectionChange = this.onSelectionChange;
			this.onSelectionChange = null;

			_ListView.AddToSelection(index);

			this.onSelectionChange = onSelectionChange;
		}

		public void RemoveFromSelection(NodeEditor nodeEditor)
		{
			var index = _ViewNodes.IndexOf(nodeEditor);
			if (index < 0)
			{
				return;
			}

			var onSelectionChange = this.onSelectionChange;
			this.onSelectionChange = null;

			_ListView.RemoveFromSelection(index);

			this.onSelectionChange = onSelectionChange;
		}

		public void ClearSelection()
		{
			var onSelectionChange = this.onSelectionChange;
			this.onSelectionChange = null;

			_ListView.ClearSelection();

			this.onSelectionChange = onSelectionChange;
		}

		void OnSelectionChange(IEnumerable<object> items)
		{
			onSelectionChange?.Invoke(items.Cast<NodeEditor>());
		}

		void RegisterCallbackOnGraphEditor()
		{
			if (_GraphEditor != null)
			{
				_GraphEditor.onChangedGraph += OnChangedGraph;
				_GraphEditor.onChangedNodes += OnChangedNodes;
			}
		}

		void UnregisterCallbackFromGraphEditor()
		{
			if (_GraphEditor != null)
			{
				_GraphEditor.onChangedGraph -= OnChangedGraph;
				_GraphEditor.onChangedNodes -= OnChangedNodes;

				_GraphEditor = null;
			}
		}

		void OnChangedGraph()
		{
			RebuildViewNodes();
		}

		void OnChangedNodes()
		{
			RebuildViewNodes();
		}

		void IPropertyChanged.OnPropertyChanged(PropertyChangedType propertyChangedType)
		{
			if (propertyChangedType != PropertyChangedType.UndoRedoPerformed)
			{
				return;
			}

			RebuildViewNodes();
		}

		public void ListViewRefresh()
		{
			_ListView.RebuildList();
		}

		public void RebuildViewNodes()
		{
			int nodeCount = 0;

			var graphEditor = this.graphEditor;
			var nodeGraph = graphEditor != null? graphEditor.nodeGraph : null;

			if (nodeGraph != null)
			{
				nodeCount = nodeGraph.nodeCount;
			}

			_ViewNodes.Clear();

			SearchState searchState = this.searchState;

			for (int i = 0; i < nodeCount; i++)
			{
				Node node = nodeGraph.GetNodeFromIndex(i);
				NodeEditor nodeEditor = graphEditor.GetNodeEditor(node);

				if (nodeEditor == null || !nodeEditor.IsShowNodeList())
				{
					continue;
				}

				if (isShowCallback != null && !isShowCallback(nodeEditor))
				{
					continue;
				}

				if (!string.IsNullOrEmpty(searchState.searchText))
				{
					string nodeName = nodeEditor.GetTitle();

					switch (searchState.searchMode)
					{
						case SearchMode.All:
							if (nodeName.IndexOf(searchState.searchText, StringComparison.OrdinalIgnoreCase) >= 0)
							{
								_ViewNodes.Add(nodeEditor);
							}
							else
							{
								INodeBehaviourContainer nodeBehaviours = node as INodeBehaviourContainer;
								if (nodeBehaviours != null)
								{
									int behaviourCount = nodeBehaviours.GetNodeBehaviourCount();
									for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
									{
										NodeBehaviour behaviour = nodeBehaviours.GetNodeBehaviour<NodeBehaviour>(behaviourIndex);

										if (behaviour.GetType().Name.Equals(searchState.searchText, StringComparison.OrdinalIgnoreCase))
										{
											_ViewNodes.Add(nodeEditor);
											break;
										}
									}
								}
							}
							break;
						case SearchMode.Name:
							if (nodeName.IndexOf(searchState.searchText, StringComparison.OrdinalIgnoreCase) >= 0)
							{
								_ViewNodes.Add(nodeEditor);
							}
							break;
						case SearchMode.Type:
							{
								INodeBehaviourContainer nodeBehaviours = node as INodeBehaviourContainer;
								if (nodeBehaviours != null)
								{
									int behaviourCount = nodeBehaviours.GetNodeBehaviourCount();
									for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
									{
										NodeBehaviour behaviour = nodeBehaviours.GetNodeBehaviour<NodeBehaviour>(behaviourIndex);

										if (behaviour.GetType().Name.Equals(searchState.searchText, StringComparison.OrdinalIgnoreCase))
										{
											_ViewNodes.Add(nodeEditor);
											break;
										}
									}
								}
							}
							break;
					}
				}
				else
				{
					_ViewNodes.Add(nodeEditor);
				}
			}

			if (sortComparisonCallback != null)
			{
				_ViewNodes.Sort(sortComparisonCallback);
			}
			else
			{
				_ViewNodes.Sort(Defaults.SortComparison);
			}

			ListViewRefresh();

			UpdateSelection();
		}

		void UpdateSelection()
		{
			var onSelectionChange = this.onSelectionChange;
			this.onSelectionChange = null;

			_ListView.ClearSelection();

			if (graphEditor != null)
			{
				for (int i = 0; i < _ViewNodes.Count; i++)
				{
					var nodeEditor = _ViewNodes[i];

					bool isSelection = (isSelectionCallback!= null)? isSelectionCallback(nodeEditor) : Defaults.IsSelection(nodeEditor);
					if (isSelection)
					{
						_ListView.AddToSelection(i);
					}
				}
			}

			this.onSelectionChange = onSelectionChange;
		}

		public static class Defaults
		{
			public static int SortComparison(NodeEditor a, NodeEditor b)
			{
				return a.GetTitle().CompareTo(b.GetTitle());
			}

			public static bool IsSelection(NodeEditor a)
			{
				return a.isSelection;
			}
		}
	}
}