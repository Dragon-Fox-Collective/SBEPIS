//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace ArborEditor.IMGUI.Controls
{
	public class TreeViewValueItem<T> : TreeViewItem
	{
		public T value;

		public TreeViewValueItem(int id, string name, T value, Texture2D icon) : base(id, name, icon)
		{
			this.value = value;
		}
	}
}