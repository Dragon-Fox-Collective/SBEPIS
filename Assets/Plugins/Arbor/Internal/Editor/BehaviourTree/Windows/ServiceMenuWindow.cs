//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.BehaviourTree
{
	using Arbor.BehaviourTree;

	internal sealed class ServiceMenuWindow : SelectScriptWindow<Service>
	{
		private static ServiceMenuWindow _Instance;

		public static ServiceMenuWindow instance
		{
			get
			{
				if (_Instance == null)
				{
					ServiceMenuWindow[] objects = Resources.FindObjectsOfTypeAll<ServiceMenuWindow>();
					if (objects.Length > 0)
					{
						_Instance = objects[0];
					}
				}
				if (_Instance == null)
				{
					_Instance = ScriptableObject.CreateInstance<ServiceMenuWindow>();
				}
				return _Instance;
			}
		}

		private int _Index;
		private System.Action<int, System.Type> _OnSelect;

		protected override string searchWord
		{
			get
			{
				return ArborEditorCache.serviceSearch;
			}

			set
			{
				ArborEditorCache.serviceSearch = value;
			}
		}

		protected override string GetRootElementName()
		{
			return "Services";
		}

		protected override void OnSelect(System.Type classType)
		{
			_OnSelect?.Invoke(_Index, classType);
		}

		public void Init(Rect buttonRect, int index, System.Action<int, System.Type> onSelect)
		{
			_Index = index;
			_OnSelect = onSelect;

			Vector2 center = buttonRect.center;
			buttonRect.width = 300f;
			buttonRect.center = center;

			Open(buttonRect);
		}
	}
}