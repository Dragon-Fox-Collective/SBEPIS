//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor
{
	using ArborEditor.UnityEditorBridge.Extensions;
	using ArborEditor.UIElements;

	public abstract class SelectWindowBase : EditorWindow
	{
		public class Element : System.IComparable
		{
			public int level;
			public GUIContent content;
			public bool disabled;

			public string name
			{
				get
				{
					return content.text;
				}
			}

			public int CompareTo(object obj)
			{
				return name.CompareTo((obj as Element).name);
			}
		}

		[System.Serializable]
		public sealed class GroupElement : Element
		{
			public Vector2 scroll;
			public int selectedIndex;

			public GroupElement(int level, string name)
			{
				this.level = level;
				this.content = GUIContentCaches.Get(name);
			}
		}

		private Element[] _Tree;
		private Element[] _SearchResultTree;

		private List<GroupElement> _Stack = new List<GroupElement>();
		private float _Anim = 1f;
		private int _AnimTarget = 1;
		private long _LastTime;

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

		private GroupElement activeParent
		{
			get
			{
				return _Stack[_Stack.Count - 2 + _AnimTarget];
			}
		}

		private Element[] activeTree
		{
			get
			{
				if (hasSearch)
				{
					return _SearchResultTree;
				}
				return _Tree;
			}
		}

		private Element activeElement
		{
			get
			{
				if (activeTree == null)
				{
					return null;
				}
				List<Element> children = GetChildren(activeTree, activeParent);
				if (children.Count == 0)
				{
					return null;
				}
				return children[activeParent.selectedIndex];
			}
		}

		private void GetChildren(Element[] tree, Element parent, ref List<Element> list)
		{
			if (list == null)
			{
				list = new List<Element>();
			}
			else
			{
				list.Clear();
			}

			int num = -1;
			int index;
			for (index = 0; index < tree.Length; ++index)
			{
				if (tree[index] == parent)
				{
					num = parent.level + 1;
					++index;
					break;
				}
			}
			if (num == -1)
				return;
			for (; index < tree.Length; ++index)
			{
				Element element = tree[index];
				if (element.level >= num)
				{
					if (element.level <= num || hasSearch)
						list.Add(element);
				}
				else
					break;
			}
		}

		private List<Element> GetChildren(Element[] tree, Element parent)
		{
			List<Element> list = new List<Element>();
			GetChildren(tree, parent, ref list);
			return list;
		}

		protected class TreeBuilder
		{
			List<string> list1 = new List<string>();
			List<Element> list2 = new List<Element>();

			public Element[] elements
			{
				get
				{
					return list2.ToArray();
				}
			}

			public void Begin(string rootGroupName)
			{
				list1.Clear();
				list2.Clear();

				list2.Add(new GroupElement(0, rootGroupName));
			}

			public void AddElement(string menuName, System.Func<int, string, Element> createElement)
			{
				string[] menuNameArray = menuName.Split(new char[] { '/' });

				int oldGroupCount = list1.Count;
				int newGroupCount = menuNameArray.Length - 1;
				if (oldGroupCount > newGroupCount)
				{
					int removeCount = oldGroupCount - newGroupCount;
					list1.RemoveRange(oldGroupCount - removeCount, removeCount);
				}

				for (int j = 0; j < newGroupCount && j < list1.Count; j++)
				{
					if (menuNameArray[j] != list1[j])
					{
						list1.RemoveRange(j, list1.Count - j);
						break;
					}
				}

				while (newGroupCount > list1.Count)
				{
					list2.Add(new GroupElement(list1.Count + 1, menuNameArray[list1.Count]));
					list1.Add(menuNameArray[list1.Count]);
				}

				var element = createElement(list1.Count + 1, menuNameArray[menuNameArray.Length - 1]);
				list2.Add(element);
			}
		}

		protected abstract string GetRootElementName();
		protected abstract void OnCreateTree(TreeBuilder builder);

		private void CreateTree()
		{
			TreeBuilder builder = new TreeBuilder();
			builder.Begin(GetRootElementName());
			OnCreateTree(builder);
			_Tree = builder.elements;

			if (_Stack.Count == 0)
			{
				_Stack.Add(_Tree[0] as GroupElement);
			}
			else
			{
				GroupElement groupElement = _Tree[0] as GroupElement;
				int level = 0;
				while (true)
				{
					GroupElement groupElement2 = _Stack[level];
					_Stack[level] = groupElement;
					_Stack[level].selectedIndex = groupElement2.selectedIndex;
					_Stack[level].scroll = groupElement2.scroll;
					level++;
					if (level == _Stack.Count)
					{
						break;
					}
					List<Element> children = GetChildren(activeTree, groupElement);
					Element element = children.FirstOrDefault((c) => c.name == _Stack[level].name);
					if (element != null && element is GroupElement)
					{
						groupElement = (element as GroupElement);
					}
					else
					{
						while (_Stack.Count > level)
						{
							_Stack.RemoveAt(level);
						}
					}
				}
			}

			RebuildSearch();
		}

		private void RebuildSearch()
		{
			if (!hasSearch)
			{
				_SearchResultTree = null;
				if (_Stack[_Stack.Count - 1].name == "Search")
				{
					_Stack.Clear();
					_Stack.Add(_Tree[0] as GroupElement);
				}
				_AnimTarget = 1;
				_LastTime = System.DateTime.Now.Ticks;
			}
			else
			{
				string[] searchWords = searchWord.ToLower().Split(new char[] { ' ' });

				List<Element> matchesStart = new List<Element>();
				List<Element> matchesWithin = new List<Element>();

				for (int elementIndex = 0; elementIndex < _Tree.Length; elementIndex++)
				{
					Element element = _Tree[elementIndex];
					if (element is GroupElement)
					{
						continue;
					}

					string name = element.name.ToLower().Replace(" ", "");

					bool didMatchAll = true;
					bool didMatchStart = false;

					for (int wordIndex = 0; wordIndex < searchWords.Length; ++wordIndex)
					{
						string word = searchWords[wordIndex];
						if (name.Contains(word))
						{
							if (wordIndex == 0 && name.StartsWith(word, System.StringComparison.Ordinal))
							{
								didMatchStart = true;
							}
						}
						else
						{
							didMatchAll = false;
							break;
						}
					}
					if (didMatchAll)
					{
						if (didMatchStart)
							matchesStart.Add(element);
						else
							matchesWithin.Add(element);
					}
				}

				matchesStart.Sort();
				matchesWithin.Sort();

				List<Element> searchTree = new List<Element>();
				searchTree.Add((Element)new GroupElement(0, "Search"));
				searchTree.AddRange(matchesStart);
				searchTree.AddRange(matchesWithin);

				_SearchResultTree = searchTree.ToArray();
				_Stack.Clear();
				_Stack.Add(_SearchResultTree[0] as GroupElement);

				_Anim = 1f;
				_AnimTarget = 1;

				if (GetChildren(activeTree, activeParent).Count >= 1)
				{
					activeParent.selectedIndex = 0;
				}
				else
				{
					activeParent.selectedIndex = -1;
				}
			}

			_ListElement.Refresh(0);
			_ListElement.SetSelection(activeParent.selectedIndex, false);

			_PrevListElement.RemoveFromHierarchy();
			_ListAreaElement.style.left = 0f;
		}

		private GroupElement GetElementRelative(int rel)
		{
			int index = _Stack.Count + rel - 1;
			if (index < 0)
			{
				return null;
			}
			return _Stack[index];
		}

		private void GoToParent()
		{
			if (_Stack.Count <= 1)
			{
				return;
			}
			_AnimTarget = 0;
			_LastTime = System.DateTime.Now.Ticks;

			_ListElement.Refresh(0);
			_PrevListElement.Refresh(-1);
			_ListAreaElement.Insert(0, _PrevListElement);
		}

		protected abstract void OnSelect(Element element);

		private void GoToChild(Element e, bool addIfComponent)
		{
			if (e is GroupElement)
			{
				if (hasSearch)
					return;
				_LastTime = System.DateTime.Now.Ticks;
				if (_AnimTarget == 0)
				{
					_AnimTarget = 1;
				}
				else
				{
					if (_Anim != 1.0f)
						return;
					_Anim = 0.0f;
					_Stack.Add(e as GroupElement);
				}

				_ListElement.Refresh(0);
				_PrevListElement.Refresh(-1);

				_ListAreaElement.Insert(0, _PrevListElement);
			}
			else if(e != null)
			{
				if (!addIfComponent)
					return;

				if (e.disabled)
					return;

				_IsSelected = true;

				OnSelect(e);

				Close();
			}
		}

		protected void Open(Rect buttonRect)
		{
			buttonRect.x = Mathf.Round(buttonRect.x);
			buttonRect.y = Mathf.Round(buttonRect.y);
			buttonRect.width = Mathf.Floor(buttonRect.width);
			buttonRect.height = Mathf.Floor(buttonRect.height);

			this.ShowAsDropDown(buttonRect, new Vector2(300f, 320f), true);
		}

		private bool _IsCompiling = false;

		void Update()
		{
			UpdateAnim();

			bool endCompile = false;
			if (EditorApplication.isCompiling)
			{
				_IsCompiling = true;
			}
			else if (_IsCompiling)
			{
				endCompile = true;
				_IsCompiling = false;
			}

			// Close on lost focus.
			// OnLostFocus() will be called even if the UnityEditor application loses focus, so check with Update.
			if (endCompile || UnityEditorInternal.InternalEditorUtility.isApplicationActive && EditorWindow.focusedWindow != this)
			{
				Close();
			}
		}

		void UpdateAnim()
		{
			if (_Anim == _AnimTarget)
				return;
			long ticks = System.DateTime.Now.Ticks;
			float num = (float)(ticks - _LastTime) / 1E+07f;
			_LastTime = ticks;
			_Anim = Mathf.MoveTowards(_Anim, _AnimTarget, num * 4f);
			if (_AnimTarget == 0 && _Anim == 0.0f)
			{
				_Anim = 1f;
				_AnimTarget = 1;
				_Stack.RemoveAt(_Stack.Count - 1);
			}

			if (_Anim == _AnimTarget)
			{
				_ListElement.Refresh(0);

				_PrevListElement.RemoveFromHierarchy();
				_ListAreaElement.style.left = 0f;
			}
			else
			{
				float anim = Mathf.SmoothStep(0.0f, 1f, _Anim);
				_ListAreaElement.style.left = -_ListAreaElement.contentRect.width * anim;
			}

			Repaint();
		}

		void SearchGUI()
		{
			GUI.SetNextControlName("ArborBehaviourSearch");
			string str = EditorGUITools.DropdownSearchField(searchWord);
			if (str != searchWord)
			{
				searchWord = str;
				RebuildSearch();
			}
		}

		private void HandleKeyboard()
		{
			Event current = Event.current;
			if (current.type != EventType.KeyDown)
			{
				return;
			}

			if (current.keyCode == KeyCode.DownArrow)
			{
				++activeParent.selectedIndex;
				activeParent.selectedIndex = Mathf.Min(activeParent.selectedIndex, GetChildren(activeTree, activeParent).Count - 1);
				_ListElement.SetSelection(activeParent.selectedIndex);
				current.Use();
			}
			if (current.keyCode == KeyCode.UpArrow)
			{
				--activeParent.selectedIndex;
				activeParent.selectedIndex = Mathf.Max(activeParent.selectedIndex, 0);
				_ListElement.SetSelection(activeParent.selectedIndex);
				current.Use();
			}
			if (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter)
			{
				GoToChild(activeElement, true);
				current.Use();
			}
			if (hasSearch)
			{
				return;
			}
			if (current.keyCode == KeyCode.LeftArrow || current.keyCode == KeyCode.Backspace)
			{
				GoToParent();
				current.Use();
			}
			if (current.keyCode == KeyCode.RightArrow)
			{
				GoToChild(activeElement, false);
				current.Use();
			}
			if (current.keyCode != KeyCode.Escape)
			{
				return;
			}
			Close();
			current.Use();
		}

		private bool _IsSelected = false;

		void OnEnable()
		{
			_IsSelected = false;

			rootVisualElement.AddToClassList("grey-border");

			ArborStyleSheets.Setup(rootVisualElement);

			SetupElements();

			CreateTree();
		}

		private VisualElement _ListAreaElement;
		private ListElement _ListElement;
		private ListElement _PrevListElement;

		void SetupElements()
		{
			var searchBarGUI = new IMGUIContainer(OnSearchBarGUI);
			rootVisualElement.Add(searchBarGUI);

			_ListAreaElement = new VisualElement()
			{
				style =
				{
					flexBasis = 0f,
					flexGrow = 1f,
					flexDirection = FlexDirection.Row,
				}
			};
			rootVisualElement.Add(_ListAreaElement);

			_PrevListElement = new ListElement(this)
			{
				style =
				{
					flexShrink = 0f,
					width = Length.Percent(100f),
				}
			};

			_ListElement = new ListElement(this)
			{
				style =
				{
					flexShrink = 0f,
					width = Length.Percent(100f),
				}
			};
			_ListAreaElement.Add(_ListElement);
		}

		void OnDisable()
		{
			if (!_IsSelected)
			{
				OnCancel();
			}

			OnClose();
		}

		protected virtual void OnCancel()
		{
		}

		protected virtual void OnClose()
		{
		}

		static class Styles
		{
			public static readonly GUIStyle background = new GUIStyle(GUIStyle.none);
		}

		void OnSearchBarGUI()
		{
			using (new GUILayout.VerticalScope(Styles.background))
			{
				HandleKeyboard();

				EditorGUI.FocusTextInControl("ArborBehaviourSearch");

				SearchGUI();
			}
		}

		protected virtual void OnBindIconElement(Image iconElement, Element item)
		{
		}

		protected virtual bool OnBindHelpElement(VisualElement helpElement, Element item)
		{
			return false;
		}

		protected sealed class ListElement : VisualElement
		{
			private static readonly string s_ListViewName = "arbor-script-list__list-view";
			public static readonly string ussClassName = "arbor-script-list";
			public static readonly string itemUssClassName = ussClassName + "__item";
			public static readonly string itemIconUssClassName = ussClassName + "__item-icon";
			public static readonly string itemLabelUssClassName = ussClassName + "__item-label";
			public static readonly string itemArrowRightUssClassName = ussClassName + "__item-arrow-right";
			public static readonly string itemHelpUssClassName = ussClassName + "__item-help";

			private readonly SelectWindowBase _Window;

			private GroupElement _ParentGroup;
			private GroupElement _GrandParentGroup;
			private List<Element> _Items = new List<Element>();

			private Button _Header;
			private VisualElement _ArrowLeft;
			private ListViewElement _ListView;
			private ScrollView _ScrollView;

			public ListElement(SelectWindowBase window)
			{
				name = ussClassName;
				AddToClassList(ussClassName);

				_Window = window;

				_Header = new Button(OnClickHeader);
				_Header.RemoveFromClassList(Button.ussClassName);
				_Header.AddToClassList("header-bar");
				UIElementsUtility.SetBoldFont(_Header);
				Add(_Header);

				_ArrowLeft = new VisualElement();
				_ArrowLeft.AddToClassList("arrow-navigation-left");

				_ListView = new ListViewElement(_Items, 20, MakeItem, BindItem)
				{
					style =
					{
						flexGrow = 1f,
						flexBasis = 0f,
					}
				};
				_ListView.name = s_ListViewName;
				_ListView.AddToClassList(s_ListViewName);
				_ListView.RegisterCallbackSelectionChange(OnSelectionChange);
				_ListView.RegisterCallbackItemsChosen(OnItemChosen);
				_ListView.onAfterDeserialize += UpdateSelection;

				Add(_ListView);

				_ScrollView = _ListView.Q<ScrollView>();

				var scrollContainer = _ScrollView.contentContainer;
				scrollContainer.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedContainer);
				scrollContainer.RegisterCallback<AttachToPanelEvent>(OnAttachToPanelContainer);
			}

			void StoreScrollOffset()
			{
				if (_ParentGroup != null)
				{
					_ParentGroup.scroll = _ScrollView.scrollOffset;
				}
			}

			void RestoreScrollOffset()
			{
				if (_ParentGroup != null)
				{
					_ScrollView.scrollOffset = _ParentGroup.scroll;
				}
			}

			void OnHorizontalValueChanged(float value)
			{
				StoreScrollOffset();
			}

			void OnVerticalValueChanged(float value)
			{
				StoreScrollOffset();
			}

			private bool _IsLayouted = false;

			void OnGeometryChangedContainer(GeometryChangedEvent e)
			{
				_IsLayouted = true;

				if (e.newRect.height != 0f)
				{
					RestoreScrollOffset();
				}

				_ScrollView.horizontalScroller.valueChanged -= OnHorizontalValueChanged;
				_ScrollView.horizontalScroller.valueChanged += OnHorizontalValueChanged;

				_ScrollView.verticalScroller.valueChanged -= OnVerticalValueChanged;
				_ScrollView.verticalScroller.valueChanged += OnVerticalValueChanged;
			}

			private bool _RefreshOnAttachToPanel = false;

			void OnAttachToPanelContainer(AttachToPanelEvent e)
			{
				if (_RefreshOnAttachToPanel)
				{
					_RefreshOnAttachToPanel = false;
					_ListView.RebuildList();
				}

				RestoreScrollOffset();
			}

			void ItemChosen(Element item)
			{
				var index = _Items.IndexOf(item);

				_ParentGroup.selectedIndex = index;
				_Window.GoToChild(item, true);
			}

			void OnItemChosen(IEnumerable<object> items)
			{
				var item = items.FirstOrDefault() as Element;
				if (item != null)
				{
					ItemChosen(item);
				}
			}

			void OnSelectionChange(IEnumerable<object> items)
			{
				OnItemChosen(items);
			}

			VisualElement MakeItem()
			{
				VisualElement element = new VisualElement()
				{
					name = itemUssClassName,
				};
				element.AddToClassList(itemUssClassName);

				Image icon = new Image()
				{
					name = itemIconUssClassName,
					scaleMode = ScaleMode.ScaleToFit,
				};
				icon.AddToClassList(itemIconUssClassName);
				element.Add(icon);

				TextElement label = new TextElement()
				{
					name = itemLabelUssClassName,
				};
				label.AddToClassList(itemLabelUssClassName);
				element.Add(label);

				VisualElement arrow = new VisualElement()
				{
					name = itemArrowRightUssClassName,
					style =
					{
						display = DisplayStyle.None,
					}
				};
				arrow.AddToClassList("arrow-navigation-right");
				arrow.AddToClassList(itemArrowRightUssClassName);
				element.Add(arrow);

				Button help = null;
				help = new Button(() =>
				{
					var url = help.userData as string;
					Help.BrowseURL(url);
				})
				{
					name = itemHelpUssClassName,
					style =
					{
						display = DisplayStyle.None,
					}
				};
				help.RemoveFromClassList(Button.ussClassName);
				help.AddToClassList("icon-button");
				help.AddToClassList(itemHelpUssClassName);
				help.AddManipulator(new LocalizationManipulator("Help", LocalizationManipulator.TargetText.Tooltip));
				element.Add(help);

				Image helpIcon = new Image()
				{
					image = Icons.helpIcon,
				};
				help.Add(helpIcon);

				// Unity2022.2 or later: Since OnSelectionChange is not called even if you click the selected item, handle it directly with MouseDownEvent.
				element.RegisterCallback<MouseDownEvent>(OnMouseDownElement);

				return element;
			}

			void OnMouseDownElement(MouseDownEvent e)
			{
				var targetElement = e.currentTarget as VisualElement;
				var item = targetElement?.userData as Element;

				if (item != null)
				{
					ItemChosen(item);
					e.StopPropagation();
				}
			}

			void BindItem(VisualElement element, int index)
			{
				var item = _Items[index];

				element.userData = item;

				var icon = element.Q<Image>(itemIconUssClassName);
				icon.image = item.content.image;

				_Window.OnBindIconElement(icon, item);

				var label = element.Q<TextElement>(itemLabelUssClassName);
				label.text = item.content.text;
				label.SetEnabled(!item.disabled);

				var arrow = element.Q(itemArrowRightUssClassName);

				if (item is GroupElement)
				{
					arrow.style.display = DisplayStyle.Flex;
				}
				else
				{
					arrow.style.display = DisplayStyle.None;
				}

				var help = element.Q(itemHelpUssClassName);
				
				if (_Window.OnBindHelpElement(help, item))
				{
					help.style.display = DisplayStyle.Flex;
				}
				else
				{
					help.style.display = DisplayStyle.None;
				}
			}

			public void Refresh(int rel)
			{
				var parentGroup = _Window.GetElementRelative(rel);
				if (_ParentGroup != parentGroup)
				{
					_ParentGroup = parentGroup;

					_Header.text = _ParentGroup.name;
				}

				var grandParentGroup = _Window.GetElementRelative(rel - 1);
				if (_GrandParentGroup != grandParentGroup)
				{
					_GrandParentGroup = grandParentGroup;

					if (_GrandParentGroup != null)
					{
						_Header.Add(_ArrowLeft);
					}
					else
					{
						_ArrowLeft.RemoveFromHierarchy();
					}
				}

				int oldCount = _Items != null ? _Items.Count : 0;

				_Window.GetChildren(_Window.activeTree, _ParentGroup, ref _Items);

				if (_ScrollView.panel != null)
				{
					_ListView.RebuildList();
				}
				else
				{
					_RefreshOnAttachToPanel = true;
				}

				if (oldCount == _Items.Count && _IsLayouted)
				{
					if (_ScrollView.panel != null)
					{
						RestoreScrollOffset();
					}
				}
				else
				{
					_ScrollView.horizontalScroller.valueChanged -= OnHorizontalValueChanged;
					_ScrollView.verticalScroller.valueChanged -= OnVerticalValueChanged;
				}

				SetSelection(_ParentGroup.selectedIndex, false);
			}

			void OnClickHeader()
			{
				if (_GrandParentGroup != null)
				{
					_Window.GoToParent();
				}
			}

			void UpdateSelection()
			{
				SetSelection((_ParentGroup != null) ? _ParentGroup.selectedIndex : -1);
			}

			public void SetSelection(int index, bool scrollTo = true)
			{
				_ListView.UnregisterCallbackSelectionChange(OnSelectionChange);

				if (_ListView.itemsSource.Count <= index)
				{
					index = -1;
				}
				_ListView.selectedIndex = index;
				if (scrollTo)
				{
					_ListView.ScrollToItem(index);
				}

				_ListView.RegisterCallbackSelectionChange(OnSelectionChange);
			}
		}
	}
}