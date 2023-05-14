//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ArborEditor.IMGUI.Controls
{
	using Arbor;

	internal sealed class TreeView
	{
		public ITreeFilter filter;
		public TreeViewItem noneValueItem = null;

		[System.NonSerialized]
		private string[] _SearchWords;

		public TreeViewItem root
		{
			get;
			private set;
		}

		public TreeViewItem filterRoot
		{
			get;
			private set;
		}

		public TreeViewItem searchRoot
		{
			get;
			private set;
		}

		public TreeViewItem currentRoot
		{
			get
			{
				return searchRoot ?? filterRoot ?? root;
			}
		}

		public TreeView()
		{
			root = new TreeViewItem(0, "Root", null);
		}

		static TreeViewItem FindItemRecursive(int id, TreeViewItem item)
		{
			if (item == null)
			{
				return null;
			}

			if (item.id == id)
			{
				return item;
			}

			if (item.children == null)
			{
				return null;
			}

			for (int childIndex = 0; childIndex < item.children.Count; childIndex++)
			{
				TreeViewItem child = item.children[childIndex];
				TreeViewItem result = FindItemRecursive(id, child);
				if (result != null)
				{
					return result;
				}
			}
			return null;
		}

		public TreeViewItem FindItem(int id, TreeViewItem root)
		{
			return FindItemRecursive(id, root);
		}

		public TreeViewItem FindItem(int id)
		{
			return FindItem(id, root);
		}

		static TreeViewItem FindItemRecursive(System.Func<TreeViewItem, bool> find, TreeViewItem item)
		{
			if (item == null)
			{
				return null;
			}

			if (find(item))
			{
				return item;
			}

			if (item.children == null)
			{
				return null;
			}

			for (int childIndex = 0; childIndex < item.children.Count; childIndex++)
			{
				TreeViewItem child = item.children[childIndex];
				TreeViewItem result = FindItemRecursive(find, child);
				if (result != null)
				{
					return result;
				}
			}
			return null;
		}

		public TreeViewItem FindItem(System.Func<TreeViewItem, bool> find)
		{
			return FindItemRecursive(find, root);
		}

		public static void SetupDepths(TreeViewItem root)
		{
			if (root == null)
			{
				throw new System.ArgumentNullException("root", "The root is null");
			}

			Stack<TreeViewItem> stack = new Stack<TreeViewItem>();
			stack.Push(root);
			while (stack.Count > 0)
			{
				TreeViewItem current = stack.Pop();
				if (current.children == null)
				{
					continue;
				}

				for (int childIndex = 0; childIndex < current.children.Count; childIndex++)
				{
					TreeViewItem child = current.children[childIndex];
					if (child == null)
					{
						continue;
					}

					child.depth = current.depth + 1;
					stack.Push(child);
				}
			}
		}

		public void SetupDepths()
		{
			SetupDepths(root);
		}

		List<TreeViewItem> SearchElement(TreeViewItem group, int searchIndex)
		{
			using (new ProfilerScope(group.displayName))
			{
				List<TreeViewItem> items = null;

				int elementCount = group.children.Count;
				for (int elementIndex = 0; elementIndex < elementCount; elementIndex++)
				{
					TreeViewItem item = group.children[elementIndex];

					TreeViewItem originalItem = item;
					TreeViewSearchItem searchElement = item as TreeViewSearchItem;
					if (searchElement != null)
					{
						originalItem = searchElement.original;
					}

					string searchName = item.searchName;

					if (item.children.Count > 0)
					{
						int localSearchIndex = searchIndex;
						if (_SearchWords.Length > searchIndex)
						{
							string word = _SearchWords[searchIndex];
							if (searchName.Contains(word))
							{
								localSearchIndex++;
							}
						}

						var searchItems = SearchElement(item, localSearchIndex);

						if (searchItems != null && searchItems.Count > 0)
						{
							TreeViewSearchItem newElement = new TreeViewSearchItem(originalItem.id, originalItem);
							newElement.disable = item.disable;
							newElement.AddChildren(searchItems);

							if (items == null)
							{
								items = new List<TreeViewItem>();
							}
							items.Add(newElement);
						}
					}
					else
					{
						bool flag = true;

						if (item != noneValueItem)
						{
							int wordCount = _SearchWords.Length;
							for (int wordIndex = searchIndex; wordIndex < wordCount; wordIndex++)
							{
								if (!searchName.Contains(_SearchWords[wordIndex]))
								{
									flag = false;
									break;
								}
							}
						}

						if (flag)
						{
							TreeViewSearchItem newElement = new TreeViewSearchItem(originalItem.id, originalItem);
							newElement.disable = item.disable;

							if (items == null)
							{
								items = new List<TreeViewItem>();
							}
							items.Add(newElement);
						}
					}
				}

				return items;
			}
		}

		bool IsFilterValid(TreeViewItem valueItem)
		{
			if (valueItem == null || valueItem == noneValueItem)
			{
				return true;
			}

			return filter.IsValid(valueItem);
		}

		List<TreeViewItem> FilterItems(TreeViewItem group)
		{
			List<TreeViewItem> items = null;
			
			using (new ProfilerScope(group.displayName))
			{
				int childCount = group.children.Count;
				for (int childIndex = 0; childIndex < childCount; childIndex++)
				{
					TreeViewItem item = group.children[childIndex];

					TreeViewItem originalItem = item;
					TreeViewSearchItem searchItem = item as TreeViewSearchItem;
					if (searchItem != null)
					{
						originalItem = searchItem.original;
					}

					List<TreeViewItem> filterChildren = null;
					if (item.children.Count > 0)
					{
						filterChildren = FilterItems(item);
					}

					bool filterValid = IsFilterValid(originalItem);
					if (filterValid || filterChildren != null && filterChildren.Count > 0)
					{
						TreeViewSearchItem newElement = new TreeViewSearchItem(originalItem.id, originalItem);
						newElement.disable = newElement.disable || !filterValid;

						if (filterChildren != null)
						{
							newElement.AddChildren(filterChildren);
						}

						if (items == null)
						{
							items = new List<TreeViewItem>();
						}
						items.Add(newElement);
					}
				}

				return items;
			}
		}

		public void ClearTree()
		{
			if (filterRoot != null)
			{
				filterRoot.Dispose();
				filterRoot = null;
			}
			if (searchRoot != null)
			{
				searchRoot.Dispose();
				searchRoot = null;
			}
			root.Clear();
		}

		public void RebuildSearch(bool hasSearch, string searchWord)
		{
			if (filter != null && filter.useFilter && filter.openFilter)
			{
				filterRoot = new TreeViewItem(0, "Search", null);
				filterRoot.AddChildren(FilterItems(root));
				SetupDepths(filterRoot);
			}
			else
			{
				filterRoot = null;
			}

			if (!hasSearch)
			{
				searchRoot = null;
			}
			else
			{
				using (new ProfilerScope("RebuildSearch"))
				{
					string str1 = searchWord.ToLower();
					_SearchWords = str1.Split(new char[] { ' ' });

					searchRoot = new TreeViewItem(0, "Search", null);
					searchRoot.AddChildren(SearchElement(filterRoot ?? root, 0));
					SetupDepths(searchRoot);
				}
			}
		}
	}
}