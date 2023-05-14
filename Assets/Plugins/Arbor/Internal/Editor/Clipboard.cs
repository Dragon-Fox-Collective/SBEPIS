//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

using Arbor;
using Arbor.DynamicReflection;

namespace ArborEditor
{
	public sealed class Clipboard : Arbor.ScriptableSingleton<Clipboard>
	{
		private static readonly DynamicField s_NodeGraph_IsEditorField;
		private static readonly DynamicField s_ParameterContainerInternal_IsEditorField;
		private static readonly DynamicField s_Node_NodeGraphField;
		private static readonly DynamicField s_NodeBehaviour_IsClipboardField;

		static Clipboard()
		{
			s_NodeGraph_IsEditorField = DynamicField.GetField(typeof(NodeGraph).GetField("_IsEditor", BindingFlags.NonPublic | BindingFlags.Instance));
			s_ParameterContainerInternal_IsEditorField = DynamicField.GetField(typeof(ParameterContainerInternal).GetField("_IsEditor", BindingFlags.NonPublic | BindingFlags.Instance));

			System.Type nodeType = typeof(Node);
			s_Node_NodeGraphField = DynamicField.GetField(nodeType.GetField("_NodeGraph", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));

			System.Type nodeBehaviourType = typeof(NodeBehaviour);
			s_NodeBehaviour_IsClipboardField = DynamicField.GetField(nodeBehaviourType.GetField("_Internal_IsClipboard", BindingFlags.Instance | BindingFlags.NonPublic));
		}

		public static void CopyNodeGraph(NodeGraph source)
		{
			instance.CopyNodeGraphInternal(source);
		}

		public static void PasteNodeGraphValues(NodeGraph nodeGraph)
		{
			instance.PasteNodeGraphValuesInternal(nodeGraph);
		}

		public static void PasteNodeGraphAsNew(GameObject gameObject)
		{
			instance.PasteNodeGraphAsNewInternal(gameObject);
		}

		public static void CopyParameterContainer(ParameterContainerInternal source)
		{
			instance.CopyParameterContainerInternal(source);
		}

		public static void CopyBehaviour(NodeBehaviour source)
		{
			instance.CopyBehaviourInternal(source);
		}

		public static void PasteBehaviourValues(NodeBehaviour behaviour)
		{
			instance.PasteBehaviourValuesInternal(behaviour);
		}

		public static bool CompareBehaviourType(System.Type behaviourType, bool inherit)
		{
			return instance.CompareBehaviourTypeInternal(behaviourType, inherit);
		}

		public static bool hasCopyedNodes
		{
			get
			{
				return instance.hasCopyedNodesInternal;
			}
		}

		public static void CopyNodes(Node[] nodes)
		{
			instance.CopyNodesInternal(nodes);
		}

		public static Node[] GetClippedNodes()
		{
			return instance.GetClippedNodesInternal();
		}

		[SerializeField]
		private GameObject _GameObject = null;

		[SerializeField]
		private NodeBehaviour _CopyBehaviour = null;

		[SerializeField]
		private NodeGraph _CopyBehaviourSourceGraph = null;

		[SerializeField]
		private NodeGraph _NodeClipboard = null;

		[SerializeField]
		private List<int> _CopyNodes = new List<int>();

		[SerializeField]
		private NodeGraph _SourceNodeGraph = null;

		[SerializeField]
		private Object _CopyComponent = null;

		private static GameObject gameObject
		{
			get
			{
				if (instance._GameObject == null)
				{
					instance._GameObject = EditorUtility.CreateGameObjectWithHideFlags("Clipboard", HideFlags.HideAndDontSave);
					instance._GameObject.tag = "EditorOnly";
				}

				instance._GameObject.SetActive(false);

				return instance._GameObject;
			}
		}

		public static NodeBehaviour copyBehaviour
		{
			get
			{
				return instance._CopyBehaviour;
			}
		}

		private void ClearInternal()
		{
			bool saveEnabled = ComponentUtility.useEditorProcessor;
			ComponentUtility.useEditorProcessor = false;

			if (_CopyBehaviour != null)
			{
				DestroyImmediate(_CopyBehaviour);
				_CopyBehaviour = null;
				_CopyBehaviourSourceGraph = null;
			}

			if (_NodeClipboard != null)
			{
				NodeGraph.Destroy(_NodeClipboard);
				_NodeClipboard = null;
			}

			_CopyNodes.Clear();

			_SourceNodeGraph = null;

			ComponentUtility.useEditorProcessor = saveEnabled;
		}

		public static bool IsSameNodeGraph(NodeGraph sourceGraph, NodeGraph destGraph)
		{
			return instance.IsSameNodeGraphInternal(sourceGraph, destGraph);
		}

		public static bool IsSameNodeGraph(NodeBehaviour sourceBehaviour, NodeBehaviour destBehaviour)
		{
			return instance.IsSameNodeGraphInternal(sourceBehaviour, destBehaviour);
		}

		public static bool GetEditorNodeGraph(NodeGraph nodeGraph)
		{
			return (bool)s_NodeGraph_IsEditorField.GetValue(nodeGraph);
		}

		public static void SetEditorParameterContainer(ParameterContainerInternal parameterContainer, bool isEditor)
		{
			s_ParameterContainerInternal_IsEditorField.SetValue(parameterContainer, isEditor);
		}

