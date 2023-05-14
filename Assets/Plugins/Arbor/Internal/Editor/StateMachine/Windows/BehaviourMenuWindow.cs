//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	internal sealed class BehaviourMenuWindow : SelectScriptWindow<StateBehaviour>
	{
		private static BehaviourMenuWindow _Instance;

		public static BehaviourMenuWindow instance
		{
			get
			{
				if (_Instance == null)
				{
					BehaviourMenuWindow[] objects = Resources.FindObjectsOfTypeAll<BehaviourMenuWindow>();
					if (objects.Length > 0)
					{
						_Instance = objects[0];
					}
				}
				if (_Instance == null)
				{
					_Instance = ScriptableObject.CreateInstance<BehaviourMenuWindow>();
				}
				return _Instance;
			}
		}

		private int _InsertIndex = -1;
		private System.Action<int, System.Type> _OnSelect;

		protected override string searchWord
		{
			get
			{
				return ArborEditorCache.behaviourSearch;
			}

			set
			{
				ArborEditorCache.behaviourSearch = value;
			}
		}

		protected override string GetRootElementName()
		{
			return "Behaviours";
		}

		protected override void OnSelect(System.Type classType)
		{
			_OnSelect?.Invoke(_InsertIndex, classType);
		}

		public void Init(Rect buttonRect, int insertIndex, System.Action<int, System.Type> onSelect)
		{
			_InsertIndex = insertIndex;
			_OnSelect = onSelect;

			Vector2 center = buttonRect.center;
			buttonRect.width = 300f;
			buttonRect.center = center;

			Open(buttonRect);
		}
	}
}
