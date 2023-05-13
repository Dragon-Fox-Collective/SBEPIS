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
	internal class ResizableElement : VisualElement, INotifyValueChanged<float>
	{
		private ResizerElement _ResizerElement;
		private VisualElement _ContentContainer;

		public VisualElement header
		{
			get
			{
				return _ResizerElement;
			}
		}

		public override VisualElement contentContainer
		{
			get
			{
				return _ContentContainer;
			}
		}

		private float _Size;

		public float value
		{
			get
			{
				return _Size;
			}
			set
			{
				if (_Size != value)
				{
					using (var e = ChangeEvent<float>.GetPooled(_Size, value))
					{
						SetValueWithoutNotify(value);

						e.target = this;
						SendEvent(e);
					}
				}
			}
		}

		public float minSize
		{
			get
			{
				return _ResizerElement.minSize;
			}
			set
			{
				_ResizerElement.minSize = value;
			}
		}

		public ResizableElement()
		{
			_ResizerElement = new ResizerElement();
			_ResizerElement.RegisterCallback<ChangeEvent<float>>(e =>
			{
				value = e.newValue;
			});
			_ResizerElement.RegisterCallback<GeometryChangedEvent>(e =>
			{
				if (resolvedStyle.display == DisplayStyle.Flex)
				{
					_ResizerHeight = e.newRect.height;
				}
			});
			_ResizerElement.SetValueWithoutNotify(_Size);
			hierarchy.Add(_ResizerElement);

			_ContentContainer = new VisualElement();
			_ContentContainer.style.display = DisplayStyle.None;
			hierarchy.Add(_ContentContainer);

			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
		}

		VisualElement _Parent;
		private float _ResizerHeight;

		void OnAttachToPanel(AttachToPanelEvent e)
		{
			if (_Parent != null)
			{
				_Parent.UnregisterCallback<GeometryChangedEvent>(OnGeometryChangedParent);
				_Parent = null;
			}

			_Parent = hierarchy.parent;
			_Parent.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedParent);
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			if (_Parent != null)
			{
				_Parent.UnregisterCallback<GeometryChangedEvent>(OnGeometryChangedParent);
				_Parent = null;
			}
		}

		void OnGeometryChangedParent(GeometryChangedEvent e)
		{
			int index = hierarchy.parent.IndexOf(this);
			VisualElement previousElement = index >= 1 ? hierarchy.parent[index-1] : null;
			float minRemainingSize = previousElement.layout.y + previousElement.resolvedStyle.minHeight.value;

			_ResizerElement.maxSize = e.newRect.height - minRemainingSize - _ResizerHeight;
			if (_ResizerElement.maxSize >= _ResizerElement.minSize)
			{
				style.display = StyleKeyword.Null;
				float currentSize = _Size;
				if (currentSize >= 0f)
				{
					float height = Mathf.Min(currentSize, _ResizerElement.maxSize);
					_ContentContainer.style.height = height;
					_ResizerElement.SetValueWithoutNotify(height);
				}
			}
			else
			{
				style.display = DisplayStyle.None;
			}
		}

		public void SetValueWithoutNotify(float value)
		{
			_Size = value;

			_ResizerElement.SetValueWithoutNotify(value);

			if (_Size >= 0)
			{
				_ContentContainer.style.display = StyleKeyword.Null;
				_ContentContainer.style.height = _Size;
			}
			else
			{
				_ContentContainer.style.display = DisplayStyle.None;
			}
		}
	}
}