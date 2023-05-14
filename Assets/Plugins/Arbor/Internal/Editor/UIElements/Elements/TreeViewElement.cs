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
	using ArborEditor.IMGUI.Controls;
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	internal sealed class TreeViewElement : VisualElement
	{
		private static readonly string s_ListViewName = "arbor-tree-view__list-view";
		public static readonly string ussClassName = "arbor-tree-view";
		public static readonly string itemUssClassName = ussClassName + "__item";
		public static readonly string itemToggleUssClassName = ussClassName + "__item-toggle";
		public static readonly string itemIconUssClassName = ussClassName + "__item-icon";
		public static readonly string itemLabelUssClassName = ussClassName + "__item-label";
		public static readonly string itemIndentsContainerUssClassName = ussClassName + "__item-indents";
		public static readonly string itemIndentUssClassName = ussClassName + "__item-indent";
		public static readonly string itemContentContainerUssClassName = ussClassName + "__item-content";

		private TreeView _TreeView;
		private List<TreeViewItem> _ViewItems = new List<TreeViewItem>();
		private ListViewElement _ListView;
		private ScrollView _ScrollView;
		
		public TreeViewState state
		{
			get;
			private set;
		}

		Func<VisualElement> _MakeItem;
		public Func<VisualElement> makeItem
		{
			get
			{
				return _MakeItem;
			}
			set
			{
				if (_MakeItem != value)
				{
					_MakeItem = value;
					ListViewRefresh();
				}
			}
		}

		Action<VisualElement, TreeViewItem> _BindItem;
		public Action<VisualElement, TreeViewItem> bindItem
		{
			get
			{
				return _BindItem;
			}
			set
			{
				if (_BindItem != value)
				{
					_BindItem = value;
					ListViewRefresh();
				}
			}
		}

		public TreeViewElement(TreeView treeView, TreeViewState state, Func<VisualElement> makeItem, Action<VisualElement, TreeViewItem> bindItem)
		{
			name = ussClassName;
			viewDataKey = ussClassName;
			AddToClassList(ussClassName);

			_TreeView = treeView;
			this.state = state;

			_MakeItem = makeItem;
			_BindItem = bindItem;

			_ListView = new ListViewElement(_ViewItems, (int)EditorGUIUtility.singleLineHeight, MakeTreeItem, BindTreeItem);
			_ListView.name = s_ListViewName;
			_ListView.viewDataKey = s_ListViewName;
			_ListView.AddToClassList(s_ListViewName);
			_ListView.onAfterDeserialize += UpdateSelection;

			_ListView.RegisterCallbackSelectionChange(OnSelectionChange);
			_ListView.RegisterCallbackItemsChosen(OnItemsChosen);

			hierarchy.Add(_ListView);

			_ScrollView = _ListView.Q<ScrollView>();
			_ScrollView.RegisterCallback<KeyDownEvent>(OnKeyDown);

			RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
			RegisterCallback<WheelEvent>(OnWheel, TrickleDown.TrickleDown);
		}

		void OnItemsChosen(IEnumerable<object> items)
		{
			TreeViewItem treeViewitem = items.FirstOrDefault() as TreeViewItem;
			if (treeViewitem != null)
			{
				SubmitItem(treeViewitem);
			}
		}

		public new void Focus()
		{
			_ListView.Focus();
		}

		private bool _IsLayouted = false;

		internal void ResetLayouted()
		{
			_IsLayouted = false;
		}

		void OnGeometryChanged(GeometryChangedEvent e)
		{
			_IsLayouted = true;
			if (_ScrollToItem != null)
			{
				var item = _ScrollToItem;
				_ScrollToItem = null;

				ScrollToItem(item);
			}
		}

		void OnWheel(WheelEvent e)
		{
			if (_RenameOverlay == null || !_RenameOverlay.IsRenaming())
			{
				return;
			}

			e.StopPropagation();
		}

		void OnKeyDown(KeyDownEvent evt)
		{
			switch (evt.keyCode)
			{
				case KeyCode.Return:
				case KeyCode.KeypadEnter:
					{
						if (Application.platform == RuntimePlatform.OSXEditor)
						{
							BeginRenameEditing();
						}

						evt.StopPropagation();
					}
					break;
				case KeyCode.F2:
					{
						if (Application.platform != RuntimePlatform.OSXEditor)
						{
							BeginRenameEditing();
						}

						evt.StopPropagation();
					}
					break;
				case KeyCode.LeftArrow:
					{
						SelectParent();
						evt.StopPropagation();
					}
					break;
				case KeyCode.RightArrow:
					{
						SelectChild();
						evt.StopPropagation();
					}
					break;
			}
		}

		void SelectParent()
		{
			var index = _ListView.selectedIndex;
			if (index < 0)
			{
				return;
			}

			var selectedItem = _ViewItems[index];
			if (selectedItem == null)
			{
				return;
			}

			if (selectedItem.children.Count > 0 && state.IsExpanded(selectedItem))
			{
				if (state.SetExpanded(selectedItem.id, false))
				{
					UpdateViewTree();
				}
			}
			else
			{
				TreeViewItem parent = selectedItem.parent;
				if (parent != null)
				{
					if (parent.parent != null)
					{
						SetSelectedItem(parent, true);
					}
					else
					{
						int prevIndex = parent.children.IndexOf(selectedItem) - 1;
						if (prevIndex >= 0)
						{
							var prevItem = parent.children[prevIndex];
							SetSelectedItem(prevItem, true);
						}
					}
				}
			}
		}

		void SelectChild()
		{
			var index = _ListView.selectedIndex;
			if (index < 0)
			{
				return;
			}

			var selectedItem = _ViewItems[index];
			if (selectedItem == null)
			{
				return;
			}

			if (selectedItem.children.Count > 0 && !state.IsExpanded(selectedItem))
			{
				if (state.SetExpanded(selectedItem.id, true))
				{
					UpdateViewTree();
				}
			}
			else
			{
				bool find = false;

				for (int i = 0, count = selectedItem.children.Count; i < count; ++i)
				{
					TreeViewItem item = selectedItem.children[i];
					if (item.children.Count > 0)
					{
						SetSelectedItem(item, true);

						find = true;
						break;
					}
				}

				if (!find)
				{
					TreeViewItem currentItem = selectedItem;
					while (currentItem != null && currentItem.parent != null)
					{
						TreeViewItem parent = currentItem.parent;
						int nextIndex = parent.children.IndexOf(currentItem) + 1;
						for (int i = nextIndex, count = parent.children.Count; i < count; ++i)
						{
							TreeViewItem item = parent.children[i];
							if (item.children.Count > 0)
							{
								SetSelectedItem(item, true);

								find = true;
								break;
							}
						}

						if (find)
						{
							break;
						}

						currentItem = parent;
					}
				}
			}
		}

		public bool selectSubmit = false;
		public event Action<TreeViewItem> onSubmit;
		public event Action<GenericMenu, TreeViewItem> contextClickItemCallback;
		public event Action<string, int> onRenameEnded;

		public void SetSelectedItem(TreeViewItem item, bool scrollTo)
		{
			var index = _ViewItems.IndexOf(item);
			_ListView.selectedIndex = index;

			if (scrollTo)
			{
				_ListView.ScrollToItem(index);
			}
		}

		private TreeViewItem _ScrollToItem;

		public void ScrollToItem(TreeViewItem item)
		{
			if (item == null)
			{
				return;
			}

			if (_IsLayouted)
			{
				int index = _ViewItems.IndexOf(item);
				if (index >= 0)
				{
					_ListView.ScrollToItem(index);
				}
			}
			else
			{
				_ScrollToItem = item;
			}
		}

		private bool SubmitItem(TreeViewItem item)
		{
			TreeViewSearchItem searchItemElement = item as TreeViewSearchItem;
			if (searchItemElement != null)
			{
				if (searchItemElement.disable)
				{
					return false;
				}

				item = searchItemElement.original;
			}

			if (item != null && !item.disable)
			{
				onSubmit?.Invoke(item);

				return true;
			}

			return false;
		}

		void OnSelectionChange(IEnumerable<object> items)
		{
			var treeViewItems = items.Cast<TreeViewItem>();
			state.SetSelection(treeViewItems);

			if (treeViewItems.Count() > 0 && selectSubmit)
			{
				SubmitItem(treeViewItems.ElementAt(0));
			}
		}

		private void ToggleExpandedState(ChangeEvent<bool> evt)
		{
			var toggle = evt.target as Toggle;
			var id = (int)toggle.userData;
			var isExpanded = state.IsExpanded(id);

			if (state.SetExpanded(id, !isExpanded))
			{
				UpdateViewTree();
			}

			// To make sure our TreeView gets focus, we need to force this. :(
			_ScrollView.contentContainer.Focus();
		}

		public bool renamable = false;

		RenameOverlayElement _RenameOverlay;

		private RenameOverlayElement GetRenameOverlay()
		{
			if (_RenameOverlay == null)
			{
				_RenameOverlay = new RenameOverlayElement(OnRenameEnded)
				{
					focusAfterComfirm = _ScrollView.contentContainer,
				};
			}
			return _RenameOverlay;
		}

		bool BeginNameEditing(TreeViewItem item, VisualElement itemContainer, float delay)
		{
			if (!renamable || item == null ||
				!item.renamable || item.disable)
			{
				return false;
			}

			var renameOverlay = GetRenameOverlay();
			renameOverlay.BeginRename(item.displayName, item.id, delay);

			if (itemContainer != null)
			{
				if (renameOverlay.parent != itemContainer)
				{
					itemContainer.Add(renameOverlay);
				}

				VisualElement label = itemContainer.Q<TextElement>(itemLabelUssClassName);
				renameOverlay.attachTarget = label;
			}
			
			var index = _ViewItems.IndexOf(item);
			if (index >= 0)
			{
				_ListView.ScrollToItem(index);
			}
			
			return true;
		}

		VisualElement GetItemElement(TreeViewItem target)
		{
			foreach (var itemContainer in _ScrollView.Children())
			{
				if (!itemContainer.visible)
				{
					continue;
				}

				var item = itemContainer.userData as TreeViewItem;
				if (target == item)
				{
					return itemContainer;
				}
			}

			return null;
		}

		bool BeginRenameEditing()
		{
			var index = _ListView.selectedIndex;
			if (index < 0)
			{
				return false;
			}

			var selectedItem = _ViewItems[index];
			VisualElement itemContainer = GetItemElement(selectedItem);
			return BeginNameEditing(selectedItem, itemContainer, 0f);
		}

		void OnRenameEnded(string name, int userData)
		{
			onRenameEnded?.Invoke(name, userData);
		}

		void OnContextClickItem(ContextClickEvent e)
		{
			var itemContainer = e.currentTarget as VisualElement;
			var item = itemContainer.userData as TreeViewItem;

			if (item != null)
			{
				GenericMenu menu = new GenericMenu();

				if (renamable)
				{
					if (item.renamable)
					{
						menu.AddItem(EditorContents.rename, false, () =>
						{
							BeginNameEditing(item, itemContainer, 0f);
						});
					}
					else
					{
						menu.AddDisabledItem(EditorContents.rename);
					}
				}

				contextClickItemCallback?.Invoke(menu, item);
				
				if (menu.GetItemCount() > 0)
				{
					menu.ShowAsContext();
				}
			}

			e.StopPropagation();
		}

		private bool _AllowRenameOnMouseUp = false;

		void OnMouseDownItem(MouseDownEvent e)
		{
			if(e.button != 0 || e.clickCount == 2)
			{
				return;
			}

			var element = e.currentTarget as VisualElement;
			var item = element.userData as TreeViewItem;

			if (!selectSubmit)
			{
				SubmitItem(item);
			}

			if (!renamable)
			{
				return;
			}

			if (item == null || !item.renamable)
			{
				return;
			}

			var selectedItem = _ListView.selectedItem as TreeViewItem;

			_AllowRenameOnMouseUp = item == selectedItem;

			if (_AllowRenameOnMouseUp)
			{
				element.CaptureMouse();
				e.StopPropagation();
			}
		}

		void OnMouseUpItem(MouseUpEvent e)
		{
			if (!_AllowRenameOnMouseUp)
			{
				return;
			}

			VisualElement element = e.currentTarget as VisualElement;

			var item = element.userData as TreeViewItem;
			var selectedItem = _ListView.selectedItem as TreeViewItem;

			if (selectedItem == item)
			{
				var label = element.Q<TextElement>(itemLabelUssClassName);
				var mousePosition = element.ChangeCoordinatesTo(label, e.localMousePosition);

				if (label.ContainsPoint(mousePosition))
				{
					BeginNameEditing(item, element, 0.5f);
				}
			}

			_AllowRenameOnMouseUp = false;

			element.ReleaseMouse();
			e.StopPropagation();
		}

		void OnMouseCaptureOutItem(MouseCaptureOutEvent e)
		{
			_AllowRenameOnMouseUp = false;
		}

		VisualElement MakeTreeItem()
		{
			var itemContainer = new VisualElement()
			{
				name = itemUssClassName,
			};
			itemContainer.AddToClassList(itemUssClassName);

			itemContainer.RegisterCallback<MouseDownEvent>(OnMouseDownItem);
			itemContainer.RegisterCallback<MouseUpEvent>(OnMouseUpItem);
			itemContainer.RegisterCallback<MouseCaptureOutEvent>(OnMouseCaptureOutItem);

			var indents = new VisualElement()
			{
				name = itemIndentsContainerUssClassName,
			};
			indents.AddToClassList(itemIndentsContainerUssClassName);
			itemContainer.hierarchy.Add(indents);

			var toggle = new Toggle() 
			{
				name = itemToggleUssClassName,
			};
			toggle.AddToClassList(Foldout.toggleUssClassName);
			toggle.AddToClassList(itemToggleUssClassName);
			toggle.RegisterValueChangedCallback(ToggleExpandedState);
			itemContainer.hierarchy.Add(toggle);

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

			if (_MakeItem != null)
			{
				userContentContainer.Add(_MakeItem());
			}

			itemContainer.AddManipulator(new ContextClickManipulator(OnContextClickItem));

			return itemContainer;
		}

		void OnChangeDisplayName(TreeViewItem item)
		{
			var itemContainer = GetItemElement(item);
			if (itemContainer != null)
			{
				var label = itemContainer.Q<TextElement>(itemLabelUssClassName);
				label.text = item.displayName;
			}
		}

		void BindTreeItem(VisualElement element, int index)
		{
			var item = _ViewItems[index];

			element.userData = item;

			// Add indentation.
			var indents = element.Q(itemIndentsContainerUssClassName);
			indents.Clear();
			for (int i = 0; i < item.depth -1; ++i)
			{
				var indentElement = new VisualElement();
				indentElement.AddToClassList(itemIndentUssClassName);
				indents.Add(indentElement);
			}

			// Set toggle data.
			var toggle = element.Q<Toggle>(itemToggleUssClassName);
			toggle.SetValueWithoutNotify(state.IsExpanded(item));
			toggle.userData = item.id;
			if (item.children.Count > 0)
				toggle.visible = true;
			else
				toggle.visible = false;

			var icon = element.Q<Image>(itemIconUssClassName);
			if (item.icon != null)
			{
				icon.image = item.icon;
				icon.style.display = DisplayStyle.Flex;
			}
			else
			{
				icon.style.display = DisplayStyle.None;
			}

			var label = element.Q<TextElement>(itemLabelUssClassName);
			label.SetEnabled(!item.disable);
			label.text = item.displayName;

			item.onChangedDisplayName -= OnChangeDisplayName;
			item.onChangedDisplayName += OnChangeDisplayName;

			if (_RenameOverlay != null && _RenameOverlay.IsRenaming() && _RenameOverlay.renameUserData == item.id)
			{
				if (_RenameOverlay.parent != element)
				{
					element.Add(_RenameOverlay);
				}
				_RenameOverlay.attachTarget = label;
			}

			if (_BindItem == null)
				return;

			// Bind user content container.
			var userContentContainer = element.Q(itemContentContainerUssClassName).ElementAt(0);
			_BindItem(userContentContainer, item);
		}

		void ListupTreeView(TreeViewItem group)
		{
			int elementCount = group.children.Count;
			for (int elementIndex = 0; elementIndex < elementCount; elementIndex++)
			{
				TreeViewItem item = group.children[elementIndex];

				_ViewItems.Add(item);

				if (item.children.Count > 0 && state.IsExpanded(item))
				{
					ListupTreeView(item);
				}
			}
		}

		public void ListViewRefresh()
		{
			_ListView.RebuildList();
		}

		public void UpdateViewTree()
		{
			_ViewItems.Clear();

			ListupTreeView(_TreeView.currentRoot);

			ListViewRefresh();

			UpdateSelection();
		}

		void UpdateSelection()
		{
			_ListView.UnregisterCallbackSelectionChange(OnSelectionChange);

			_ListView.ClearSelection();

			for (int i = 0; i < _ViewItems.Count; i++)
			{
				var item = _ViewItems[i];
				if (state.IsSelected(item))
				{
					_ListView.AddToSelection(i);
				}
			}

			_ListView.RegisterCallbackSelectionChange(OnSelectionChange);
		}
	}
}