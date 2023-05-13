//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

namespace Arbor
{
	using Arbor.Extensions;

#if ARBOR_DOC_JA
	/// <summary>
	/// ArborFSMの各種ノードに割り当てるスクリプトの基本クラス。
	/// </summary>
#else
	/// <summary>
	/// Base class for scripts to be assigned to various nodes of ArborFSM.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[HideType(true)]
	public class NodeBehaviour : MonoBehaviour, ISerializationCallbackReceiver
	{
		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_StateMachine")]
		private NodeGraph _NodeGraph;

		[SerializeField]
		[FormerlySerializedAs("_StateID")]
		[FormerlySerializedAs("_CalculatorID")]
		[HideInInspector]
		private int _NodeID;

		[SerializeField]
#if !ARBOR_DEBUG
		[HideInInspector]
#endif
		private List<DataLinkFieldInfo> _DataSlotFieldLinks = new List<DataLinkFieldInfo>();

#if ARBOR_DOC_JA
		/// <summary>
		/// ArborEditorWindow上での開閉状態。
		/// </summary>
#else
		/// <summary>
		/// Expanded on ArborEditorWindow.
		/// </summary>
#endif
		[HideInInspector]
		public bool expanded = true;

		private static DataSlot[] s_EmptyDataSlots = new DataSlot[0];

		private DataSlot[] _DataSlots = s_EmptyDataSlots;

