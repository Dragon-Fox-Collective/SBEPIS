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
	internal sealed class DirectionElement : VisualElement, INotifyValueChanged<Vector2>
	{
		private Vector2 _Direction = Vector2.up;

		public Vector2 value
		{
			get
			{
				return _Direction;
			}
			set
			{
				if (_Direction != value)
				{
					using (var e = ChangeEvent<Vector2>.GetPooled(_Direction, value))
					{
						e.target = this;

						SetValueWithoutNotify(value);

						SendEvent(e);
					}
				}
			}
		}

		public bool isDragging
		{
			get
			{
				return _DragManipulator.isActive;
			}
		}

		private ArrowCapElement _ArrowCapElement;
		private ArrowDragManipulator _DragManipulator;
		private bool _IsLayouted;
		private bool _IsHover;

		private Color _ArrowColor = Color.white;

		public Color arrowColor
		{
			get
			{
				return _ArrowColor;
			}
			set
			{
				if (_ArrowColor != value)
				{
					_ArrowColor = value;

					if (!_IsHover && !_DragManipulator.isActive)
					{
						_ArrowCapElement.color = _ArrowColor;
					}
				}
			}
		}

		public DirectionElement()
		{
			pickingMode = PickingMode.Ignore;

			_ArrowCapElement = new ArrowCapElement()
			{
				width = 8f,
			};
			Add(_ArrowCapElement);
			
			_DragManipulator = new ArrowDragManipulator(this);
			_ArrowCapElement.AddManipulator(_DragManipulator);
			_ArrowCapElement.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
			_ArrowCapElement.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);

			RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
		}

		void OnMouseEnter(MouseEnterEvent e)
		{
			_IsHover = true;

			_ArrowCapElement.color = Color.cyan;
		}

		void OnMouseLeave(MouseLeaveEvent e)
		{
			_IsHover = false;

			if (!_DragManipulator.isActive)
			{
				_ArrowCapElement.color = _ArrowColor;
			}
		}

		public void SetValueWithoutNotify(Vector2 newValue)
		{
			_Direction = newValue;

			UpdateArrowCap();
		}

		void UpdateArrowCap()
		{
			if (_IsLayouted)
			{
				_ArrowCapElement.position = contentRect.center + _Direction * 8f;
				_ArrowCapElement.direction = _Direction;
				_ArrowCapElement.UpdateLayout();
			}
		}

		void OnGeometryChanged(GeometryChangedEvent e)
		{
			_IsLayouted = true;
			UpdateArrowCap();
		}

		sealed class ArrowDragManipulator : DragManipulator
		{
			private Vector2 _BeginDirection;
			private DirectionElement _DirectionElement;

			public ArrowDragManipulator(DirectionElement directionElement)
			{
				_DirectionElement = directionElement;

				activators.Add(new ManipulatorActivationFilter() { button = MouseButton.LeftMouse });
			}

			protected override void RegisterCallbacksOnTarget()
			{
				base.RegisterCallbacksOnTarget();
				target.RegisterCallback<KeyDownEvent>(OnKeyDown);
			}

			protected override void UnregisterCallbacksFromTarget()
			{
				base.UnregisterCallbacksFromTarget();
				target.UnregisterCallback<KeyDownEvent>(OnKeyDown);
			}

			protected override void OnMouseDown(MouseDownEvent e)
			{
				_BeginDirection = _DirectionElement.value;
				_DirectionElement._ArrowCapElement.color = Color.cyan;
				e.StopPropagation();
			}

			protected override void OnMouseMove(MouseMoveEvent e)
			{
				Vector2 mousePos = (e.target as VisualElement).ChangeCoordinatesTo(_DirectionElement, e.localMousePosition);
				_DirectionElement.value = (mousePos - _DirectionElement.contentRect.center).normalized;
				e.StopPropagation();
			}

			protected override void OnEndDrag()
			{
				base.OnEndDrag();

				_DirectionElement._ArrowCapElement.color = _DirectionElement.arrowColor;
			}

			void OnKeyDown(KeyDownEvent e)
			{
				if (!isActive || e.keyCode != KeyCode.Escape)
				{
					return;
				}

				_DirectionElement.value = _BeginDirection;
				
				EndDrag();
				e.StopPropagation();
			}
		}
	}
}