		public static bool GetEditorParameterContainer(ParameterContainerInternal parameterContainer)
		{
			return (bool)s_ParameterContainerInternal_IsEditorField.GetValue(parameterContainer);
		}

		public static void SetEditorNodeGraph(NodeGraph nodeGraph, bool isEditor)
		{
			s_NodeGraph_IsEditorField.SetValue(nodeGraph, isEditor);
		}

		static void SetNodeGraph(Node node, NodeGraph graph)
		{
			s_Node_NodeGraphField.SetValue(node, graph);
		}

		internal static void SetNodeBehaviourIsClipboard(NodeBehaviour behaviour, bool isClipboard)
		{
			s_NodeBehaviour_IsClipboardField.SetValue(behaviour, isClipboard);
		}

		public static void CopyNodeBehaviour(NodeBehaviour source, NodeBehaviour dest, bool checkLink)
		{
			var processor = GetProcessor(source.GetType());
			if (processor != null)
			{
				processor.CopyNodeBehaviour(source, dest, checkLink);
				return;
			}

			DoCopyNodeBehaviour(source, dest, checkLink);
		}

		static void CopyNodeGraphBehaviours(NodeGraph dest)
		{
			for (int count = dest.nodeCount, i = 0; i < count; i++)
			{
				Node node = dest.GetNodeFromIndex(i);

				if (node.nodeGraph != dest)
				{
					SetNodeGraph(node, dest);
				}

				INodeBehaviourContainer behaviours = node as INodeBehaviourContainer;
				if (behaviours != null)
				{
					int behaviourCount = behaviours.GetNodeBehaviourCount();
					for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
					{
						NodeBehaviour behaviour = behaviours.GetNodeBehaviour<NodeBehaviour>(behaviourIndex);
						if (behaviour != null)
						{
							NodeBehaviour copyBehaviour = NodeBehaviour.CreateNodeBehaviour(node, behaviour.GetType(), true);

							CopyNodeBehaviour(behaviour, copyBehaviour, false);

							behaviours.SetNodeBehaviour(behaviourIndex, copyBehaviour);
						}
					}
				}
			}
		}

		private static bool _IsCopyNodeGraph = false;

		static void CopyNodeGraph(NodeGraph source, NodeGraph dest)
		{
			using (new Presets.DisableApplyDefaultPresetScope(true))
			{
				bool tmpIsCopyNodeGraph = _IsCopyNodeGraph;
				_IsCopyNodeGraph = true;

				try
				{
					Object destOwnerBehaviour = dest.ownerBehaviourObject;

					dest.DestroySubComponents(false);

					bool isEditor = Clipboard.GetEditorNodeGraph(dest);
					Clipboard.SetEditorNodeGraph(dest, true);

					ParameterContainerInternal destParameterContainer = null;
					ParameterContainerInternal sourceParameterContainer = source.parameterContainer;
					if (sourceParameterContainer != null)
					{
						destParameterContainer = ParameterContainerInternal.Create(dest.gameObject, sourceParameterContainer.GetType(), dest);
						SetEditorParameterContainer(destParameterContainer, isEditor);

						CopyParameterContainer(sourceParameterContainer, destParameterContainer);
					}

					bool cachedEnabled = ComponentUtility.useEditorProcessor;
					ComponentUtility.useEditorProcessor = true;

					Object sourceOwnerBehaviour = source.ownerBehaviourObject;
					if (!isEditor)
					{
						source.ownerBehaviourObject = destOwnerBehaviour;
					}

					EditorUtility.CopySerialized(source, dest);

					source.ownerBehaviourObject = sourceOwnerBehaviour;

					for (int i = 0; i < dest.dataBranchRerouteNodes.count; i++)
					{
						var rerouteNode = dest.dataBranchRerouteNodes[i];
						if (rerouteNode == null)
						{
							continue;
						}

						RerouteSlot slot = rerouteNode.link;
						if (slot == null)
						{
							continue;
						}

						slot.nodeGraph = dest;
						slot.DirtyBranchCache();
						
						IInputSlot inputSlot = slot as IInputSlot;
						if (inputSlot != null)
						{
							DataBranch branch = inputSlot.GetBranch();
							if (branch != null)
							{
								branch.inBehaviour = dest;
								branch.inNodeID = rerouteNode.nodeID;
							}
						}

						IOutputSlot outputSlot = slot as IOutputSlot;
						if (outputSlot != null)
						{
							for (int j = 0; j < outputSlot.branchCount; j++)
							{
								DataBranch branch = outputSlot.GetBranch(j);
								if (branch != null)
								{
									branch.outBehaviour = dest;
									branch.outNodeID = rerouteNode.nodeID;
								}
							}
						}
					}

					ComponentUtility.useEditorProcessor = cachedEnabled;

					CopyNodeGraphBehaviours(dest);

					ParameterContainerEditorUtility.SetParameterContainer(dest, destParameterContainer);
					if (sourceParameterContainer != null)
					{
						MoveParameterReference(dest, sourceParameterContainer);
					}

					dest.ownerBehaviourObject = destOwnerBehaviour;

					Clipboard.SetEditorNodeGraph(dest, isEditor);

				}
				finally
				{
					_IsCopyNodeGraph = tmpIsCopyNodeGraph;
				}
			}
		}

