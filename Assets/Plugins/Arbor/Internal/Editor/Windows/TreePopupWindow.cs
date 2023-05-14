//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using System.Collections.Generic;

namespace ArborEditor
{
	using Arbor;
	using ArborEditor.IMGUI.Controls;
	using ArborEditor.UIElements;

	internal abstract class TreePopupWindow<T> : EditorWindow
	{
		protected static TreePopupWindow<T> s_Instance;

		protected int _ControlID;

		const string kTreePopupWindowChangedMessage = "TreePopupWindowChanged";
		const string kTreePopupSearchControlName = "TreePopupSearchControlName";

		private static int s_SearchFieldHash = "s_SearchFieldHash".GetHashCode();

		[SerializeField]
		private TreeViewState _TreeViewState = new TreeViewState();

		[System.NonSerialized]
		protected TreeView _TreeView = null;

		private bool _FocusToSearchBar = false;
		private bool _HasSeachFilterFocus = false;
		private bool _DidSelectSearchResult;

		protected EditorWindow _SourceView;

		protected abstract string searchWord
		{
			get;
			set;
		}

		private bool hasSearch
		{
			get
			{
				return !string.IsNullOrEmpty(searchWord);
			}
		}

		protected abstract void OnCreateTree(TreeViewItem root);

		protected void CreateTree()
		{
			if (_TreeView == null)
			{
				InitTreeView();
			}

			TreeViewItem root = _TreeView.root;
			root.children.Clear();

			OnCreateTree(root);

			_TreeView.SetupDepths();

			_IsCreatedTree = true;

			SetupElements();

			RebuildSearch();

			_FocusToSearchBar = true;
		}

		internal static T GetSelectedValueForControl(int controlID, T selected)
		{
			Event current = Event.current;
			if (current.type == EventType.ExecuteCommand && current.commandName == kTreePopupWindowChangedMessage)
			{
				if (s_Instance != null && s_Instance._ControlID == controlID)
				{
					selected = s_Instance.selectedValue;
					GUI.changed = true;

					current.Use();
				}
			}
			return selected;
		}

		protected virtual bool isReady
		{
			get
			{
				return true;
			}
		}

		private bool _IsCreatedTree;
		public T selectedValue
		{
			get;
			private set;
		}

		protected void Init(Rect buttonRect, int controlID, T selected)
		{
			_ControlID = controlID;
			selectedValue = selected;
			_SourceView = EditorWindow.focusedWindow;

			if (isReady)
			{
				CreateTree();
			}
			else
			{
				_IsCreatedTree = false;
				SetupElements();
			}

			Vector2 center = buttonRect.center;
			buttonRect.width = 300f;
			buttonRect.center = center;
			ShowAsDropDown(buttonRect, new Vector2(300f, 320f));
		}

		void SetFilterExpandedItems(TreeViewItem root)
		{
			var filterExpandedIDs = _TreeViewState.filterExpandedIDs;
			filterExpandedIDs.Clear();

			if (root == null)
			{
				throw new System.ArgumentNullException("root", "The root is null");
			}

			Stack<TreeViewItem> stack = new Stack<TreeViewItem>();
			stack.Push(root);
			while (stack.Count > 0)
			{
				TreeViewItem current = stack.Pop();

				filterExpandedIDs.Add(current.id);

				if (current.children == null)
				{
					continue;
				}

				int childCount = current.children.Count;
				for (int i = 0; i < childCount; i++)
				{
					var child = current.children[i];
					if (child == null)
					{
						continue;
					}

					stack.Push(child);
				}
			}

			filterExpandedIDs.Sort();
		}

		protected void RebuildSearch()
		{
			if (!_IsCreatedTree)
			{
				return;
			}

			_TreeView.RebuildSearch(hasSearch, searchWord);

			TreeViewItem currentRoot = _TreeView.currentRoot;
			_TreeViewState.filtering = currentRoot != _TreeView.root;
			if (_TreeViewState.filtering)
			{
				SetFilterExpandedItems(currentRoot);
			}

			_TreeViewElement.UpdateViewTree();
			int selectedId = _TreeViewState.selectedId;
			if (selectedId != 0)
			{
				var selectedItem = _TreeView.FindItem(selectedId, _TreeView.currentRoot);
				_TreeViewElement.ScrollToItem(selectedItem);
			}
		}

		void SetSearchFilter(string searchFilter)
		{
			searchWord = searchFilter;

			RebuildSearch();

			if (_DidSelectSearchResult && string.IsNullOrEmpty(searchFilter))
			{
				_DidSelectSearchResult = false;

				_TreeViewElement?.Focus();
			}
		}

