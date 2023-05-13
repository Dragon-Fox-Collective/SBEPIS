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
	using ArborEditor.UnityEditorBridge.UIElements;
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	public interface IDropElement : IEventHandler
	{
	}

	public sealed class DragEventPropagation : Manipulator
	{
		private VisualElement _PrevDropElement;
		private VisualElement _CurrentDropElement;
		private HashSet<EventBase> _SkipEvents = new HashSet<EventBase>();

		protected override void RegisterCallbacksOnTarget()
		{
			target.RegisterCallback<DragEnterEvent>(OnDragEnter, TrickleDown.TrickleDown);
			target.RegisterCallback<DragUpdatedEvent>(OnDragUpdated, TrickleDown.TrickleDown);
			target.RegisterCallback<DragPerformEvent>(OnDragPerform, TrickleDown.TrickleDown);
			target.RegisterCallback<DragLeaveEvent>(OnDragLeave, TrickleDown.TrickleDown);
			target.RegisterCallback<DragExitedEvent>(OnDragExited, TrickleDown.TrickleDown);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			target.UnregisterCallback<DragEnterEvent>(OnDragEnter, TrickleDown.TrickleDown);
			target.UnregisterCallback<DragUpdatedEvent>(OnDragUpdated, TrickleDown.TrickleDown);
			target.UnregisterCallback<DragPerformEvent>(OnDragPerform, TrickleDown.TrickleDown);
			target.UnregisterCallback<DragLeaveEvent>(OnDragLeave, TrickleDown.TrickleDown);
			target.UnregisterCallback<DragExitedEvent>(OnDragExited, TrickleDown.TrickleDown);
		}

		static VisualElement PerformPickDropElement(VisualElement root, VisualElement target, Vector2 point)
		{
			if (root.resolvedStyle.display == DisplayStyle.None)
			{
				return null;
			}

			bool isDropElement = root is IDropElement;

			if (!isDropElement && root.pickingMode == PickingMode.Ignore && root.hierarchy.childCount == 0)
			{
				return null;
			}

			if (!root.GetWorldBoundingBox().Contains(point))
			{
				return null;
			}

			Vector2 localPoint = root.WorldToLocal(point);
			bool containsPoint = root.ContainsPoint(localPoint);
			if (!containsPoint && VisualElementBridge.ShouldClip(root))
			{
				return null;
			}

			var cCount = root.hierarchy.childCount;
			for (int i = cCount - 1; i >= 0; i--)
			{
				var child = root.hierarchy[i];
				var result = PerformPickDropElement(child, target, point);
				if (result != null && result.visible)
				{
					return result;
				}
			}

			if (root == target)
			{
				return root;
			}

			if (root.enabledInHierarchy && root.visible && (isDropElement || root.pickingMode == PickingMode.Position) && containsPoint)
			{
				return root;
			}

			return null;
		}

		static bool IsDropElement(VisualElement element)
		{
			return element is IDropElement && element.pickingMode == PickingMode.Ignore;
		}

		static bool HasElement(VisualElement root, VisualElement target)
		{
			if (root == target)
			{
				return true;
			}

			for (int i = 0; i < root.hierarchy.childCount; i++)
			{
				var child = root.hierarchy[i];
				if (HasElement(child, target))
				{
					return true;
				}
			}

			return false;
		}

		void ProcessDragEventPropagation(EventBase e)
		{
			if (!PreProcessEvent(e))
			{
				return;
			}

			IMouseEvent mouseEvent = e as IMouseEvent;
			if (mouseEvent == null)
			{
				return;
			}

			VisualElement dropTarget = null;
			if (e.eventTypeId != DragLeaveEvent.TypeId() || e.propagationPhase != PropagationPhase.AtTarget || e.target != target)
			{
				dropTarget = PerformPickDropElement(target, e.target as VisualElement, mouseEvent.mousePosition);
			}

			if (_CurrentDropElement != dropTarget)
			{
				if (IsDropElement(_CurrentDropElement) || IsDropElement(dropTarget))
				{
					SendEnterLeave<DragLeaveEvent, DragEnterEvent>(_CurrentDropElement, dropTarget, mouseEvent);
				}
			}

			if (dropTarget is IDropElement)
			{
				var eventTypeId = e.eventTypeId;
				if (eventTypeId == DragExitedEvent.TypeId())
				{
					using (var exitedEvent = DragExitedEvent.GetPooled(mouseEvent))
					{
						exitedEvent.target = dropTarget;
						dropTarget.SendEvent(exitedEvent);
						_SkipEvents.Add(exitedEvent);
					}
				}
				else if (eventTypeId == DragUpdatedEvent.TypeId())
				{
					using (var updatedEvent = DragUpdatedEvent.GetPooled(mouseEvent))
					{
						updatedEvent.target = dropTarget;
						dropTarget.SendEvent(updatedEvent);
						_SkipEvents.Add(updatedEvent);
					}
				}
				else if (eventTypeId == DragPerformEvent.TypeId())
				{
					using (var performEvent = DragPerformEvent.GetPooled(mouseEvent))
					{
						performEvent.target = dropTarget;
						dropTarget.SendEvent(performEvent);
						_SkipEvents.Add(performEvent);
					}
				}

				e.StopImmediatePropagation();
				e.PreventDefault();
			}

			if (_CurrentDropElement != dropTarget)
			{
				_PrevDropElement = _CurrentDropElement;
				_CurrentDropElement = dropTarget;
			}
		}

		void OnDragEnter(DragEnterEvent e)
		{
			ProcessDragEventPropagation(e);
		}

		void OnDragUpdated(DragUpdatedEvent e)
		{
			ProcessDragEventPropagation(e);
		}

		void OnDragPerform(DragPerformEvent e)
		{
			ProcessDragEventPropagation(e);
		}

		void OnDragLeave(DragLeaveEvent e)
		{
			ProcessDragEventPropagation(e);
		}

		void OnDragExited(DragExitedEvent e)
		{
			ProcessDragEventPropagation(e);

			_CurrentDropElement = null;
			_PrevDropElement = null;
		}

		bool PreProcessEvent(EventBase e)
		{
			if (_SkipEvents.Contains(e))
			{
				_SkipEvents.Remove(e);
				return false;
			}

			VisualElement eventTarget = e.target as VisualElement;
			if (IsDropElement(eventTarget))
			{
				if (e.eventTypeId == DragEnterEvent.TypeId() && _PrevDropElement == eventTarget)
				{
					// already sent
					e.StopPropagation();
					e.PreventDefault();
					return false;
				}
			}

			return true;
		}

		void SendEnterLeave<TLeaveEvent, TEnterEvent>(VisualElement previousTopElementUnderMouse, VisualElement currentTopElementUnderMouse, IMouseEvent triggerEvent) where TLeaveEvent : MouseEventBase<TLeaveEvent>, new() where TEnterEvent : MouseEventBase<TEnterEvent>, new()
		{
			if (previousTopElementUnderMouse != null && previousTopElementUnderMouse.panel == null)
			{
				// If previousTopElementUnderMouse has been removed from panel,
				// do as if there is no element under the mouse.
				previousTopElementUnderMouse = null;
			}

			bool canEnterEvent = IsDropElement(currentTopElementUnderMouse);
			//bool canLeaveEvent = IsDropElement(previousTopElementUnderMouse);
			bool canLeaveEvent = true;

			// We want to find the common ancestor CA of previousTopElementUnderMouse and currentTopElementUnderMouse,
			// send Leave (MouseLeave or DragLeave) events to elements between CA and previousTopElementUnderMouse
			// and send Enter (MouseEnter or DragEnter) events to elements between CA and currentTopElementUnderMouse.

			int prevDepth = 0;
			var p = previousTopElementUnderMouse;
			while (p != null)
			{
				prevDepth++;
				p = p.hierarchy.parent;
			}

			int currDepth = 0;
			var c = currentTopElementUnderMouse;
			while (c != null)
			{
				currDepth++;
				c = c.hierarchy.parent;
			}

			p = previousTopElementUnderMouse;
			c = currentTopElementUnderMouse;

			bool isLeaveTarget = false;

			while (prevDepth > currDepth)
			{
				if (canLeaveEvent)
				{
					using (var leaveEvent = MouseEventBase<TLeaveEvent>.GetPooled(triggerEvent))
					{
						leaveEvent.target = p;
						p.SendEvent(leaveEvent);
						if (!isLeaveTarget)
						{
							_SkipEvents.Add(leaveEvent);
						}
					}
				}

				if (p == target)
				{
					isLeaveTarget = true;
				}

				prevDepth--;
				p = p.hierarchy.parent;
			}

			// We want to send enter events after all the leave events.
			// We will store the elements being entered in this list.
			using (Arbor.Pool.ListPool<VisualElement>.Get(out var enteringElements))
			{
				if (enteringElements.Capacity < currDepth)
				{
					enteringElements.Capacity = currDepth;
				}
				
				while (currDepth > prevDepth)
				{
					enteringElements.Add(c);

					currDepth--;
					c = c.hierarchy.parent;
				}

				// Now p and c are at the same depth. Go up the tree until p == c.
				while (p != c)
				{
					if (canLeaveEvent)
					{
						using (var leaveEvent = MouseEventBase<TLeaveEvent>.GetPooled(triggerEvent))
						{
							leaveEvent.target = p;
							p.SendEvent(leaveEvent);
							if (!isLeaveTarget)
							{
								_SkipEvents.Add(leaveEvent);
							}
						}
					}

					enteringElements.Add(c);

					if (p == target)
					{
						isLeaveTarget = true;
					}

					p = p.hierarchy.parent;
					c = c.hierarchy.parent;
				}

				bool isEnterTarget = !isLeaveTarget;

				if (canEnterEvent)
				{
					for (var i = enteringElements.Count - 1; i >= 0; i--)
					{
						var enteringElement = enteringElements[i];

						if (enteringElement == target)
						{
							isEnterTarget = true;
						}

						using (var enterEvent = MouseEventBase<TEnterEvent>.GetPooled(triggerEvent))
						{
							enterEvent.target = enteringElement;
							enteringElement.SendEvent(enterEvent);
							if (isEnterTarget)
							{
								_SkipEvents.Add(enterEvent);
							}
						}
					}
				}
			}
		}
	}
}