		private static Node s_CreatingNode;

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeGraphを取得。
		/// </summary>
#else
		/// <summary>
		/// Gets the NodeGraph.
		/// </summary>
#endif
		public NodeGraph nodeGraph
		{
			get
			{
				if (_NodeGraph == null)
				{
					if (gameObject != null)
					{
						var nodeGraphs = gameObject.GetComponentsTemp<NodeGraph>();
						for (int graphIndex = 0; graphIndex < nodeGraphs.Count; graphIndex++)
						{
							NodeGraph nodeGraph = nodeGraphs[graphIndex];
							Node c = nodeGraph.FindNodeContainsBehaviour(this);
							if (c != null)
							{
								_NodeGraph = nodeGraph;
								_NodeID = c.nodeID;
								break;
							}
						}
					}
				}
				if (_NodeGraph == null && s_CreatingNode != null)
				{
					_NodeGraph = s_CreatingNode.nodeGraph;
					_NodeID = s_CreatingNode.nodeID;
				}
				return _NodeGraph;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードIDを取得。
		/// </summary>
#else
		/// <summary>
		/// Gets the node identifier.
		/// </summary>
#endif
		public int nodeID
		{
			get
			{
				if (_NodeID == 0 && s_CreatingNode != null)
				{
					_NodeID = s_CreatingNode.nodeID;
				}
				return _NodeID;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Nodeを取得。
		/// </summary>
#else
		/// <summary>
		/// Get the Node.
		/// </summary>
#endif
		public Node node
		{
			get
			{
				Node node = null;

				NodeGraph graph = nodeGraph;
				if (graph != null)
				{
					node = graph.GetNodeFromID(nodeID);
				}

				if (node == null && s_CreatingNode != null)
				{
					node = s_CreatingNode;
				}

				return node;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// OnValidateのときに呼ばれるコールバック
		/// </summary>
#else
		/// <summary>
		/// Callback called during OnValidate
		/// </summary>
#endif
		public event System.Action onValidate;

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数はスクリプトがロードされた時やインスペクターの値が変更されたときに呼び出される（この呼出はエディター上のみ）
		/// </summary>
#else
		/// <summary>
		/// This function is called when the script is loaded or when the inspector value changes (this call is only in the editor)
		/// </summary>
#endif
		protected virtual void OnValidate()
		{
			onValidate?.Invoke();

			if (nodeGraph != null)
			{
				nodeGraph.DelayRefresh();
			}
		}

		void Initialize(Node node, bool duplicate)
		{
			SetHideFlags(this);

			Initialize(node.nodeGraph, node.nodeID);

			RebuildFields();

			if (!duplicate)
			{
				OnCreated();
			}

			OnInitializeEnabled();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 内部用。
		/// </summary>
#else
		/// <summary>
		/// For internal.
		/// </summary>
#endif
		public void RebuildFields()
		{
			AssignFields();

			RebuildDataSlotFields();

			OnRebuildFields();
		}

		internal void RefreshDataSlots()
		{
			SetupDataBranchSlotField();

			int slotCount = _DataSlots.Length;
			for (int slotIndex = 0; slotIndex < slotCount; slotIndex++)
			{
				DataSlot slot = _DataSlots[slotIndex];
				if (slot == null)
				{
					continue;
				}

				slot.RefreshDataBranchSlot();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールドに関するデータを再構築する際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// It is called when reconstructing data about fields.
		/// </summary>
#endif
		protected virtual void OnRebuildFields()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor用。
		/// </summary>
		/// <param name="node">ノード</param>
		/// <param name="type">型</param>
		/// <param name="duplicate">複製したか</param>
		/// <returns>作成したNodeBehaviourを返す。</returns>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
		/// <param name="node">Node</param>
		/// <param name="type">Type</param>
		/// <param name="duplicate">Whether duplicated</param>
		/// <returns>Returns the created NodeBehaviour.</returns>
#endif
		public static NodeBehaviour CreateNodeBehaviour(Node node, System.Type type, bool duplicate = false)
		{
			System.Type classType = typeof(NodeBehaviour);
			if (type != classType && !TypeUtility.IsSubclassOf(type, classType))
			{
				throw new System.ArgumentException("The type `" + type.Name + "' must be convertible to `NodeBehaviour' in order to use it as parameter `type'", "type");
			}

			s_CreatingNode = node;

			NodeBehaviour nodeBehaviour = ComponentUtility.AddComponent(node.nodeGraph.gameObject, type) as NodeBehaviour;

			ComponentUtility.RecordObject(nodeBehaviour, "Add " + type.Name);

			nodeBehaviour.Initialize(node, duplicate);

			ComponentUtility.SetDirty(nodeBehaviour);

			s_CreatingNode = null;

			return nodeBehaviour;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor用。
		/// </summary>
		/// <typeparam name="Type">型</typeparam>
		/// <param name="node">ノード</param>
		/// <param name="duplicate">複製したか</param>
		/// <returns>作成したNodeBehaviourを返す。</returns>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
		/// <typeparam name="Type">Type</typeparam>
		/// <param name="node">Node</param>
		/// <param name="duplicate">Whether duplicated</param>
		/// <returns>Returns the created NodeBehaviour.</returns>
#endif
		public static Type CreateNodeBehaviour<Type>(Node node, bool duplicate = false) where Type : NodeBehaviour
		{
			s_CreatingNode = node;

			Type nodeBehaviour = ComponentUtility.AddComponent<Type>(node.nodeGraph.gameObject);

			ComponentUtility.RecordObject(nodeBehaviour, "Add " + nodeBehaviour.GetType().Name);

			nodeBehaviour.Initialize(node, duplicate);

			ComponentUtility.SetDirty(nodeBehaviour);

			s_CreatingNode = null;

			return nodeBehaviour;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBehaviourを破棄する。
		/// </summary>
		/// <param name="behaviour">NodeBehaviour</param>
#else
		/// <summary>
		/// Destroy NodeBehaviour.
		/// </summary>
		/// <param name="behaviour">NodeBehaviour</param>
#endif
		public static void Destroy(NodeBehaviour behaviour)
		{
			if (behaviour != null)
			{
				NodeGraph rootGraph = null;

				bool isSubGraphBehaviour = behaviour is ISubGraphBehaviour;
				if (isSubGraphBehaviour)
				{
					rootGraph = behaviour.nodeGraph.rootGraph;
					if (rootGraph != null)
					{
						rootGraph.disableCallbackGraphTree = true;
					}
				}

				try
				{
					behaviour.OnPreDestroy();
				}
				catch (System.Exception ex)
				{
					Debug.LogException(ex);
				}

				if (isSubGraphBehaviour && rootGraph != null)
				{
					rootGraph.disableCallbackGraphTree = false;
					rootGraph.ChangedGraphTree();
				}
			}

			ComponentUtility.Destroy(behaviour);
		}

		internal bool attachedNode = false;
		internal event System.Action delayAttachToNode;

		internal void OnAttachToNode()
		{
			if (!attachedNode)
			{
				attachedNode = true;
				delayAttachToNode?.Invoke();
				delayAttachToNode = null;
			}
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			INodeBehaviourSerializationCallbackReceiver receiver = this as INodeBehaviourSerializationCallbackReceiver;
			if (receiver != null)
			{
				receiver.OnBeforeSerialize();
			}
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			INodeBehaviourSerializationCallbackReceiver receiver = this as INodeBehaviourSerializationCallbackReceiver;
			if (receiver != null)
			{
				receiver.OnAfterDeserialize();
			}

			RebuildFields();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataSlotの個数
		/// </summary>
#else
		/// <summary>
		/// Number of DataSlot
		/// </summary>
#endif
		public int dataSlotCount
		{
			get
			{
				return _DataSlots.Length;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataSlotを取得する。
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <returns>DataSlot</returns>
#else
		/// <summary>
		/// Get DataSlot.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>DataSlot</returns>
#endif
		public DataSlot GetDataSlot(int index)
		{
			DataSlot slot = _DataSlots[index];
			if (slot == null)
			{
				Debug.LogError(this.GetType() + " : GetDataSlot(" + index + ") == null");
			}
			return slot;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataSlotが含まれているか判定する。
		/// </summary>
		/// <param name="slot">DataSlot</param>
		/// <returns>含まれている場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Determine if the Data Slot is included.
		/// </summary>
		/// <param name="slot">DataSlot</param>
		/// <returns>Returns true if included.</returns>
#endif
		public bool ContainsSlot(DataSlot slot)
		{
			int count = _DataSlots.Length;
			for (int i = 0; i < count; i++)
			{
				DataSlot s = _DataSlots[i];
				if (s != null && object.ReferenceEquals(s, slot))
				{
					return true;
				}
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataSlotFieldの個数
		/// </summary>
#else
		/// <summary>
		/// Number of DataSlotField
		/// </summary>
#endif
		[System.Obsolete("use dataSlotCount")]
		public int dataSlotFieldCount
		{
			get
			{
				return dataSlotCount;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataSlotFieldの個数
		/// </summary>
#else
		/// <summary>
		/// Number of DataSlotField
		/// </summary>
#endif
		[System.Obsolete("use dataSlotCount")]
		public int calculatorSlotFieldCount
		{
			get
			{
				return dataSlotCount;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataSlotFieldを取得する。
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <returns>DataSlotField</returns>
#else
		/// <summary>
		/// Get DataSlotField.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>DataSlotField</returns>
#endif
		[System.Obsolete("use GetDataSlot(index).slotField")]
		public DataSlotField GetDataSlotField(int index)
		{
			DataSlot slot = GetDataSlot(index);
			if (slot == null || slot.slotField == null)
			{
				Debug.LogError(this.GetType() + " : GetDataSlotField(" + index + ") == null");
				return null;
			}

			return slot.slotField;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataSlotFieldを取得する。
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <returns>DataSlotField</returns>
#else
		/// <summary>
		/// Get DataSlotField.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>DataSlotField</returns>
#endif
		[System.Obsolete("use GetDataSlotField")]
		public DataSlotField GetCalculatorSlotField(int index)
		{
			return GetDataSlotField(index);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataSlotが存在しているか確認し必要であれば再構築する。
		/// </summary>
		/// <param name="slot">存在を確認するDataSlot</param>
#else
		/// <summary>
		/// Check if DataSlot exists and rebuild if necessary.
		/// </summary>
		/// <param name="slot">Data Slot to confirm existence</param>
#endif
		public void RebuildDataSlotFieldIfNecessary(DataSlot slot)
		{
			if (ContainsSlot(slot))
			{
				return;
			}

			Debug.LogWarning("GetDataSlotField: DataSlotField was not found.\nIf you change the number of DataSlot fields dynamically from the script, it is recommended to invoke NodeBehaviour.RebuildDataSlotFields() immediately after the change.");
			RebuildDataSlotFields();
			return;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataSlotFieldを取得する。
		/// </summary>
		/// <param name="slot">DataSlot</param>
		/// <param name="rebuild">見つからない場合に再構築する。</param>
		/// <returns>DataSlotField</returns>
#else
		/// <summary>
		/// Get DataSlotField.
		/// </summary>
		/// <param name="slot">DataSlot</param>
		/// <param name="rebuild">Rebuild if not found.</param>
		/// <returns>DataSlotField</returns>
#endif
		[System.Obsolete("use RebuildDataSlotFieldIfNecessary and slot.slotField")]
		public DataSlotField GetDataSlotField(DataSlot slot, bool rebuild = false)
		{
			if (ContainsSlot(slot))
			{
				return slot.slotField;
			}

			if (rebuild)
			{
				Debug.LogWarning("GetDataSlotField: DataSlotField was not found.\nIf you change the number of DataSlot fields dynamically from the script, it is recommended to invoke NodeBehaviour.RebuildDataSlotFields() immediately after the change.");
				RebuildDataSlotFields();
				return GetDataSlotField(slot, false);
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataSlotFieldを取得する。
		/// </summary>
		/// <param name="slot">DataSlot</param>
		/// <returns>DataSlotField</returns>
#else
		/// <summary>
		/// Get DataSlotField.
		/// </summary>
		/// <param name="slot">DataSlot</param>
		/// <returns>DataSlotField</returns>
#endif
		[System.Obsolete("use GetDataSlotField(DataSlot)")]
		public DataSlotField GetCalculatorSlotField(DataSlot slot)
		{
			return GetDataSlotField(slot);
		}

		private sealed class FieldCache
		{
			public DynamicReflection.DynamicField field;
			public DataLinkAttribute attribute;
			public FormerlySerializedAsAttribute[] formerlySerializedAsList;
		}

		private sealed class TypeCache
		{
			private static Dictionary<System.Type, TypeCache> s_Types = new Dictionary<System.Type, TypeCache>();

			public static TypeCache GetTypeCache(System.Type type)
			{
				TypeCache typeCache = null;

				if (!s_Types.TryGetValue(type, out typeCache))
				{
					typeCache = new TypeCache(type);
					s_Types.Add(type, typeCache);
				}

				return typeCache;
			}

			public static bool IsValidDataSlotField(System.Type fieldType)
			{
				return Serialization.SerializationUtility.IsSerializableFieldType(fieldType) &&
						!Serialization.SerializationUtility.IsSupportedArray(fieldType);
			}

			public List<FieldCache> dataLinkFields
			{
				get;
				private set;
			}

			public TypeCache baseType
			{
				get;
				private set;
			}

			public TypeCache(System.Type type)
			{
				dataLinkFields = new List<FieldCache>();

				var fields = Serialization.FieldCache.GetFields(type);
				for (int i = 0, count = fields.Length; i < count; i++)
				{
					var fieldInfo = fields[i];
					System.Type fieldType = fieldInfo.FieldType;
					if (IsValidDataSlotField(fieldType))
					{
						DataLinkAttribute attribute = AttributeHelper.GetAttribute<DataLinkAttribute>(fieldInfo);
						if (attribute != null)
						{
							FieldCache fieldCache = new FieldCache();
							fieldCache.field = DynamicReflection.DynamicField.GetField(fieldInfo);
							fieldCache.attribute = attribute;
							fieldCache.formerlySerializedAsList = AttributeHelper.GetAttributes<FormerlySerializedAsAttribute>(fieldInfo);
							dataLinkFields.Add(fieldCache);
						}
					}
				}

				System.Type baseType = TypeUtility.GetBaseType(type);
				if (baseType != null && !EachFieldUtility.IsIgnoreDeclaringType(baseType))
				{
					this.baseType = GetTypeCache(baseType);
				}
			}
		}

		private static Dictionary<string, DataLinkFieldInfo> s_OldDataLinks = null;

		void RebuildDataSlotFieldLinks()
		{
			System.Type type = GetType();

			if (s_OldDataLinks == null)
			{
				s_OldDataLinks = new Dictionary<string, DataLinkFieldInfo>();
			}
			else
			{
				s_OldDataLinks.Clear();
			}

			for (int linkIndex = 0; linkIndex < _DataSlotFieldLinks.Count; linkIndex++)
			{
				DataLinkFieldInfo link = _DataSlotFieldLinks[linkIndex];
				s_OldDataLinks.Add(link.fieldName, link);
			}

			_DataSlotFieldLinks.Clear();

			for (TypeCache typeCache = TypeCache.GetTypeCache(type); typeCache != null; typeCache = typeCache.baseType)
			{
				var dataLinkFields = typeCache.dataLinkFields;
				for (int fieldIndex = 0; fieldIndex < dataLinkFields.Count; fieldIndex++)
				{
					FieldCache fieldCache = dataLinkFields[fieldIndex];
					DynamicReflection.DynamicField field = fieldCache.field;

					System.Reflection.FieldInfo fieldInfo = field.fieldInfo;
					string fieldName = fieldInfo.Name;

					System.Type fieldType = fieldInfo.FieldType;

					DataLinkFieldInfo link = null;
					if (s_OldDataLinks.TryGetValue(fieldName, out link))
					{
						s_OldDataLinks.Remove(fieldName);
					}
					else
					{
						var formerlySerializedAsList = fieldCache.formerlySerializedAsList;
						for (int index = 0; index < formerlySerializedAsList.Length; index++)
						{
							FormerlySerializedAsAttribute formerlySerializedAs = formerlySerializedAsList[index];
							string oldFieldName = formerlySerializedAs.oldName;
							if (s_OldDataLinks.TryGetValue(formerlySerializedAs.oldName, out link) && link.slot.dataType == fieldType)
							{
								s_OldDataLinks.Remove(oldFieldName);
								link.fieldName = fieldName;
								break;
							}

							link = null;
						}
					}

					if (link == null)
					{
						link = new DataLinkFieldInfo();
						link.fieldName = fieldName;
					}

					System.Type slotType = link.slot.dataType;

					if (slotType != fieldType)
					{
						if (link.slot.branchID != 0 && (slotType == null || !TypeUtility.IsAssignableFrom(slotType, fieldType)))
						{
							Debug.LogWarningFormat("{0}#{1} is disconnected because its type has been changed : {2} -> {3}", TypeUtility.GetTypeName(type), fieldName, TypeUtility.GetTypeName(slotType), TypeUtility.GetTypeName(fieldType));
							link.slot.Disconnect();
						}
						link.slot.SetType(fieldType);
					}

					link.field = field;
					link.attribute = fieldCache.attribute;

					_DataSlotFieldLinks.Add(link);
				}
			}

			foreach (DataLinkFieldInfo link in s_OldDataLinks.Values)
			{
				link.slot.Disconnect();
			}
		}

#if !NETFX_CORE
		[System.Reflection.Obfuscation(Exclude = true)]
#endif
		[System.NonSerialized]
		private bool _Internal_IsClipboard = false;

		private class DataSlotFieldsBuilder : EachField<IDataSlot>.IFindReceiver
		{
			public NodeBehaviour nodeBehaviour
			{
				get;
				private set;
			}

			public List<DataSlot> dataSlots;

			public DataSlotFieldsBuilder(NodeBehaviour nodeBehaviour)
			{
				this.nodeBehaviour = nodeBehaviour;
			}

			public void Build()
			{
				if (dataSlots == null)
				{
					dataSlots = new List<DataSlot>();
				}
				
				EachField<IDataSlot>.Find(nodeBehaviour, nodeBehaviour.GetType(), this);

				nodeBehaviour._DataSlots = dataSlots.ToArray();

				dataSlots.Clear();
			}

			public void OnFind(IDataSlot s, System.Reflection.FieldInfo f)
			{
				DataSlot slot = s as DataSlot;
				if (slot == null)
				{
					return;
				}

				slot.SetupField(f);

				dataSlots.Add(slot);
			}
		}

		private DataSlotFieldsBuilder _DataSlotFieldsBuilder = null;

#if ARBOR_DOC_JA
		/// <summary>
		/// DataSlotFieldを再構築する。
		/// </summary>
		/// <remarks>ランタイム中にDataSlotのフィールドの数を変更した場合に呼ぶ必要があります。</remarks>
#else
		/// <summary>
		/// Rebuild the DataSlotField.
		/// </summary>
		/// <remarks>It is necessary to call it when changing the number of fields of DataSlot at runtime.</remarks>
#endif
		public void RebuildDataSlotFields()
		{
			RebuildDataSlotFieldLinks();

			if (_DataSlotFieldsBuilder == null || _DataSlotFieldsBuilder.nodeBehaviour != this)
			{
				_DataSlotFieldsBuilder = new DataSlotFieldsBuilder(this);
			}

			_DataSlotFieldsBuilder.Build();

			if (!_Internal_IsClipboard)
			{
				if (_NodeGraph != null)
				{
					if (_NodeGraph.isDeserialized)
					{
						SetupDataBranchSlotField();
					}
					else
					{
						_NodeGraph.onAfterDeserialize += SetupDataBranchSlotField;
					}
				}
			}
		}

		void SetupDataBranchSlotField()
		{
			// NOTE: When adding a list of slots, the same branch is referenced because the element is copied.
			// Clear the reference from the branch once and give priority to the connection found first.
			for (int i = 0, count = _DataSlots.Length; i < count; i++)
			{
				DataSlot slot = _DataSlots[i];
				if (slot == null)
				{
					continue;
				}

				slot.ClearDataBranchSlot();
			}

			for (int i = 0, count = _DataSlots.Length; i < count; i++)
			{
				DataSlot slot = _DataSlots[i];
				if (slot == null)
				{
					continue;
				}

				slot.SetupDataBranchSlot();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataSlotFieldを再構築する。
		/// </summary>
		/// <remarks>ランタイム中にDataSlotのフィールドの数を変更した場合に呼ぶ必要があります。</remarks>
#else
		/// <summary>
		/// Rebuild the DataSlotField.
		/// </summary>
		/// <remarks>It is necessary to call it when changing the number of fields of DataSlot at runtime.</remarks>
#endif
		[System.Obsolete("use RebuildDataSlotFields")]
		public void RebuildCalculatorSlotFields()
		{
			RebuildDataSlotFields();
		}

		private class AssignFieldsReceiver : EachField<IAssignFieldReceiver>.IFindReceiver
		{
			public NodeBehaviour nodeBehaviour
			{
				get;
				private set;
			}

			public AssignFieldsReceiver(NodeBehaviour nodeBehaviour)
			{
				this.nodeBehaviour = nodeBehaviour;
			}

			public void OnFind(IAssignFieldReceiver field, System.Reflection.FieldInfo fi)
			{
				field.OnAssignField(nodeBehaviour, fi);

				EachField<IAssignFieldReceiver>.Find(field, field.GetType(), this, true);
			}
		}

		private AssignFieldsReceiver _AssignFieldsReceiver = null;
		
		void AssignFields()
		{
			if (_AssignFieldsReceiver == null || _AssignFieldsReceiver.nodeBehaviour != this)
			{
				_AssignFieldsReceiver = new AssignFieldsReceiver(this);
			}
			EachField<IAssignFieldReceiver>.Find(this, this.GetType(), _AssignFieldsReceiver);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor用。
		/// </summary>
		/// <param name="nodeGraph">NodeGraph</param>
		/// <param name="nodeID">ノードID</param>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
		/// <param name="nodeGraph">NodeGraph</param>
		/// <param name="nodeID">Node ID</param>
#endif
		public void Initialize(NodeGraph nodeGraph, int nodeID)
		{
			_NodeGraph = nodeGraph;
			_NodeID = nodeID;
		}

		internal static void SetHideFlags(Object behaviourObj)
		{
#if !ARBOR_DEBUG
			behaviourObj.hideFlags |= HideFlags.HideInInspector;
			behaviourObj.hideFlags &= ~(HideFlags.HideInHierarchy);
#endif
		}

		internal static void RefreshBehaviour(Object behaviourObj, bool isPlaying)
		{
			if (!ComponentUtility.IsValidObject(behaviourObj))
			{
				return;
			}

			SetHideFlags(behaviourObj);

			NodeBehaviour behaviour = behaviourObj as NodeBehaviour;
			if (behaviour != null)
			{
				if (!isPlaying && behaviour.enabled)
				{
					behaviour.enabled = false;
				}
			}

			INodeGraphContainer graphContainer = behaviourObj as INodeGraphContainer;
			if (graphContainer != null)
			{
				int graphCount = graphContainer.GetNodeGraphCount();
				for (int graphIndex = 0; graphIndex < graphCount; graphIndex++)
				{
					NodeGraph nodeGraph = graphContainer.GetNodeGraph<NodeGraph>(graphIndex);
					if (nodeGraph != null && !nodeGraph.external)
					{
#if !ARBOR_DEBUG
						nodeGraph.hideFlags |= HideFlags.HideInHierarchy | HideFlags.HideInInspector;
#endif

						if (!isPlaying)
						{
							nodeGraph.enabled = false;
						}
					}
				}
			}
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// 生成時に呼ばれるメソッド.
		/// </summary>
#else
		/// <summary>
		/// Raises the created event.
		/// </summary>
#endif
		protected virtual void OnCreated()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// enabledの初期化を行うために呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called to perform enabled initialization.
		/// </summary>
#endif
		protected virtual void OnInitializeEnabled()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 破棄前に呼ばれるメソッド。
		/// </summary>
#else
		/// <summary>
		/// Raises the pre destroy event.
		/// </summary>
#endif
		protected virtual void OnPreDestroy()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数はグラフが一時停止したときに呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// This function is called when the graph is paused.
		/// </summary>
#endif
		protected virtual void OnGraphPause()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数はグラフが再開したときに呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// This function is called when the graph resumes.
		/// </summary>
#endif
		protected virtual void OnGraphResume()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数はグラフが停止したときに呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// This function is called when the graph stops.
		/// </summary>
#endif
		protected virtual void OnGraphStop()
		{
		}

		internal void CallPauseEvent()
		{
			UpdateDataLink(DataLinkUpdateTiming.Execute);

			try
			{
#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
				using (new ProfilerScope(GetProfilerName("OnGraphPause()")))
#endif
				{
					OnGraphPause();
				}
			}
			catch (System.Exception ex)
			{
				Debug.LogException(ex, this);
			}
		}

		internal void CallResumeEvent()
		{
			UpdateDataLink(DataLinkUpdateTiming.Execute);

			try
			{
#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
				using (new ProfilerScope(GetProfilerName("OnGraphResume()")))
#endif
				{
					OnGraphResume();
				}
			}
			catch (System.Exception ex)
			{
				Debug.LogException(ex, this);
			}
		}

		internal void CallStopEvent()
		{
			UpdateDataLink(DataLinkUpdateTiming.Execute);

			try
			{
#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
				using (new ProfilerScope(GetProfilerName("OnGraphStop()")))
#endif
				{
					OnGraphStop();
				}
			}
			catch (System.Exception ex)
			{
				Debug.LogException(ex, this);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 手動によるDataLinkの値更新。
		/// <see cref="DataLinkUpdateTiming.Manual"/>のDataLinkフィールドの値を更新する。
		/// </summary>
#else
		/// <summary>
		/// Manually update DataLink values.
		/// Update the value of DataLink field of <see cref="DataLinkUpdateTiming.Manual"/>.
		/// </summary>
#endif
		public void UpdateDataLink()
		{
			UpdateDataLink(DataLinkUpdateTiming.Manual);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataLinkの値更新。
		/// </summary>
		/// <param name="updateTiming">更新の呼び出しタイミング</param>
#else
		/// <summary>
		/// DataLink value update.
		/// </summary>
		/// <param name="updateTiming">Update call timing</param>
#endif
		protected void UpdateDataLink(DataLinkUpdateTiming updateTiming)
		{
			int count = _DataSlotFieldLinks.Count;
			for (int i = 0; i < count; i++)
			{
				DataLinkFieldInfo link = _DataSlotFieldLinks[i];
				if ((link.currentUpdateTiming & updateTiming) != 0)
				{
					object value = null;
					if (link.slot.GetValue(ref value))
					{
						link.field.SetValue(this, value);
					}
				}
			}
		}

#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
		private Dictionary<string, string> _ProfilerNames = new Dictionary<string, string>();
		internal string GetProfilerName(string name)
		{
			string message = null;
			if (!_ProfilerNames.TryGetValue(name, out message))
			{
				message = TypeUtility.GetTypeName(GetType()) + " : " + name;
				_ProfilerNames.Add(name, message);
			}
			return message;
		}
#endif
	}
}