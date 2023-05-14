//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	public class DataSlotProperty : IPropertyChanged
	{
		// Paths
		private const string kNodeGraphPath = "nodeGraph";

		private SerializedProperty _NodeGraphProperty;

		private DataSlot _DataSlot = null;

		private bool _IsSetCallback = false;

		public SerializedProperty property
		{
			get;
			private set;
		}

		public SerializedProperty nodeGraphProperty
		{
			get
			{
				if (_NodeGraphProperty == null)
				{
					_NodeGraphProperty = property.FindPropertyRelative(kNodeGraphPath);
				}
				return _NodeGraphProperty;
			}
		}

		public NodeGraph nodeGraph
		{
			get
			{
				return nodeGraphProperty.objectReferenceValue as NodeGraph;
			}
			set
			{
				nodeGraphProperty.objectReferenceValue = value;
			}
		}

		public DataSlot slot
		{
			get
			{
				if (_DataSlot == null)
				{
					if (property != null)
					{
						_DataSlot = SerializedPropertyUtility.GetPropertyObject<DataSlot>(property);
						if (_DataSlot != null)
						{
							NodeBehaviour nodeBehaviour = property.serializedObject.targetObject as NodeBehaviour;
							if (nodeBehaviour != null)
							{
								nodeBehaviour.RebuildDataSlotFieldIfNecessary(_DataSlot);
							}
						}
					}
					EnableCallback();
				}
				return _DataSlot;
			}
		}

		[System.Obsolete("use slot")]
		public DataSlotField dataSlotField
		{
			get
			{
				DataSlot slot = this.slot;
				if (slot != null)
				{
					return slot.slotField;
				}
				return null;
			}
		}

		public DataSlotProperty(SerializedProperty property)
		{
			this.property = property;
		}

		public virtual void Clear()
		{
			nodeGraph = null;
		}

		public virtual void Disconnect()
		{
		}

		void EnableCallback()
		{
			if (_IsSetCallback)
			{
				return;
			}

			EditorCallbackUtility.RegisterPropertyChanged(this);
			_IsSetCallback = true;
		}

		void DisableCallback()
		{
			if (!_IsSetCallback)
			{
				return;
			}

			EditorCallbackUtility.UnregisterPropertyChanged(this);
			_IsSetCallback = false;
		}

		void IPropertyChanged.OnPropertyChanged(PropertyChangedType propertyChangedType)
		{
			_DataSlot = null;

			DisableCallback();
		}
	}

	public class InputSlotBaseProperty : DataSlotProperty
	{
		// Paths
		private const string kBranchIdPath = "branchID";

		private SerializedProperty _BranchIDProperty;

		public SerializedProperty branchIDProperty
		{
			get
			{
				if (_BranchIDProperty == null)
				{
					_BranchIDProperty = property.FindPropertyRelative(kBranchIdPath);
				}
				return _BranchIDProperty;
			}
		}

		public int branchID
		{
			get
			{
				return branchIDProperty.intValue;
			}
			set
			{
				branchIDProperty.intValue = value;
			}
		}

		public InputSlotBaseProperty(SerializedProperty property) : base(property)
		{
		}

		public override void Clear()
		{
			base.Clear();

			branchID = 0;
		}

		public override void Disconnect()
		{
			NodeGraph nodeGraph = this.nodeGraph;
			if (nodeGraph == null)
			{
				return;
			}

			int branchID = this.branchID;
			DataBranch branch = nodeGraph.GetDataBranchFromID(branchID);
			if (branch != null)
			{
				nodeGraph.DeleteDataBranch(branch, property.serializedObject.targetObject);

				this.nodeGraph = null;
				this.branchID = 0;
			}
		}
	}

	public sealed class InputSlotTypableProperty : InputSlotBaseProperty
	{
		// Paths
		private const string kTypePath = "_Type";

		private ClassTypeReferenceProperty _TypeProperty;

		public ClassTypeReferenceProperty typeProperty
		{
			get
			{
				if (_TypeProperty == null)
				{
					_TypeProperty = new ClassTypeReferenceProperty(property.FindPropertyRelative(kTypePath));
				}
				return _TypeProperty;
			}
		}

		public System.Type type
		{
			get
			{
				return typeProperty.type;
			}
			set
			{
				typeProperty.type = value;
			}
		}

		public InputSlotTypableProperty(SerializedProperty property) : base(property)
		{
		}

		public override void Clear()
		{
			base.Clear();

			typeProperty.Clear();
		}
	}

	public class OutputSlotBaseProperty : DataSlotProperty
	{
		// Paths
		private const string kBranchIDsPath = "branchIDs";

		private SerializedProperty _BranchIDsProperty;

		public SerializedProperty branchIDsProperty
		{
			get
			{
				if (_BranchIDsProperty == null)
				{
					_BranchIDsProperty = property.FindPropertyRelative(kBranchIDsPath);
				}
				return _BranchIDsProperty;
			}
		}

		public OutputSlotBaseProperty(SerializedProperty property) : base(property)
		{
		}

		public override void Clear()
		{
			base.Clear();

			branchIDsProperty.ClearArray();
		}

		public bool IsConnected()
		{
			NodeGraph nodeGraph = this.nodeGraph;
			if (nodeGraph == null)
			{
				return false;
			}

			for (int i = 0; i < branchIDsProperty.arraySize; ++i)
			{
				int branchID = branchIDsProperty.GetArrayElementAtIndex(i).intValue;

				DataBranch branch = nodeGraph.GetDataBranchFromID(branchID);
				if (branch != null)
				{
					return true;
				}
			}

			return false;
		}

		public override void Disconnect()
		{
			NodeGraph nodeGraph = this.nodeGraph;
			if (nodeGraph == null)
			{
				return;
			}

			for (int i = 0; i < branchIDsProperty.arraySize; ++i)
			{
				int branchID = branchIDsProperty.GetArrayElementAtIndex(i).intValue;

				DataBranch branch = nodeGraph.GetDataBranchFromID(branchID);
				if (branch != null)
				{
					nodeGraph.DeleteDataBranch(branch, property.serializedObject.targetObject);
				}
			}

			this.nodeGraph = null;
			branchIDsProperty.ClearArray();
		}
	}

	public sealed class OutputSlotTypableProperty : OutputSlotBaseProperty
	{
		// Paths
		private const string kTypePath = "_Type";

		private ClassTypeReferenceProperty _Type;

		public ClassTypeReferenceProperty typeProperty
		{
			get
			{
				if (_Type == null)
				{
					_Type = new ClassTypeReferenceProperty(property.FindPropertyRelative(kTypePath));
				}
				return _Type;
			}
		}

		public System.Type type
		{
			get
			{
				return typeProperty.type;
			}
			set
			{
				typeProperty.type = value;
			}
		}

		public OutputSlotTypableProperty(SerializedProperty property) : base(property)
		{
		}

		public override void Clear()
		{
			base.Clear();

			typeProperty.Clear();
		}
	}

	public sealed class OutputSlotComponentProperty : OutputSlotBaseProperty
	{
		// Paths
		private const string kTypePath = "_Type";

		private ClassTypeReferenceProperty _Type;

		public ClassTypeReferenceProperty typeProperty
		{
			get
			{
				if (_Type == null)
				{
					_Type = new ClassTypeReferenceProperty(property.FindPropertyRelative(kTypePath));
				}
				return _Type;
			}
		}

		public System.Type type
		{
			get
			{
				return typeProperty.type;
			}
			set
			{
				typeProperty.type = value;
			}
		}

		public OutputSlotComponentProperty(SerializedProperty property) : base(property)
		{
		}

		public override void Clear()
		{
			base.Clear();

			typeProperty.Clear();
		}
	}
}