		public static GameObject SaveToPrefab(string path, NodeGraph nodeGraph)
		{
			GameObject prefabGameObject = null;
			GameObject gameObject = EditorUtility.CreateGameObjectWithHideFlags(nodeGraph.graphName, HideFlags.HideInHierarchy);

			try
			{
				bool cachedEnabled = ComponentUtility.useEditorProcessor;
				ComponentUtility.useEditorProcessor = false;

				NodeGraph destGraph = NodeGraph.Create(gameObject, nodeGraph.GetType());
				SetEditorNodeGraph(destGraph, true);

				CopyNodeGraph(nodeGraph, destGraph);

				SetEditorNodeGraph(destGraph, false);
				destGraph.enabled = true;

				ComponentUtility.useEditorProcessor = cachedEnabled;

				prefabGameObject = PrefabUtility.SaveAsPrefabAsset(gameObject, path);
			}
			catch (System.Exception ex)
			{
				Debug.LogException(ex);
			}
			finally
			{
				Object.DestroyImmediate(gameObject);
			}

			return prefabGameObject;
		}

		static void CopyInternalVariable(InternalVariableBase source, InternalVariableBase dest)
		{
			if (dest == null)
			{
				return;
			}

			ParameterContainerInternal container = dest.parameterContainer;

			EditorUtility.CopySerialized(source, dest);

			dest.Initialize(container);
		}

		static void CopyParameterVariables(ParameterContainerInternal dest)
		{
			for (int count = dest.parameterCount, i = 0; i < count; i++)
			{
				Parameter parameter = dest.GetParameterFromIndex(i);

				if (parameter.container != dest)
				{
					parameter.container = dest;
				}

				switch (parameter.type)
				{
					case Parameter.Type.Variable:
						{
							VariableBase variable = parameter.variableObject;
							if (variable != null)
							{
								VariableBase copyVariable = VariableBase.Create(dest, variable.GetType());

								CopyInternalVariable(variable, copyVariable);

								parameter.variableObject = copyVariable;
							}
						}
						break;
					case Parameter.Type.VariableList:
						{
							VariableListBase variable = parameter.variableListObject;
							if (variable != null)
							{
								VariableListBase copyVariable = VariableListBase.Create(dest, variable.GetType());

								CopyInternalVariable(variable, copyVariable);

								parameter.variableListObject = copyVariable;
							}
						}
						break;
				}
			}
		}

		static void CopyParameterContainer(ParameterContainerInternal source, ParameterContainerInternal dest)
		{
			bool isEditor = Clipboard.GetEditorParameterContainer(dest);

			bool cachedEnabled = ComponentUtility.useEditorProcessor;
			ComponentUtility.useEditorProcessor = true;

			Object owner = dest.owner;

			EditorUtility.CopySerialized(source, dest);

			dest.owner = owner;

			ComponentUtility.useEditorProcessor = cachedEnabled;

			CopyParameterVariables(dest);

			Clipboard.SetEditorParameterContainer(dest, isEditor);
		}

		internal static void DestroyChildGraphs(NodeBehaviour dest)
		{
			INodeGraphContainer graphContainer = dest as INodeGraphContainer;
			if (graphContainer == null)
			{
				return;
			}

			int graphCount = graphContainer.GetNodeGraphCount();
			for (int i = 0; i < graphCount; i++)
			{
				NodeGraph nodeGraph = graphContainer.GetNodeGraph<NodeGraph>(i);

				if (nodeGraph != null)
				{
					if (!nodeGraph.external)
					{
						NodeGraph.Destroy(nodeGraph);
					}
				}
			}
		}

		static void MoveDataSlots(NodeBehaviour behaviour)
		{
			NodeGraph nodeGraph = behaviour.nodeGraph;

			ComponentUtility.RecordObject(behaviour, "Copy Behaviour");

			for (int i = 0; i < behaviour.dataSlotCount; i++)
			{
				var slot = behaviour.GetDataSlot(i);
				if (slot == null)
				{
					continue;
				}

				slot.nodeGraph = nodeGraph;
				slot.DirtyBranchCache();
				
				ComponentUtility.RecordObject(nodeGraph, "Copy Behaviour");

				IInputSlot inputSlot = slot as IInputSlot;
				if (inputSlot != null)
				{
					DataBranch branch = inputSlot.GetBranch();
					if (branch != null && branch.inBehaviour != behaviour)
					{
						branch.inBehaviour = behaviour;
					}
				}

				IOutputSlot outputSlot = slot as IOutputSlot;
				if (outputSlot != null)
				{
					for (int j = 0; j < outputSlot.branchCount; j++)
					{
						DataBranch branch = outputSlot.GetBranch(j);
						if (branch != null && branch.outBehaviour != behaviour)
						{
							branch.outBehaviour = behaviour;
						}
					}
				}

				ComponentUtility.SetDirty(nodeGraph);
			}

			ComponentUtility.SetDirty(behaviour);
		}

		static void ClearDataSlots(NodeBehaviour behaviour)
		{
			for (int i = 0; i < behaviour.dataSlotCount; i++)
			{
				DataSlot slot = behaviour.GetDataSlot(i);
				slot?.ClearBranch();
			}
		}

