//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace ArborEditor
{
	using Arbor;

	public abstract class OwnsDataSlotPropertyEditor : PropertyEditor, IPropertyChanged
	{
		private DataSlot _DataSlot = null;

		private bool _IsSetCallback = false;
		private bool _IsDirtyCallback = false;

		protected abstract DataSlotProperty slotProperty
		{
			get;
		}

		protected void EnableConnectionChanged()
		{
			_DataSlot = slotProperty.slot;
			if (_DataSlot != null)
			{
				EditorCallbackUtility.RegisterPropertyChanged(this);

				_DataSlot.onConnectionChanged += OnConnectionChanged;

				_IsSetCallback = true;
			}
		}

		protected void DisableConnectionChanged()
		{
			if (_DataSlot != null)
			{
				if (_IsSetCallback)
				{
					_DataSlot.onConnectionChanged -= OnConnectionChanged;

					EditorCallbackUtility.UnregisterPropertyChanged(this);

					_IsSetCallback = false;
				}

				_DataSlot = null;
			}
		}

		void UpdateCallback()
		{
			if (_IsDirtyCallback)
			{
				if (_IsSetCallback)
				{
					DisableConnectionChanged();
				}
				EnableConnectionChanged();

				if (_IsSetCallback)
				{
					_IsDirtyCallback = false;
				}
			}
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();

			_IsDirtyCallback = true;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			DisableConnectionChanged();
		}

		protected abstract void DoGUI(Rect position, GUIContent label);

		protected override sealed void OnGUI(Rect position, GUIContent label)
		{
			UpdateCallback();

			DoGUI(position, label);
		}

		void IPropertyChanged.OnPropertyChanged(PropertyChangedType propertyChangedType)
		{
			_IsDirtyCallback = true;
		}

		protected abstract void OnConnectionChanged(bool isConnect);
	}
}