		void OnSearchBarGUI()
		{
			Event evt = Event.current;

			if (evt.type == EventType.KeyDown && evt.keyCode == KeyCode.Escape)
			{
				Close();
				evt.Use();
			}

			if (_HasSeachFilterFocus && evt.type == EventType.KeyDown && (evt.keyCode == KeyCode.DownArrow || evt.keyCode == KeyCode.UpArrow))
			{
				_TreeViewElement.Focus();
				_DidSelectSearchResult = !string.IsNullOrEmpty(searchWord);
				evt.Use();
			}

			GUI.SetNextControlName(kTreePopupSearchControlName);
			if (_FocusToSearchBar)
			{
				EditorGUI.FocusTextInControl(kTreePopupSearchControlName);
				if (Event.current.type == EventType.Repaint)
				{
					_FocusToSearchBar = false;
				}
			}

			Rect rect = GUILayoutUtility.GetRect(0, UnityEditorBridge.EditorGUILayoutBridge.kLabelFloatMaxW * 1.5f, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight, BuiltInStyles.toolbarSearchFieldRaw);

			int controlId = GUIUtility.GetControlID(s_SearchFieldHash, FocusType.Keyboard, rect);

			EditorGUI.BeginChangeCheck();
			int i = 0;
			string str = UnityEditorBridge.EditorGUIBridge.ToolbarSearchField(controlId, rect, null, ref i, searchWord);
			if (EditorGUI.EndChangeCheck())
			{
				if (str != searchWord)
				{
					SetSearchFilter(str);
				}
			}

			_HasSeachFilterFocus = GUIUtility.keyboardControl == controlId;
		}

		private VisualElement _WaitReadyElement;
		private VisualElement _TreeElement;
		private VisualElement _FilterField;
		private TreeViewElement _TreeViewElement;

		void OnEnable()
		{
			ArborStyleSheets.Setup(rootVisualElement);

			rootVisualElement.AddToClassList("grey-border");
			rootVisualElement.RegisterCallback<KeyDownEvent>(e =>
			{
				if (e.keyCode == KeyCode.Escape)
				{
					Close();
					e.StopPropagation();
				}
			}, TrickleDown.TrickleDown);

			SetupElements();
		}

		VisualElement MakeTreeItem()
		{
			Image checkMark = new Image()
			{
				image = Defaults.successTex,
				scaleMode = ScaleMode.ScaleToFit,
			};

			onSelectionChanged += () =>
			{
				TreeViewItem item = checkMark.userData as TreeViewItem;
				if (item == null)
				{
					return;
				}

				var searchItem = item as TreeViewSearchItem;
				if (searchItem != null)
				{
					item = searchItem.original;
				}

				var valueItem = item as TreeViewValueItem<T>;
				bool isSelectedValue = valueItem != null && EqualityComparer<T>.Default.Equals(valueItem.value, selectedValue);

				if (isSelectedValue)
				{
					checkMark.style.display = DisplayStyle.Flex;
				}
				else
				{
					checkMark.style.display = DisplayStyle.None;
				}
			};

			return checkMark;
		}

		protected static class Defaults
		{
			public static readonly Texture2D successTex;

			static Defaults()
			{
				successTex = EditorGUIUtility.IconContent("TestPassed").image as Texture2D;
			}
		}

		void BindTreeItem(VisualElement element, TreeViewItem item)
		{
			element.userData = item;

			var searchItem = item as TreeViewSearchItem;
			if (searchItem != null)
			{
				item = searchItem.original;
			}

			var valueItem = item as TreeViewValueItem<T>;
			bool isSelectedValue = valueItem != null && EqualityComparer<T>.Default.Equals(valueItem.value, selectedValue);

			if (isSelectedValue)
			{
				element.style.display = DisplayStyle.Flex;
			}
			else
			{
				element.style.display = DisplayStyle.None;
			}
		}

