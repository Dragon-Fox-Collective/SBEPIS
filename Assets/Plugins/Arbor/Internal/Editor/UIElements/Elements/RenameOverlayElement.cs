//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor.UIElements
{
	using ArborEditor.UnityEditorBridge;
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	internal sealed class RenameOverlayElement : IMGUIContainer
	{
		private RenameOverlay _RenameOverlay;

		private System.Action<string, int> _OnRenameEnded = null;

		public int renameUserData
		{
			get
			{
				return _RenameOverlay.userData;
			}
		}

		public bool isWaitingForDelay
		{
			get
			{
				return _RenameOverlay.isWaitingForDelay;
			}
		}

		private VisualElement _AttachTarget;
		public VisualElement attachTarget
		{
			get
			{
				return _AttachTarget;
			}
			set
			{
				if (_AttachTarget != value)
				{
					if (_AttachTarget != null)
					{
						UnregisterCallbackFromAttachElement();
					}

					_AttachTarget = value;

					if (_AttachTarget != null)
					{
						RegisterCallbackOnAttachElement();

						AlignOnTarget();
					}
				}
			}
		}

		private List<VisualElement> _WatchedElements = null;

		void RegisterCallbackOnAttachElement()
		{
			var commonAncestor = _AttachTarget.FindCommonAncestor(this);

			if (_WatchedElements == null)
			{
				_WatchedElements = new List<VisualElement>();
			}

			VisualElement v = _AttachTarget;

			while (v != commonAncestor)
			{
				_WatchedElements.Add(v);
				v.RegisterCallback<GeometryChangedEvent>(OnTargetLayout);
				v = v.hierarchy.parent;
			}

			v = hierarchy.parent;

			while (v != commonAncestor)
			{
				_WatchedElements.Add(v);
				v.RegisterCallback<GeometryChangedEvent>(OnTargetLayout);
				v = v.hierarchy.parent;
			}
		}

		void UnregisterCallbackFromAttachElement()
		{
			_AttachTarget.visible = true;

			if (_WatchedElements == null || _WatchedElements.Count == 0)
				return;

			foreach (VisualElement v in _WatchedElements)
			{
				v.UnregisterCallback<GeometryChangedEvent>(OnTargetLayout);
			}

			_WatchedElements.Clear();
		}

		private void OnTargetLayout(GeometryChangedEvent evt)
		{
			AlignOnTarget();
		}

		internal void AlignOnTarget()
		{
			if (_AttachTarget == null || hierarchy.parent == null)
			{
				return;
			}

			Rect targetRect = _AttachTarget.layout;
			targetRect.position = Vector2.zero;

			this.SetLayout(_AttachTarget.ChangeCoordinatesTo(hierarchy.parent, targetRect));
		}

		public RenameOverlayElement(System.Action<string, int> onRenameEnded)
		{
			focusable = true;

			onGUIHandler = OnGUI;
			_RenameOverlay = new RenameOverlay();

			_OnRenameEnded = onRenameEnded;

			style.position = Position.Absolute;

			RegisterCallback<FocusEvent>(OnFocus);
			RegisterCallback<BlurEvent>(OnBlur);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			_HasFocus = false;

			if (_AttachTarget != null)
			{
				UnregisterCallbackFromAttachElement();

				_AttachTarget = null;
			}
		}

		private bool _HasFocus = false;

		private void OnFocus(FocusEvent evt)
		{
			_HasFocus = true;
		}

		private void OnBlur(BlurEvent evt)
		{
			_HasFocus = false;

			EndRename(true);
		}

		public void BeginRename(string name, int userData, float delay)
		{
			_Renaming = false;

			_RenameOverlay.BeginRename(name, userData, delay);
		}

		public void EndRename(bool acceptChanges)
		{
			if (!_RenameOverlay.IsRenaming())
			{
				return;
			}
			
			_RenameOverlay.EndRename(acceptChanges);

			RenameEnded();
		}

		public bool IsRenaming()
		{
			return _RenameOverlay.IsRenaming();
		}

		private void RenameEnded()
		{
			if (_RenameOverlay.userAcceptedRename)
			{
				if (_OnRenameEnded != null)
				{
					string name = !string.IsNullOrEmpty(_RenameOverlay.name) ? _RenameOverlay.name : _RenameOverlay.originalName;
					_OnRenameEnded(name, _RenameOverlay.userData);
				}
			}
			_RenameOverlay.Clear();

			_Renaming = false;

			RemoveFromHierarchy();
		}

		public VisualElement focusAfterComfirm;

		private bool _Renaming = false;

		private void OnGUI()
		{
			if (!_RenameOverlay.IsRenaming())
			{
				return;
			}

			if (!_Renaming)
			{
				if (!_RenameOverlay.isWaitingForDelay)
				{
					if (_AttachTarget != null)
					{
						_AttachTarget.visible = false;
					}
					_Renaming = true;
				}
			}

			if (!_HasFocus && Event.current.type != EventType.Layout)
			{
				Focus();
			}

			_RenameOverlay.OnEvent();

			_RenameOverlay.editFieldRect = contentRect;
			if (_RenameOverlay.OnGUI(BuiltInStyles.renameTextField))
			{
				return;
			}

			var focusedElement = focusController?.focusedElement;

			RenameEnded();

			if (focusAfterComfirm != null)
			{
				focusedElement?.Blur();
				focusAfterComfirm.Focus();
			}

			GUIUtility.ExitGUI();
		}
	}
}