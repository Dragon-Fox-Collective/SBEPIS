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
	internal sealed class ResizerElement : VisualElement, INotifyValueChanged<float>
	{
		private float _Value = 0f;
		public float value
		{
			get
			{
				return _Value;
			}
			set
			{
				if (_Value != value)
				{
					using (var e = ChangeEvent<float>.GetPooled(_Value, value))
					{
						SetValueWithoutNotify(value);

						e.target = this;
						SendEvent(e);
					}
				}
			}
		}

		private float _MinSize = 0f;
		public float minSize
		{
			get
			{
				return _MinSize;
			}
			set
			{
				_MinSize = value;
			}
		}

		private float _MaxSize = float.MaxValue;
		public float maxSize
		{
			get
			{
				return _MaxSize;
			}
			set
			{
				_MaxSize = value;
			}
		}

		private VisualElement _ContentContainer;

		public override VisualElement contentContainer
		{
			get
			{
				return _ContentContainer;
			}
		}

		public ResizerElement()
		{
			AddToClassList("resizer");

			_ContentContainer = new VisualElement()
			{
				pickingMode = PickingMode.Ignore,
			};
			hierarchy.Add(_ContentContainer);

			VisualElement dragHandle = new VisualElement()
			{
				pickingMode = PickingMode.Ignore,
			};
			dragHandle.AddToClassList("drag-handle");
			hierarchy.Add(dragHandle);

			this.AddManipulator(new ResizeManipulator());
		}

		public void SetValueWithoutNotify(float value)
		{
			_Value = value;
		}

		sealed class ResizeManipulator : DragManipulator
		{
			private Vector2 _StartWorldMousePosition;
			private float _StartValue;
			private bool _IsDragged;

			public ResizeManipulator()
			{
				activators.Add(new ManipulatorActivationFilter() { button = MouseButton.LeftMouse });
			}

			protected override void OnMouseDown(MouseDownEvent e)
			{
				var resizerElement = target as ResizerElement;
				_StartWorldMousePosition = (e.currentTarget as VisualElement).LocalToWorld(e.localMousePosition);

				float currentValue = resizerElement.value;
				_StartValue = currentValue>0f? currentValue: 0f;
				_IsDragged = false;

				e.StopPropagation();
			}

			protected override void OnMouseMove(MouseMoveEvent e)
			{
				var resizerElement = target as ResizerElement;
				Vector2 worldMousePosition = (e.currentTarget as VisualElement).LocalToWorld(e.localMousePosition);
				float delta = (_StartWorldMousePosition.y - worldMousePosition.y);
				if (!_IsDragged)
				{
					_IsDragged = Mathf.Abs(delta) >= 6f;
				}

				if (!_IsDragged)
				{
					return;
				}

				float nextValue = Mathf.Clamp(_StartValue + delta, 0, resizerElement.maxSize);
				if (nextValue < resizerElement.minSize)
				{
					if (nextValue < resizerElement.minSize / 2)
					{
						resizerElement.value = -resizerElement.minSize;
					}
				}
				else
				{
					resizerElement.value = nextValue;
				}
			}

			protected override void OnMouseUp(MouseUpEvent e)
			{
				if (!_IsDragged)
				{
					var resizerElement = target as ResizerElement;
					resizerElement.value = -resizerElement.value;
				}
			}

			protected override void OnEndDrag()
			{
				base.OnEndDrag();

				_IsDragged = false;
			}
		}
	}
}