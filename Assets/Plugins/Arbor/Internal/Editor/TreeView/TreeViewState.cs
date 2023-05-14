//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace ArborEditor.IMGUI.Controls
{
	[System.Serializable]
	internal class TreeViewState
	{
		public Vector2 scrollPos;
		public bool filtering;

		[SerializeField]
		private List<int> _ExpandedIDs = new List<int>();

		[SerializeField]
		private List<int> _FilterExpandedIDs = new List<int>();

		[SerializeField]
		private List<int> _SelectedItemIDs = new List<int>();

		public List<int> expandedIDs
		{
			get
			{
				return _ExpandedIDs;
			}
		}

		public List<int> filterExpandedIDs
		{
			get
			{
				return _FilterExpandedIDs;
			}
		}

		public List<int> currentExpandedIDs
		{
			get
			{
				if (filtering)
				{
					return _FilterExpandedIDs;
				}
				else
				{
					return _ExpandedIDs;
				}
			}
		}

		public int selectedId
		{
			get
			{
				var selectedItemIDs = _SelectedItemIDs;
				if (selectedItemIDs == null || selectedItemIDs.Count == 0)
				{
					return 0;
				}

				return selectedItemIDs[0];
			}
		}

		public bool IsExpanded(int id)
		{
			return currentExpandedIDs.BinarySearch(id) >= 0;
		}

		public bool IsExpanded(TreeViewItem item)
		{
			if (item == null)
			{
				return false;
			}

			return IsExpanded(item.id);
		}

		public bool SetExpanded(int id, bool expand, bool sort = true)
		{
			bool expanded = IsExpanded(id);
			if (expand == expanded)
			{
				return false;
			}

			var currentExpandedIDs = this.currentExpandedIDs;

			if (expand)
			{
				currentExpandedIDs.Add(id);
				if (sort)
				{
					currentExpandedIDs.Sort();
				}
			}
			else
			{
				currentExpandedIDs.Remove(id);
			}

			return true;
		}

		public bool IsSelected(TreeViewItem item)
		{
			if (item == null)
			{
				return false;
			}

			return _SelectedItemIDs.Contains(item.id);
		}

		public void SetSelected(TreeViewItem item)
		{
			if (item == null)
			{
				_SelectedItemIDs.Clear();
				return;
			}

			if (IsSelected(item))
			{
				return;
			}

			_SelectedItemIDs.Clear();
			_SelectedItemIDs.Add(item.id);
		}

		public void SetSelection(IEnumerable<TreeViewItem> items)
		{
			_SelectedItemIDs.Clear();
			foreach (var item in items)
			{
				_SelectedItemIDs.Add(item.id);
			}
		}

		public void Clear()
		{
			scrollPos = Vector2.zero;
			_ExpandedIDs.Clear();
			_FilterExpandedIDs.Clear();
			_SelectedItemIDs.Clear();
		}
	}
}