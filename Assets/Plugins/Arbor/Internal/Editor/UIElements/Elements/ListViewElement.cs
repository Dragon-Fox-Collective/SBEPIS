//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArborEditor.UIElements
{
	internal sealed class ListViewElement : ListView, ISerializationCallbackReceiver
	{
#if ARBOR_DLL
		private static readonly System.Reflection.PropertyInfo s_FixedItemHeightProperty;
		private static readonly System.Reflection.PropertyInfo s_ItemHeightProperty;

		static ListViewElement()
		{
			s_FixedItemHeightProperty = typeof(ListView).GetProperty("fixedItemHeight");
			s_ItemHeightProperty = typeof(ListView).GetProperty("itemHeight");
		}
#endif

		public ListViewElement(IList itemsSource, int itemHeight, Func<VisualElement> makeItem, Action<VisualElement, int> bindItem)
#if !ARBOR_DLL
			: base(itemsSource, itemHeight, makeItem, bindItem)
#endif
		{
#if ARBOR_DLL
			this.itemsSource = itemsSource;
			if (s_FixedItemHeightProperty != null)
			{
				s_FixedItemHeightProperty.SetValue(this, (float)itemHeight);
			}
			else if(s_ItemHeightProperty != null)
			{
				s_ItemHeightProperty.SetValue(this, itemHeight);
			}
			this.makeItem = makeItem;
			this.bindItem = bindItem;
#endif
		}

#if !UNITY_2020_1_OR_NEWER
		public new void AddToSelection(int index)
		{
			base.AddToSelection(index);
		}

		public new void RemoveFromSelection(int index)
		{
			base.RemoveFromSelection(index);
		}

		public new void ClearSelection()
		{
			base.ClearSelection();
		}
#endif

		public event Action onAfterDeserialize;

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			// Because viewData restores the selection list
			// Reflects the node selection status of NodeGraphEditor
			onAfterDeserialize?.Invoke();
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}
	}
}