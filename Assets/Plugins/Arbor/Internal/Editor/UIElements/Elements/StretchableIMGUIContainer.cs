//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;

namespace ArborEditor.UIElements
{
	internal sealed class StretchableIMGUIContainer : IMGUIContainer
	{
		public enum StretchMode
		{
			None,
			Flex,
			Absolute,
		}

		public delegate bool ContainsPointCallback(Vector2 localPoint);

		public event ContainsPointCallback containsPointCallback;

		public StretchableIMGUIContainer(System.Action onGUIHandler, StretchMode stretchMode) : base(onGUIHandler)
		{
			switch (stretchMode)
			{
				case StretchMode.None:
					break;
				case StretchMode.Flex:
					style.flexGrow = 1f;
					break;
				case StretchMode.Absolute:
					this.StretchToParentSize();
					break;
			}
		}

		public override bool ContainsPoint(Vector2 localPoint)
		{
			Matrix4x4 graphMatrix = transform.matrix.inverse;
			Vector2 min = graphMatrix.MultiplyPoint(layout.min);
			Vector2 max = graphMatrix.MultiplyPoint(layout.max);
			Rect localLayout = Rect.MinMaxRect(min.x, min.y, max.x, max.y);

			return localLayout.Contains(localPoint) && (containsPointCallback!=null ? containsPointCallback(localPoint) : true );
		}

#if ARBOR_DEBUG
		bool VisualElementCanGrabFocus()
		{
			return enabledInHierarchy && canGrabFocus;
		}

		public void DebugDefaultAction(EventBase evt)
		{
			System.Reflection.FieldInfo hasFocusableControlsFieldInfo = typeof(IMGUIContainer).GetField("hasFocusableControls", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			System.Reflection.PropertyInfo imguiKeyboardControlPropertyInfo = typeof(FocusController).GetProperty("imguiKeyboardControl", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			bool hasFocusableControls = (bool)hasFocusableControlsFieldInfo.GetValue(this);
			int immguiKeyboardControl = focusController != null? (int)imguiKeyboardControlPropertyInfo.GetValue(focusController, null) : 0;

			long eventTypeId = evt.eventTypeId;
			if (eventTypeId == BlurEvent.TypeId())
			{
				Debug.Log(name + " : BlurEvent , " + base.canGrabFocus + " , " + VisualElementCanGrabFocus() + " , " + hasFocusableControls + " , " + immguiKeyboardControl + " , " + GUIUtility.keyboardControl);
			}
			else if (eventTypeId == FocusEvent.TypeId())
			{
				Debug.Log(name + " : FocusEvent , " + base.canGrabFocus + " , " + VisualElementCanGrabFocus() + " , " + hasFocusableControls + " , " + immguiKeyboardControl + " , " + GUIUtility.keyboardControl);
			}
		}

		protected override void ExecuteDefaultAction(EventBase evt)
		{
			DebugDefaultAction(evt);

			base.ExecuteDefaultAction(evt);
		}
#endif
	}
}