		static void ReconnectDataSlots(NodeBehaviour behaviour)
		{
			NodeGraph nodeGraph = behaviour.nodeGraph;

			for (int i = 0; i < behaviour.dataSlotCount; i++)
			{
				DataSlot slot = behaviour.GetDataSlot(i);
				if (slot == null)
				{
					continue;
				}

				bool clear = true;

				if (slot.slotType == SlotType.Input)
				{
					slot.nodeGraph = nodeGraph;
					slot.DirtyBranchCache();

					IInputSlot inputSlot = slot as IInputSlot;
					if (inputSlot != null)
					{
						DataBranch branch = inputSlot.GetBranch();
						if (branch != null)
						{
							DataSlot outputSlot = branch.outputSlot;
							if (outputSlot != null && DataSlot.IsConnectable(slot, outputSlot))
							{
								DataBranch newBranch = nodeGraph.ConnectDataBranch(behaviour.nodeID, behaviour, slot, branch.outNodeID, branch.outBehaviour, branch.outputSlot);
								if (newBranch != null)
								{
									clear = false;
									newBranch.enabled = true;
								}
							}
						}
					}
				}

				if (clear)
				{
					slot.ClearBranch();
				}
			}
		}

		internal static void CopyChildGraphs(NodeBehaviour dest)
		{
			INodeGraphContainer graphContainer = dest as INodeGraphContainer;
			if (graphContainer == null)
			{
				return;
			}

			NodeGraph parentGraph = dest.nodeGraph;
			bool isEditor = instance._CopyBehaviour == dest || (parentGraph != null && GetEditorNodeGraph(parentGraph));

			int graphCount = graphContainer.GetNodeGraphCount();
			for (int i = 0; i < graphCount; i++)
			{
				NodeGraph nodeGraph = graphContainer.GetNodeGraph<NodeGraph>(i);

				if (!nodeGraph.external)
				{
					NodeGraph destGraph = NodeGraph.Create(dest.gameObject, nodeGraph.GetType()) as NodeGraph;
#if !ARBOR_DEBUG
					destGraph.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
#endif
					destGraph.ownerBehaviour = dest;

					SetEditorNodeGraph(destGraph, isEditor);

					CopyNodeGraph(nodeGraph, destGraph);

					SetEditorNodeGraph(destGraph, false);

					graphContainer.SetNodeGraph(i, destGraph);
				}
			}
		}

		public static void DoCopyNodeBehaviour(NodeBehaviour source, NodeBehaviour dest, bool checkLink)
		{
			if (dest == null)
			{
				return;
			}

			NodeGraph nodeGraph = dest.nodeGraph;
			int nodeID = dest.nodeID;
			bool expanded = dest.expanded;

			DestroyChildGraphs(dest);

			if (nodeGraph != null)
			{
				nodeGraph.DisconnectDataBranch(dest);
			}

			SetNodeBehaviourIsClipboard(dest, true);
			EditorUtility.CopySerialized(source, dest);
			SetNodeBehaviourIsClipboard(dest, false);

			dest.Initialize(nodeGraph, nodeID);
			dest.expanded = expanded;

			bool isSameNodeGraph = IsSameNodeGraph(source, dest);

			if (_IsCopyNodeGraph)
			{
				MoveDataSlots(dest);
			}
			else if (!_IsDuplicateNode)
			{
				if (isSameNodeGraph)
				{
					ReconnectDataSlots(dest);
				}
				else if (checkLink)
				{
					ClearDataSlots(dest);
				}
				else
				{
					MoveDataSlots(dest);
				}
			}

			CopyChildGraphs(dest);
		}

		private static Dictionary<System.Type, ClipboardProcessor> s_Processors = new Dictionary<System.Type, ClipboardProcessor>();

		private static ClipboardProcessor GetProcessor(System.Type type)
		{
			if (type == null)
			{
				return null;
			}

			ClipboardProcessor processor = null;
			if (!s_Processors.TryGetValue(type, out processor))
			{
				System.Type processorType = CustomAttributes<CustomClipboardProcessor, ClipboardProcessor>.FindEditorType(type);
				if (processorType != null)
				{
					processor = (ClipboardProcessor)System.Activator.CreateInstance(processorType);
				}
				else
				{
					processor = null;
				}

				s_Processors.Add(type, processor);
			}

			return processor;
		}

		public static void MoveBehaviour(Node node, NodeBehaviour sourceBehaviour)
		{
			var processor = GetProcessor(sourceBehaviour.GetType());
			if (processor != null)
			{
				processor.MoveBehaviour(node, sourceBehaviour);
				return;
			}
		}

		static void MoveParameterReference(NodeGraph nodeGraph, ParameterContainerInternal sourceParameterContainer)
		{
			ParameterContainerInternal destParameterContainer = nodeGraph.parameterContainer;

			int nodeCount = nodeGraph.nodeCount;
			for (int i = 0; i < nodeCount; i++)
			{
				var node = nodeGraph.GetNodeFromIndex(i);

				var behaviourContainer = node as INodeBehaviourContainer;
				if (behaviourContainer == null)
				{
					continue;
				}

				int behaviourCount = behaviourContainer.GetNodeBehaviourCount();
				for (int j = 0; j < behaviourCount; j++)
				{
					NodeBehaviour behaviour = behaviourContainer.GetNodeBehaviour<NodeBehaviour>(j);

					EachField<ParameterReference>.Find(behaviour, behaviour.GetType(), (r) =>
					{
						if (r.constantContainer == sourceParameterContainer)
						{
							r.constantContainer = destParameterContainer;
						}
					});
				}
			}
		}

