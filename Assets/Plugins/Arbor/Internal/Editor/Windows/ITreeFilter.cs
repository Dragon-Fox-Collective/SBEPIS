//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine.UIElements;

namespace ArborEditor
{
	using ArborEditor.IMGUI.Controls;

	public interface ITreeFilter
	{
		bool useFilter
		{
			get;
		}

		bool openFilter
		{
			get;
			set;
		}

		VisualElement CreateFilterSettingsElement();

		bool IsValid(TreeViewItem item);
	}
}