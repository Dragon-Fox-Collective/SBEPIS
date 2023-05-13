//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	public abstract class FlexibleFieldPropertyBase
	{
		// Paths
		private const string kTypePath = "_Type";
		private const string kValuePath = "_Value";
		private const string kParameterPath = "_Parameter";
		private const string kSlotPath = "_Slot";

		private SerializedProperty _Type;
		private SerializedProperty _Value;
		private ParameterReferenceProperty _Parameter;
		private InputSlotBaseProperty _Slot;

		public SerializedProperty property
		{
			get;
			private set;
		}

		public SerializedProperty typeProperty
		{
			get
			{
				if (_Type == null)
				{
					_Type = property.FindPropertyRelative(kTypePath);
				}
				return _Type;
			}
		}

		public SerializedProperty valueProperty
		{
			get
			{
				if (_Value == null)
				{
					_Value = property.FindPropertyRelative(kValuePath);
				}
				return _Value;
			}
		}

		public ParameterReferenceProperty parameterProperty
		{
			get
			{
				if (_Parameter == null)
				{
					_Parameter = new ParameterReferenceProperty(property.FindPropertyRelative(kParameterPath));
				}
				return _Parameter;
			}
		}

		public InputSlotBaseProperty slotProperty
		{
			get
			{
				if (_Slot == null)
				{
					_Slot = new InputSlotBaseProperty(property.FindPropertyRelative(kSlotPath));
				}
				return _Slot;
			}
		}

		public FlexibleFieldPropertyBase(SerializedProperty property)
		{
			this.property = property;
		}

		public void Clear(bool clearType)
		{
			if (clearType)
			{
				ClearType();
			}

			valueProperty.Clear();
			parameterProperty.Clear();
			slotProperty.Clear();

			OnClear();
		}

		public void ClearSlot()
		{
			parameterProperty.slotProperty.Clear();
			slotProperty.Clear();
		}

		public void Disconnect()
		{
			if (IsSlotType())
			{
				slotProperty.Disconnect();
			}
			else if (IsParameterType())
			{
				parameterProperty.Disconnect();
			}
		}

		protected virtual void OnClear()
		{
		}

		protected abstract void ClearType();
		public abstract void SetSlotType();
		public abstract bool IsSlotType();
		public abstract bool IsParameterType();

		public bool IsShowOutsideSlot()
		{
			if (IsSlotType())
			{
				return false;
			}

			switch (ArborSettings.dataSlotShowMode)
			{
				case DataSlotShowMode.Outside:
					return true;
				case DataSlotShowMode.Inside:
				case DataSlotShowMode.Flexibly:
					NodeBehaviour nodeBehaviour = property.serializedObject.targetObject as NodeBehaviour;
					if (nodeBehaviour != null)
					{
						if (DataSlotGUI.IsDragSlotConnectable(nodeBehaviour.node, slotProperty.slot, nodeBehaviour))
						{
							return true;
						}
					}
					break;
			}

			return false;
		}
	}
}