		public static void MoveParameterContainer(NodeGraph nodeGraph)
		{
			bool cachedEnabled = ComponentUtility.useEditorProcessor;
			ComponentUtility.useEditorProcessor = false;

			ParameterContainerInternal sourceParameterContainer = nodeGraph.parameterContainer;
			if (sourceParameterContainer != null)
			{
				if (sourceParameterContainer.gameObject == nodeGraph.gameObject && sourceParameterContainer.owner == null)
				{
					sourceParameterContainer.owner = nodeGraph;
				}
				else
				{
					ParameterContainerInternal destParameterContainer = ParameterContainerInternal.Create(nodeGraph.gameObject, sourceParameterContainer.GetType(), nodeGraph);

					CopyParameterContainer(sourceParameterContainer, destParameterContainer);

					ParameterContainerEditorUtility.SetParameterContainer(nodeGraph, destParameterContainer);

					MoveParameterReference(nodeGraph, sourceParameterContainer);
				}

				var scene = nodeGraph.gameObject.scene;
				if (scene.IsValid())
				{
					UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
				}
				else
				{
					EditorUtility.SetDirty(nodeGraph);
				}
			}

			ComponentUtility.useEditorProcessor = cachedEnabled;
		}

		public static void MoveVariable(Parameter parameter, VariableBase sourceVariable)
		{
			bool cachedEnabled = ComponentUtility.useEditorProcessor;
			ComponentUtility.useEditorProcessor = false;

			VariableBase destVariable = VariableBase.Create(parameter.container, sourceVariable.GetType()) as VariableBase;
			parameter.variableObject = destVariable;

			if (destVariable != null)
			{
				CopyInternalVariable(sourceVariable, destVariable);
			}

			ComponentUtility.useEditorProcessor = cachedEnabled;
		}

		public static void MoveVariableList(Parameter parameter, VariableListBase sourceVariableList)
		{
			bool cachedEnabled = ComponentUtility.useEditorProcessor;
			ComponentUtility.useEditorProcessor = false;

			VariableListBase destVariableList = VariableListBase.Create(parameter.container, sourceVariableList.GetType()) as VariableListBase;
			parameter.variableListObject = destVariableList;

			if (destVariableList != null)
			{
				CopyInternalVariable(sourceVariableList, destVariableList);
			}

			ComponentUtility.useEditorProcessor = cachedEnabled;
		}

		public static System.Type copiedComponentType
		{
			get
			{
				Object component = instance._CopyComponent;
				if (component == null)
				{
					return null;
				}

				return component.GetType();
			}
		}

		void DestroyCopyComponent()
		{
			if (_CopyComponent == null)
			{
				return;
			}

			bool cachedEnabled = ComponentUtility.useEditorProcessor;
			ComponentUtility.useEditorProcessor = false;

			if (_CopyComponent is NodeGraph)
			{
				NodeGraph.Destroy(_CopyComponent as NodeGraph);
			}
			else if (_CopyComponent is NodeBehaviour)
			{
				NodeBehaviour.Destroy(_CopyComponent as NodeBehaviour);
			}
			else
			{
				Object.DestroyImmediate(_CopyComponent);
			}

			ComponentUtility.useEditorProcessor = cachedEnabled;

			_CopyComponent = null;
		}

		private void CopyNodeGraphInternal(NodeGraph source)
		{
			DestroyCopyComponent();

			bool cachedEnabled = ComponentUtility.useEditorProcessor;
			ComponentUtility.useEditorProcessor = false;

			NodeGraph dest = NodeGraph.Create(gameObject, source.GetType());
			SetEditorNodeGraph(dest, true);

			CopyNodeGraph(source, dest);

			ComponentUtility.useEditorProcessor = cachedEnabled;

			_CopyComponent = dest;
		}

		private void PasteNodeGraphValuesInternal(NodeGraph nodeGraph)
		{
			NodeGraph source = _CopyComponent as NodeGraph;
			if (source == null || nodeGraph == null ||
				source.GetType() != nodeGraph.GetType())
			{
				return;
			}

			Undo.IncrementCurrentGroup();

			int currentGroup = Undo.GetCurrentGroup();

			Undo.RegisterCompleteObjectUndo(nodeGraph, "Paste " + nodeGraph.GetType() + " Values");

			CopyNodeGraph(source, nodeGraph);

			Undo.CollapseUndoOperations(currentGroup);

			EditorUtility.SetDirty(nodeGraph);
		}

		private void PasteNodeGraphAsNewInternal(GameObject gameObject)
		{
			NodeGraph source = _CopyComponent as NodeGraph;
			if (source == null)
			{
				return;
			}

			Undo.IncrementCurrentGroup();

			int currentGroup = Undo.GetCurrentGroup();

			NodeGraph destGraph = NodeGraph.Create(gameObject, source.GetType());

			Undo.RecordObject(destGraph, "Paste");

			CopyNodeGraph(source, destGraph);

			Undo.CollapseUndoOperations(currentGroup);

			EditorUtility.SetDirty(destGraph);
		}

