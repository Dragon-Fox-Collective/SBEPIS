//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace ArborEditor.IMGUI.Controls
{
	public class TreeViewSearchItem : TreeViewItem
	{
		public TreeViewItem original;

		public TreeViewSearchItem(int id, TreeViewItem original) : base(id, original.displayName, original.icon)
		{
			this.original = original;
			this.disable = original.disable;
		}
	}
}