		void SetupElements()
		{
			if (isReady && _IsCreatedTree)
			{
				if (_WaitReadyElement != null && _WaitReadyElement.parent != null)
				{
					_WaitReadyElement.RemoveFromHierarchy();
				}

				if (_TreeElement == null)
				{
					_TreeElement = new VisualElement()
					{
						style =
						{
							flexGrow = 1f,
						}
					};

					var toolbar = new Toolbar();
					_TreeElement.Add(toolbar);

					var searchField = new IMGUIContainer(OnSearchBarGUI)
					{
						style =
						{
							flexGrow = 1f,
							marginTop = 2f,
							marginLeft = 3f,
							marginRight = 3f,
						}
					};
					toolbar.Add(searchField);

					ITreeFilter filterSettings = this as ITreeFilter;
					bool useFilter = filterSettings != null && filterSettings.useFilter;
					if (useFilter)
					{
						var filterToggle = new ToolbarToggle()
						{
							style =
							{
								alignItems = Align.Center,
								justifyContent = Justify.Center,
							}
						};
						filterToggle.SetValueWithoutNotify(filterSettings.openFilter);
						filterToggle.RegisterValueChangedCallback(e =>
						{
							filterSettings.openFilter = e.newValue;
							if (filterSettings.openFilter)
							{
								_FilterField.style.display = DisplayStyle.Flex;
							}
							else
							{
								_FilterField.style.display = DisplayStyle.None;
							}

							_TreeViewElement.ResetLayouted();

							RebuildSearch();
						});
						var filterImage = new Image()
						{
							image = Icons.filterIcon,
							scaleMode = ScaleMode.ScaleToFit,
							tintColor = EditorGUITools.GetIconColor(),
						};
						filterToggle.Add(filterImage);
						filterToggle.AddManipulator(new LocalizationManipulator("Filter", LocalizationManipulator.TargetText.Tooltip));
						toolbar.Add(filterToggle);

						_FilterField = filterSettings.CreateFilterSettingsElement();
						_FilterField.style.borderBottomColor = EditorGUITools.GetSplitColor();
						_FilterField.style.borderBottomWidth = 1f;
						if (filterSettings.openFilter)
						{
							_FilterField.style.display = DisplayStyle.Flex;
						}
						else
						{
							_FilterField.style.display = DisplayStyle.None;
						}
						_TreeElement.Add(_FilterField);
					}

					_TreeViewElement = new TreeViewElement(_TreeView, _TreeViewState, MakeTreeItem, BindTreeItem);
					_TreeViewElement.onSubmit += OnSelect;
					_TreeElement.Add(_TreeViewElement);
				}

				if (_TreeElement.parent == null)
				{
					rootVisualElement.Add(_TreeElement);
				}
			}
			else
			{
				if (_TreeElement != null && _TreeElement.parent != null)
				{
					_TreeElement.RemoveFromHierarchy();
				}

				if (_WaitReadyElement == null)
				{
					_WaitReadyElement = new VisualElement()
					{
						style =
						{
							flexDirection = FlexDirection.Row,
							alignItems = Align.Center,
							justifyContent = Justify.Center,
						}
					};
					_WaitReadyElement.StretchToParentSize();

					var indicator = new WaitSpin();
					_WaitReadyElement.Add(indicator);

					var label = new Label()
					{
						style =
					{
						fontSize = 13f,
					}
					};
					label.AddManipulator(new LocalizationManipulator("Loading", LocalizationManipulator.TargetText.Text));
					_WaitReadyElement.Add(label);
				}

				if (_WaitReadyElement.parent == null)
				{
					rootVisualElement.Add(_WaitReadyElement);
				}
			}
		}

		void InitTreeView()
		{
			if (_TreeViewState == null)
			{
				_TreeViewState = new TreeViewState();
			}

			_TreeView = new TreeView();
			_TreeView.filter = this as ITreeFilter;
		}

		protected bool IsExpanded(TreeViewItem item)
		{
			return _TreeViewState.IsExpanded(item);
		}

		protected void SetExpanded(TreeViewItem item, bool expand)
		{
			_TreeViewState.SetExpanded(item.id, expand);

			_TreeViewElement?.UpdateViewTree();
		}

		protected void SetSelectedItem(TreeViewItem item, bool scrollTo)
		{
			if (_TreeViewElement != null)
			{
				_TreeViewElement.SetSelectedItem(item, scrollTo);
			}
			else
			{
				_TreeViewState.SetSelected(item);
			}
		}

		private event System.Action onSelectionChanged;

		void OnSelect(TreeViewItem select)
		{
			var valueItem = select as TreeViewValueItem<T>;
			if (valueItem != null)
			{
				selectedValue = valueItem.value;
				_SourceView.SendEvent(EditorGUIUtility.CommandEvent(kTreePopupWindowChangedMessage));

				onSelectionChanged?.Invoke();
			}
		}

		void OnDisable()
		{
			_ControlID = 0;
			s_Instance = null;
		}

		void OnInspectorUpdate()
		{
			if (EditorApplication.isCompiling)
			{
				Close();
			}
			else if (!_IsCreatedTree)
			{
				if (ClassList.isReady)
				{
					CreateTree();
				}

				Repaint();
			}
		}
	}
}