		private void CopyParameterContainerInternal(ParameterContainerInternal source)
		{
			DestroyCopyComponent();

			bool cachedEnabled = ComponentUtility.useEditorProcessor;
			ComponentUtility.useEditorProcessor = false;

			ParameterContainerInternal dest = ParameterContainerInternal.Create(gameObject, source.GetType());

			SetEditorParameterContainer(dest, true);

			CopyParameterContainer(source, dest);

			ComponentUtility.useEditorProcessor = cachedEnabled;

			UnityEditorInternal.ComponentUtility.CopyComponent(dest);

			_CopyComponent = dest;
		}

		private void CopyBehaviourInternal(NodeBehaviour source)
		{
			ClearInternal();

			bool cachedEnabled = ComponentUtility.useEditorProcessor;
			ComponentUtility.useEditorProcessor = false;

			_CopyBehaviour = gameObject.AddComponent(source.GetType()) as NodeBehaviour;
			_CopyBehaviourSourceGraph = source.nodeGraph;

			CopyNodeBehaviour(source, _CopyBehaviour, false);

			ComponentUtility.useEditorProcessor = cachedEnabled;
		}

		private void PasteBehaviourValuesInternal(NodeBehaviour behaviour)
		{
			CopyNodeBehaviour(_CopyBehaviour, behaviour, true);
		}

		private bool CompareBehaviourTypeInternal(System.Type behaviourType, bool inherit)
		{
			if (_CopyBehaviour == null)
			{
				return false;
			}

			System.Type copyType = _CopyBehaviour.GetType();

			return copyType == behaviourType || (inherit && behaviourType.IsAssignableFrom(copyType));
		}

		private bool hasCopyedNodesInternal
		{
			get
			{
				return _CopyNodes.Count != 0;
			}
		}

		private class ReconnectData
		{
			public DataBranch targetBranch = null;

			public int destInNodeID = 0;
			public NodeBehaviour destInBehaviour = null;
			public DataSlot destInputSlot = null;

			public int destOutNodeID = 0;
			public NodeBehaviour destOutBehaviour = null;
			public DataSlot destOutputSlot = null;
		}

