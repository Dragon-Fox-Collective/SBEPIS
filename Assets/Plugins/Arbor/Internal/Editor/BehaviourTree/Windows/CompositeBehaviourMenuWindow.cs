//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.BehaviourTree
{
	using Arbor.BehaviourTree;

	internal sealed class CompositeBehaviourMenuWindow : SelectScriptWindow<CompositeBehaviour>
	{
		private static CompositeBehaviourMenuWindow _Instance;

		public static CompositeBehaviourMenuWindow instance
		{
			get
			{
				if (_Instance == null)
				{
					CompositeBehaviourMenuWindow[] objects = Resources.FindObjectsOfTypeAll<CompositeBehaviourMenuWindow>();
					if (objects.Length > 0)
					{
						_Instance = objects[0];
					}
				}
				if (_Instance == null)
				{
					_Instance = ScriptableObject.CreateInstance<CompositeBehaviourMenuWindow>();
				}
				return _Instance;
			}
		}

		private Vector2 _Position;

		protected override string searchWord
		{
			get
			{
				return ArborEditorCache.compositeBehaviourSearch;
			}

			set
			{
				ArborEditorCache.compositeBehaviourSearch = value;
			}
		}

		protected override string GetRootElementName()
		{
			return "Composites";
		}

		public delegate void OnSelectCallback(Vector2 position, System.Type classType);
		public delegate void OnCancelCallback();
		public delegate void OnCloseCallback();

		private OnSelectCallback _OnSelect;
		private OnCancelCallback _OnCancel;
		private OnCloseCallback _OnClose;

		protected override void OnSelect(System.Type classType)
		{
			_OnSelect?.Invoke(_Position, classType);
		}

		protected override void OnCancel()
		{
			_OnCancel?.Invoke();
		}

		protected override void OnClose()
		{
			_OnClose?.Invoke();
		}

		public void Init(Vector2 position, Rect buttonRect, OnSelectCallback onSelect, OnCancelCallback onCancel, OnCloseCallback onClose)
		{
			_Position = position;
			_OnSelect = onSelect;
			_OnCancel = onCancel;
			_OnClose = onClose;

			Open(buttonRect);
		}
	}
}