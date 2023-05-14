//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArborEditor.UIElements
{
	internal static class ListViewExtensions
	{
#if ARBOR_DLL
		private static readonly System.Reflection.EventInfo s_OnSelectionChangeEvent;
		private static readonly System.Reflection.EventInfo s_OnItemsChosenEvent;
		private static readonly System.Reflection.MethodInfo s_RebuildMethod;

		static ListViewExtensions()
		{
			var listViewType = typeof(ListView);
			s_OnSelectionChangeEvent = listViewType.GetEvent("selectionChanged", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			if (s_OnSelectionChangeEvent == null)
			{
				s_OnSelectionChangeEvent = listViewType.GetEvent("onSelectionChange", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			}
			if (s_OnSelectionChangeEvent == null)
			{
				s_OnSelectionChangeEvent = listViewType.GetEvent("onSelectionChanged", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			}

			s_OnItemsChosenEvent = listViewType.GetEvent("itemsChosen", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			if(s_OnItemsChosenEvent == null)
			{
				s_OnItemsChosenEvent = listViewType.GetEvent("onItemsChosen", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			}

			s_RebuildMethod = listViewType.GetMethod("Rebuild", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			if (s_RebuildMethod == null)
			{
				s_RebuildMethod = listViewType.GetMethod("Refresh", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			}
		}
#endif

		public static void RegisterCallbackSelectionChange(this ListView listView, System.Action<IEnumerable<object>> onSelectionChange)
		{
#if ARBOR_DLL
			if (s_OnSelectionChangeEvent != null)
			{
				s_OnSelectionChangeEvent.AddEventHandler(listView, onSelectionChange);
			}
#elif UNITY_2022_2_OR_NEWER
			listView.selectionChanged += onSelectionChange;
#elif UNITY_2020_1_OR_NEWER
			listView.onSelectionChange += onSelectionChange;
#else
			listView.onSelectionChanged += onSelectionChange;
#endif
		}

		public static void UnregisterCallbackSelectionChange(this ListView listView, System.Action<IEnumerable<object>> onSelectionChange)
		{
#if ARBOR_DLL
			if (s_OnSelectionChangeEvent != null)
			{
				s_OnSelectionChangeEvent.RemoveEventHandler(listView, onSelectionChange);
			}
#elif UNITY_2022_2_OR_NEWER
			listView.selectionChanged -= onSelectionChange;
#elif UNITY_2020_1_OR_NEWER
			listView.onSelectionChange -= onSelectionChange;
#else
			listView.onSelectionChanged -= onSelectionChange;
#endif
		}

#if ARBOR_DLL || !UNITY_2020_1_OR_NEWER
		sealed class ItemsChosenDispacher
		{
#if ARBOR_DLL
			private static readonly System.Reflection.EventInfo s_OnItemChosenEvent;

			static ItemsChosenDispacher()
			{
				var listViewType = typeof(ListView);
				s_OnItemChosenEvent = listViewType.GetEvent("onItemChosen", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			}
#endif

			public System.Action<IEnumerable<object>> onItemsChosen;

			public void OnItemChosen(object item)
			{
				if (onItemsChosen != null)
				{
					using (Arbor.Pool.ListPool<object>.Get(out var items))
					{
						items.Add(item);
						onItemsChosen(items);
					}
				}
			}

			private static Dictionary<ListView, ItemsChosenDispacher> s_ItemsChosen = new Dictionary<ListView, ItemsChosenDispacher>();

			public static void Register(ListView listView, System.Action<IEnumerable<object>> onItemsChosen)
			{
				ItemsChosenDispacher itemsChosen = null;
				if (!s_ItemsChosen.TryGetValue(listView, out itemsChosen))
				{
					itemsChosen = new ItemsChosenDispacher();

					System.Action<object> onItemChosen = itemsChosen.OnItemChosen;
#if ARBOR_DLL
					if (s_OnItemChosenEvent != null)
					{
						s_OnItemChosenEvent.AddEventHandler(listView, onItemChosen);
					}
#else
					listView.onItemChosen += onItemChosen;
#endif
					s_ItemsChosen.Add(listView, itemsChosen);
				}
				itemsChosen.onItemsChosen += onItemsChosen;
			}

			public static void Unregister(ListView listView, System.Action<IEnumerable<object>> onItemsChosen)
			{
				if (s_ItemsChosen.TryGetValue(listView, out var itemsChosen))
				{
					itemsChosen.onItemsChosen -= onItemsChosen;
					if (itemsChosen.onItemsChosen == null)
					{
						System.Action<object> onItemChosen = itemsChosen.OnItemChosen;
#if ARBOR_DLL
						if (s_OnItemChosenEvent != null)
						{
							s_OnItemChosenEvent.RemoveEventHandler(listView, onItemChosen);
						}
#else
						listView.onItemChosen -= onItemChosen;
#endif
						s_ItemsChosen.Remove(listView);
					}
				}
			}
		}
#endif

		public static void RegisterCallbackItemsChosen(this ListView listView, System.Action<IEnumerable<object>> onItemsChosen)
		{
#if ARBOR_DLL
			if (s_OnItemsChosenEvent != null)
			{
				s_OnItemsChosenEvent.AddEventHandler(listView, onItemsChosen);
			}
			else
			{
				ItemsChosenDispacher.Register(listView, onItemsChosen);
			}
#elif UNITY_2022_2_OR_NEWER
			listView.itemsChosen += onItemsChosen;
#elif UNITY_2020_1_OR_NEWER
			listView.onItemsChosen += onItemsChosen;
#else
			ItemsChosenDispacher.Register(listView, onItemsChosen);
#endif
		}

		public static void UnregisterCallbackItemsChosen(this ListView listView, System.Action<IEnumerable<object>> onItemsChosen)
		{
#if ARBOR_DLL
			if (s_OnItemsChosenEvent != null)
			{
				s_OnItemsChosenEvent.RemoveEventHandler(listView, onItemsChosen);
			}
			else
			{
				ItemsChosenDispacher.Unregister(listView, onItemsChosen);
			}
#elif UNITY_2022_2_OR_NEWER
			listView.itemsChosen -= onItemsChosen;
#elif UNITY_2020_1_OR_NEWER
			listView.onItemsChosen -= onItemsChosen;
#else
			ItemsChosenDispacher.Unregister(listView, onItemsChosen);
#endif
		}

		public static void RebuildList(this ListView listView)
		{
#if ARBOR_DLL
			if(s_RebuildMethod != null)
			{
				s_RebuildMethod.Invoke(listView, null);
			}
#elif UNITY_2021_2_OR_NEWER
			listView.Rebuild();
#else
			listView.Refresh();
#endif
		}
	}
}