		static void ReconnectDataBranch(NodeGraph nodeGraph, List<NodeDuplicator> duplicators, bool clip)
		{
			// Listup branchies
			HashSet<DataBranch> targetBranchies = new HashSet<DataBranch>();
			for (int duplicatorIndex = 0; duplicatorIndex < duplicators.Count; duplicatorIndex++)
			{
				NodeDuplicator duplicator = duplicators[duplicatorIndex];
				Node sourceNode = duplicator.sourceNode;

				INodeBehaviourContainer behaviours = sourceNode as INodeBehaviourContainer;
				if (behaviours != null)
				{
					int behaviourCount = behaviours.GetNodeBehaviourCount();
					for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
					{
						NodeBehaviour behaviour = behaviours.GetNodeBehaviour<NodeBehaviour>(behaviourIndex);

						if (behaviour == null)
						{
							continue;
						}

						int slotCount = behaviour.dataSlotCount;
						for (int slotIndex = 0; slotIndex < slotCount; slotIndex++)
						{
							DataSlot slot = behaviour.GetDataSlot(slotIndex);
							if (slot == null)
							{
								continue;
							}

							IInputSlot inputSlot = slot as IInputSlot;
							if (inputSlot != null)
							{
								DataBranch branch = inputSlot.GetBranch();
								if (branch != null)
								{
									targetBranchies.Add(branch);
								}
							}
							else
							{
								IOutputSlot outputSlot = slot as IOutputSlot;
								if (outputSlot != null)
								{
									int branchCount = outputSlot.branchCount;
									for (int branchIndex = 0; branchIndex < branchCount; branchIndex++)
									{
										DataBranch branch = outputSlot.GetBranch(branchIndex);
										if (branch != null)
										{
											targetBranchies.Add(branch);
										}
									}
								}
							}
						}
					}
				}
				else
				{
					DataBranchRerouteNode rerouteNode = sourceNode as DataBranchRerouteNode;
					if (rerouteNode != null)
					{
						DataBranch inputBranch = rerouteNode.link.inputSlot.GetBranch();
						if (inputBranch != null)
						{
							targetBranchies.Add(inputBranch);
						}

						int branchCount = rerouteNode.link.outputSlot.branchCount;
						for (int i = 0; i < branchCount; i++)
						{
							DataBranch branch = rerouteNode.link.outputSlot.GetBranch(i);
							if (branch != null)
							{
								targetBranchies.Add(branch);
							}
						}
					}
				}
			}

			// listup reconnects
			List<ReconnectData> reconnects = new List<ReconnectData>();
			foreach (DataBranch targetBranch in targetBranchies)
			{
				NodeBehaviour inBehaviour = targetBranch.inBehaviour as NodeBehaviour;
				NodeBehaviour outBehaviour = targetBranch.outBehaviour as NodeBehaviour;

				int destInNodeID = 0;
				NodeBehaviour destInBehaviour = null;
				DataSlot destInputSlot = null;

				int destOutNodeID = 0;
				NodeBehaviour destOutBehaviour = null;
				DataSlot destOutputSlot = null;

				for (int i = 0; i < duplicators.Count; i++)
				{
					NodeDuplicator duplicator = duplicators[i];
					if (destInNodeID == 0 && destInBehaviour == null)
					{
						if (inBehaviour != null)
						{
							destInBehaviour = duplicator.GetDestBehaviour(inBehaviour);
							if (destInBehaviour != null)
							{
								destInNodeID = destInBehaviour.nodeID;
								int slotCount = destInBehaviour.dataSlotCount;
								for (int slotIndex = 0; slotIndex < slotCount; slotIndex++)
								{
									InputSlotBase slot = destInBehaviour.GetDataSlot(slotIndex) as InputSlotBase;
									if (slot != null && slot.IsConnected(targetBranch))
									{
										destInputSlot = slot;
										break;
									}
								}
							}
						}
						else
						{
							destInBehaviour = null;
							DataBranchRerouteNode rerouteNode = duplicator.destNode as DataBranchRerouteNode;
							if (rerouteNode != null && rerouteNode.link.inputSlot.IsConnected(targetBranch))
							{
								destInNodeID = rerouteNode.nodeID;
								destInputSlot = rerouteNode.link;
							}
						}
					}

					if (destOutNodeID == 0 && destOutBehaviour == null)
					{
						if (outBehaviour != null)
						{
							destOutBehaviour = duplicator.GetDestBehaviour(outBehaviour);
							if (destOutBehaviour != null)
							{
								destOutNodeID = destOutBehaviour.nodeID;
								int slotCount = destOutBehaviour.dataSlotCount;
								for (int slotIndex = 0; slotIndex < slotCount; slotIndex++)
								{
									OutputSlotBase slot = destOutBehaviour.GetDataSlot(slotIndex) as OutputSlotBase;
									if (slot != null && slot.IsConnected(targetBranch))
									{
										destOutputSlot = slot;
										break;
									}
								}
							}
						}
						else
						{
							destOutBehaviour = null;
							DataBranchRerouteNode rerouteNode = duplicator.destNode as DataBranchRerouteNode;
							if (rerouteNode != null && rerouteNode.link.outputSlot.IsConnected(targetBranch))
							{
								destOutNodeID = rerouteNode.nodeID;
								destOutputSlot = rerouteNode.link;
							}
						}
					}

					if ((destInNodeID != 0 || destInBehaviour != null) && (destOutNodeID != 0 || destOutBehaviour != null))
					{
						break;
					}
				}

				ReconnectData reconnectData = new ReconnectData()
				{
					targetBranch = targetBranch,
					destInBehaviour = destInBehaviour,
					destInNodeID = destInNodeID,
					destInputSlot = destInputSlot,
					destOutBehaviour = destOutBehaviour,
					destOutNodeID = destOutNodeID,
					destOutputSlot = destOutputSlot,
				};

				reconnects.Add(reconnectData);
			}

			// clear branchies
			for (int i = 0; i < duplicators.Count; i++)
			{
				NodeDuplicator duplicator = duplicators[i];
				Node destNode = duplicator.destNode;

				INodeBehaviourContainer behaviours = destNode as INodeBehaviourContainer;
				if (behaviours != null)
				{
					int behaviourCount = behaviours.GetNodeBehaviourCount();
					for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
					{
						NodeBehaviour behaviour = behaviours.GetNodeBehaviour<NodeBehaviour>(behaviourIndex);

						if (behaviour == null)
						{
							continue;
						}

						int slotCount = behaviour.dataSlotCount;
						for (int slotIndex = 0; slotIndex < slotCount; slotIndex++)
						{
							DataSlot slot = behaviour.GetDataSlot(slotIndex);
							slot?.ClearBranch();
						}
					}
				}
				else
				{
					DataBranchRerouteNode rerouteNode = destNode as DataBranchRerouteNode;
					rerouteNode?.link.ClearBranch();
				}
			}

			// reconnect
			for (int i = 0; i < reconnects.Count; i++)
			{
				ReconnectData reconnectData = reconnects[i];
				DataBranch targetBranch = reconnectData.targetBranch;

				NodeBehaviour outBehaviour = targetBranch.outBehaviour as NodeBehaviour;

				int destInNodeID = reconnectData.destInNodeID;
				NodeBehaviour destInBehaviour = reconnectData.destInBehaviour;
				DataSlot destInputSlot = reconnectData.destInputSlot;

				int destOutNodeID = reconnectData.destOutNodeID;
				NodeBehaviour destOutBehaviour = reconnectData.destOutBehaviour;
				DataSlot destOutputSlot = reconnectData.destOutputSlot;

				if ((destInNodeID != 0 || destInBehaviour != null) && (destOutNodeID != 0 || destOutBehaviour != null))
				{
					Bezier2D bezier = targetBranch.lineBezier;

					DataBranch branch = null;
					if (clip)
					{
						branch = nodeGraph.ConnectDataBranch(targetBranch.branchID, destInNodeID, destInBehaviour, destInputSlot, destOutNodeID, destOutBehaviour, destOutputSlot);
					}
					else
					{
						branch = nodeGraph.ConnectDataBranch(destInNodeID, destInBehaviour, destInputSlot, destOutNodeID, destOutBehaviour, destOutputSlot);
					}

					if (branch != null)
					{
						branch.lineBezier = new Bezier2D(bezier);
						branch.enabled = true;
					}
				}
				else
				{
					if (clip)
					{
						if (destInNodeID != 0 || destInBehaviour != null)
						{
							Bezier2D bezier = targetBranch.lineBezier;

							DataBranch branch = nodeGraph.ConnectDataBranch(targetBranch.branchID, destInNodeID, destInBehaviour, destInputSlot, targetBranch.outNodeID, targetBranch.outBehaviour, targetBranch.outputSlot);

							if (branch != null)
							{
								branch.lineBezier = new Bezier2D(bezier);
								branch.enabled = true;
							}
						}
					}
					else
					{
						if (destInputSlot != null)
						{
							NodeGraph outNodeGraph = null;
							if (outBehaviour != null)
							{
								outNodeGraph = outBehaviour.nodeGraph;
							}
							else
							{
								outNodeGraph = targetBranch.outBehaviour as NodeGraph;
							}

							if (IsSameNodeGraph(outNodeGraph, nodeGraph) && targetBranch.isValidOutputSlot)
							{
								Bezier2D bezier = targetBranch.lineBezier;

								DataBranch branch = nodeGraph.ConnectDataBranch(destInNodeID, destInBehaviour, destInputSlot, targetBranch.outNodeID, targetBranch.outBehaviour, targetBranch.outputSlot);

								if (branch != null)
								{
									branch.lineBezier = new Bezier2D(bezier);
									branch.enabled = true;
								}
							}
							else
							{
								IInputSlot destInSlot = destInputSlot as IInputSlot;
								destInSlot?.RemoveBranch(targetBranch);
							}
						}
					}

					IOutputSlot destOutSlot = destOutputSlot as IOutputSlot;
					destOutSlot?.RemoveBranch(targetBranch);
				}
			}
		}

		private static bool _IsDuplicateNode = false;

		public static Node[] DuplicateNodes(Vector2 position, Node[] sourceNodes, NodeGraph nodeGraph, bool clip)
		{
			using (new Presets.DisableApplyDefaultPresetScope(true))
			{
				try
				{
					_IsDuplicateNode = true;

					List<Node> duplicateNodes = new List<Node>();

					List<NodeDuplicator> nodeDuplicators = new List<NodeDuplicator>();

					for (int i = 0; i < sourceNodes.Length; i++)
					{
						Node sourceNode = sourceNodes[i];
						NodeDuplicator duplicator = NodeDuplicator.CreateDuplicator(nodeGraph, sourceNode, clip);
						if (duplicator != null)
						{
							Node node = duplicator.Duplicate(position);
							if (node != null)
							{
								duplicateNodes.Add(node);
								nodeDuplicators.Add(duplicator);
							}
							else
							{
								Object.DestroyImmediate(duplicator);
							}
						}
					}

					for (int i = 0; i < nodeDuplicators.Count; i++)
					{
						NodeDuplicator duplicator = nodeDuplicators[i];
						duplicator.DoAfterDuplicate(nodeDuplicators);
					}

					ReconnectDataBranch(nodeGraph, nodeDuplicators, clip);

					nodeGraph.OnValidateNodes();

					for (int i = 0; i < nodeDuplicators.Count; i++)
					{
						NodeDuplicator duplicator = nodeDuplicators[i];
						Object.DestroyImmediate(duplicator);
					}

					_IsDuplicateNode = false;

					return duplicateNodes.ToArray();
				}
				finally
				{
					_IsDuplicateNode = false;
				}
			}
		}

		private void CopyNodesInternal(Node[] nodes)
		{
			ClearInternal();

			if (nodes != null && nodes.Length > 0)
			{
				_SourceNodeGraph = nodes[0].nodeGraph;
			}

			bool cachedEnabled = ComponentUtility.useEditorProcessor;
			ComponentUtility.useEditorProcessor = false;

			_NodeClipboard = NodeGraph.Create(gameObject, _SourceNodeGraph.GetType());
			SetEditorNodeGraph(_NodeClipboard, true);

			var duplicateNodes = DuplicateNodes(Vector2.zero, nodes, _NodeClipboard, true);
			for (int i = 0; i < duplicateNodes.Length; i++)
			{
				Node node = duplicateNodes[i];
				_CopyNodes.Add(node.nodeID);
			}

			ComponentUtility.useEditorProcessor = cachedEnabled;
		}

		private bool IsSameNodeGraphInternal(NodeGraph sourceGraph, NodeGraph destGraph)
		{
			if (sourceGraph == _NodeClipboard)
			{
				sourceGraph = _SourceNodeGraph;
			}

			return sourceGraph == destGraph;
		}

		private bool IsSameNodeGraphInternal(NodeBehaviour sourceBehaviour, NodeBehaviour destBehaviour)
		{
			NodeGraph sourceGraph = sourceBehaviour.nodeGraph;
			if (sourceBehaviour == _CopyBehaviour)
			{
				sourceGraph = _CopyBehaviourSourceGraph;
			}

			return IsSameNodeGraphInternal(sourceGraph, destBehaviour.nodeGraph);
		}

		private Node[] GetClippedNodesInternal()
		{
			List<Node> nodes = new List<Node>();

			if (_NodeClipboard != null)
			{
				for (int i = 0, count = _CopyNodes.Count; i < count; i++)
				{
					int nodeID = _CopyNodes[i];
					nodes.Add(_NodeClipboard.GetNodeFromID(nodeID));
				}
			}

			return nodes.ToArray();
		}
	}
}
