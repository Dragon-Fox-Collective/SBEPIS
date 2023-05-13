//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor.BehaviourTree
{
	using Arbor.Playables;

#pragma warning disable 1574
#if ARBOR_DOC_JA
	/// <summary>
	/// <see cref="Arbor.BehaviourTree.BehaviourTree" />の内部クラス。
	/// 実際にGameObjectにアタッチするには<see cref="Arbor.BehaviourTree.BehaviourTree" />を使用する。
	/// </summary>
#else
	/// <summary>
	/// Internal class of <see cref="Arbor.BehaviourTree.BehaviourTree" />.
	/// To actually attach to GameObject is to use the <see cref = "Arbor.BehaviourTree.BehaviourTree" />.
	/// </summary>
#endif
#pragma warning restore 1574
	[AddComponentMenu("")]
	public abstract class BehaviourTreeInternal : NodeGraph
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 終了時に再開するフラグ。
		/// </summary>
#else
		/// <summary>
		/// Flag to restart at finish.
		/// </summary>
#endif
		public bool restartOnFinish = true;

#if ARBOR_DOC_JA
		/// <summary>
		/// 実行に関する設定。
		/// </summary>
#else
		/// <summary>
		/// Settings related to execution.
		/// </summary>
#endif
		public ExecutionSettings executionSettings = new ExecutionSettings();

		[SerializeField]
#if !ARBOR_DEBUG
		[HideInInspector]
#endif
		private NodeBranchies _NodeBranchies = new NodeBranchies();

		[SerializeField]
#if !ARBOR_DEBUG
		[HideInInspector]
#endif
		private RootNode _RootNode = null;

		[SerializeField]
#if !ARBOR_DEBUG
		[HideInInspector]
#endif
		private CompositeNodeList _CompositeNodes = new CompositeNodeList();

		[SerializeField]
#if !ARBOR_DEBUG
		[HideInInspector]
#endif
		private ActionNodeList _ActionNodes = new ActionNodeList();

		#endregion // Serialize fields

		private List<TreeNodeBase> _ActiveNodes = new List<TreeNodeBase>();
		private TreeNodeBase _CurrentNode = null;

		private List<TreeNodeBase> _Revaluators = new List<TreeNodeBase>();

		private int _InterruptCount = 0;
		private bool _IsBreakPoint;

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBranchのリストを取得
		/// </summary>
#else
		/// <summary>
		/// Get NodeBranch List
		/// </summary>
#endif
		public NodeBranchies nodeBranchies
		{
			get
			{
				return _NodeBranchies;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// CompositeNodeのリストを取得
		/// </summary>
#else
		/// <summary>
		/// Get CompositeNode List
		/// </summary>
#endif
		public CompositeNodeList compositeNodes
		{
			get
			{
				return _CompositeNodes;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ActionNodeのリストを取得
		/// </summary>
#else
		/// <summary>
		/// Get ActionNode List
		/// </summary>
#endif
		public ActionNodeList actionNodes
		{
			get
			{
				return _ActionNodes;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RootNodeを取得
		/// </summary>
#else
		/// <summary>
		/// Get RootNode
		/// </summary>
#endif
		public RootNode rootNode
		{
			get
			{
				return _RootNode;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 現在のアクティブノード
		/// </summary>
#else
		/// <summary>
		/// Current active node
		/// </summary>
#endif
		public TreeNodeBase currentNode
		{
			get
			{
				return _CurrentNode;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プレイ開始した際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when you start playing.
		/// </summary>
#endif
		protected sealed override void OnPlay()
		{
			Push(_RootNode);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プレイ停止した際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when play is stopped.
		/// </summary>
#endif
		protected sealed override void OnStop()
		{
			while (_CurrentNode != null)
			{
				_CurrentNode.Stop();
				_ActiveNodes.Remove(_CurrentNode);

				var parentLinkSlotHolder = _CurrentNode as IParentLinkSlotHolder;

				if (parentLinkSlotHolder != null)
				{
					TreeNodeBase parentNode = null;

					NodeBranch branch = parentLinkSlotHolder.GetParentBranch();
					if (branch != null)
					{
						branch.isActive = false;
						parentNode = GetNodeFromID(branch.parentNodeID) as TreeNodeBase;
					}
					_CurrentNode = parentNode;
				}
				else
				{
					_CurrentNode = null;
				}
			}

			foreach (var revaluator in _Revaluators)
			{
				TreeBehaviourNode treeBehaviourNode = revaluator as TreeBehaviourNode;
				if (treeBehaviourNode != null)
				{
					treeBehaviourNode.RevaluationExit();
				}
			}
			_Revaluators.Clear();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ポーズした際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when you pause.
		/// </summary>
#endif
		protected sealed override void OnPause()
		{
			TreeNodeBase current = _CurrentNode;
			while (current != null)
			{
				current.Pause();
				current = current.parentNode;
			}

			int count = _Revaluators.Count;
			for (int i = 0; i < count; i++)
			{
				TreeBehaviourNode revaluator = _Revaluators[i] as TreeBehaviourNode;
				if (revaluator != null && IsRevaluationLowPriority(revaluator))
				{
					revaluator.PauseDecorators();
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再開した際に呼ばれる
		/// </summary>
#else
		/// <summary>
		/// Called when resuming
		/// </summary>
#endif
		protected sealed override void OnResume()
		{
			TreeNodeBase current = _CurrentNode;
			Stack<TreeNodeBase> stack = new Stack<TreeNodeBase>();
			while (current != null)
			{
				stack.Push(current);
				current = current.parentNode;
			}

			while (stack.Count > 0)
			{
				TreeNodeBase node = stack.Pop();
				node.Resume();
			}

			int count = _Revaluators.Count;
			for (int i = 0; i < count; i++)
			{
				TreeBehaviourNode revaluator = _Revaluators[i] as TreeBehaviourNode;
				if (revaluator != null && IsRevaluationLowPriority(revaluator))
				{
					revaluator.ResumeDecorators();
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 更新する際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when updating.
		/// </summary>
#endif
		protected sealed override void OnUpdate()
		{
			using (CalculateScope.OpenScope())
			{
				int activeNodeCount = _ActiveNodes.Count;
				for (int activeIndex = 0; activeIndex < activeNodeCount; activeIndex++)
				{
					TreeNodeBase node = _ActiveNodes[activeIndex];
					node.OnUpdate();
				}

				int actionExecutionCount = 0;
				_InterruptCount = 0;
				while (true)
				{
					if (_CurrentNode is ActionNode && _CurrentNode.isActive && _CurrentNode.status != NodeStatus.Running)
					{
						Pop(_CurrentNode.status);
					}

					if (_CurrentNode == null)
					{
						break;
					}

					_IsBreakPoint = false;

					Revaluation();

					bool execute = true;
					if (!_CurrentNode.isActive)
					{
						execute = _CurrentNode.Activate(true, false, false);

						TreeBehaviourNode treeBehaviourNode = _CurrentNode as TreeBehaviourNode;
						if (treeBehaviourNode != null && treeBehaviourNode.breakPoint)
						{
							_IsBreakPoint = true;
						}
					}

					if (execute && _IsBreakPoint)
					{
						BreakNode(_CurrentNode);
						break;
					}

					if (execute)
					{
						TreeNodeBase currentNode = _CurrentNode;

						NodeStatus status = currentNode.Execute();

						if (currentNode is ActionNode)
						{
							actionExecutionCount++;

							bool isBreak = false;

							switch (executionSettings.type)
							{
								case ExecutionType.UntilRunning:
									isBreak = status == NodeStatus.Running;
									break;
								case ExecutionType.Count:
									isBreak = actionExecutionCount >= executionSettings.maxCount;
									break;
							}

							if (isBreak)
							{
								break;
							}
						}

						if (status != NodeStatus.Running)
						{
							Pop(status);

							if (_CurrentNode == null)
							{
								INodeGraphContainer graphContainer = ownerBehaviour as INodeGraphContainer;
								if (graphContainer != null)
								{
									graphContainer.OnFinishNodeGraph(this, status == NodeStatus.Success);
								}

								if (playState != PlayState.Stopping)
								{
									if (restartOnFinish)
									{
										foreach (var revaluator in _Revaluators)
										{
											TreeBehaviourNode treeBehaviourNode = revaluator as TreeBehaviourNode;
											if (treeBehaviourNode != null)
											{
												treeBehaviourNode.RevaluationExit();
											}
										}
										_Revaluators.Clear();
										Push(_RootNode);
									}
									else
									{
										Stop();
									}
								}

								break;
							}
						}
					}
					else
					{
						_CurrentNode.Abort();
						Pop(_CurrentNode.status);
					}

					if (_InterruptCount >= currentDebugInfiniteLoopSettings.maxLoopCount)
					{
						if (currentDebugInfiniteLoopSettings.enableLogging)
						{
							Debug.LogWarning("Over " + currentDebugInfiniteLoopSettings.maxLoopCount + " interrupts per frame. Please check the infinite loop of " + ToString(), this);
						}

						if (currentDebugInfiniteLoopSettings.enableBreak)
						{
							Debug.Break();
						}
						break;
					}

					if (!isActiveAndEnabled)
					{
						break;
					}
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// LateUpdateの際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when LateUpdate.
		/// </summary>
#endif
		protected sealed override void OnLateUpdate()
		{
			using (CalculateScope.OpenScope())
			{
				int activeNodeCount = _ActiveNodes.Count;
				for (int activeIndex = 0; activeIndex < activeNodeCount; activeIndex++)
				{
					TreeNodeBase node = _ActiveNodes[activeIndex];
					node.OnLateUpdate();
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FixedUpdateの際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when FixedUpdate.
		/// </summary>
#endif
		protected sealed override void OnFixedUpdate()
		{
			using (CalculateScope.OpenScope())
			{
				int activeNodeCount = _ActiveNodes.Count;
				for (int activeIndex = 0; activeIndex < activeNodeCount; activeIndex++)
				{
					TreeNodeBase node = _ActiveNodes[activeIndex];
					node.OnFixedUpdate();
				}
			}
		}

		TreeNodeBase FindRevaluator(TreeNodeBase node)
		{
			int count = _Revaluators.Count;
			for (int i = 0; i < count; ++i)
			{
				TreeNodeBase revaluatorNode = _Revaluators[i];
				if (revaluatorNode == node)
				{
					return revaluatorNode;
				}
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再評価ノードかを返す。
		/// </summary>
		/// <param name="node">ノード</param>
		/// <returns>再評価ノードであればtrueを返す。</returns>
#else
		/// <summary>
		/// It returns the reevaluation node.
		/// </summary>
		/// <param name="node">Node</param>
		/// <returns>Returns true if it is a reevaluation node.</returns>
#endif
		public bool IsRevaluation(TreeNodeBase node)
		{
			return _Revaluators.Contains(node);
		}

		internal bool RegisterRevaluation(TreeNodeBase node)
		{
			if (!_Revaluators.Contains(node))
			{
				_Revaluators.Add(node);

				TreeBehaviourNode treeBehaviourNode = node as TreeBehaviourNode;
				if (treeBehaviourNode != null)
				{
					treeBehaviourNode.RevaluationEnter();
				}
				return true;
			}
			return false;
		}

		void AbortPop(TreeNodeBase targetNode)
		{
			while (_CurrentNode != null && _CurrentNode != targetNode)
			{
				_CurrentNode.Abort();
				Pop();
			}
		}

		bool IsRevaluationLowPriority(TreeNodeBase revaluationNode)
		{
			return !revaluationNode.isActive && revaluationNode.HasAbortFlags(AbortFlags.LowerPriority) && revaluationNode.priority < _CurrentNode.priority;
		}

		bool Revaluation(TreeNodeBase revaluationNode)
		{
			if (revaluationNode.isActive)
			{
				bool condition = revaluationNode.ConditionCheck(AbortFlags.Self);
				if (!condition)
				{
					if (_CurrentNode != revaluationNode)
					{
						AbortPop(revaluationNode);
					}

					revaluationNode.Abort();
					Pop(revaluationNode.status);

					return true;
				}
			}
			else if (IsRevaluationLowPriority(revaluationNode))
			{
				bool condition = revaluationNode.ConditionCheck(AbortFlags.LowerPriority);
				if (condition)
				{
					if (!revaluationNode.ConditionCheck(0))
					{
						return false;
					}

					TreeNodeBase commonAncestorNode = CommonAncestorNode(_CurrentNode, revaluationNode);
					if (commonAncestorNode == null)
					{
						return false;
					}

					List<TreeNodeBase> treeNodes = new List<TreeNodeBase>();
					treeNodes.Insert(0, revaluationNode);

					TreeNodeBase parentNode = revaluationNode.parentNode;
					while (parentNode != null && parentNode != commonAncestorNode)
					{
						if (parentNode.HasConditionCheck())
						{
							if (!parentNode.ConditionCheck(0))
							{
								return false;
							}
						}

						treeNodes.Insert(0, parentNode);
						parentNode = parentNode.parentNode;
					}

					treeNodes.Insert(0, commonAncestorNode);

					int revaluatorCount = _Revaluators.Count;
					for (int revaluatorIndex = revaluatorCount - 1; revaluatorIndex >= 0; --revaluatorIndex)
					{
						TreeNodeBase revaluator = _Revaluators[revaluatorIndex];
						if (revaluationNode.priority < revaluator.priority)
						{
							_Revaluators.RemoveAt(revaluatorIndex);

							TreeBehaviourNode treeBehaviourNode = revaluator as TreeBehaviourNode;
							if (treeBehaviourNode != null)
							{
								treeBehaviourNode.RevaluationExit();
							}
						}
					}

					AbortPop(commonAncestorNode);

					int parentCount = treeNodes.Count;
					for (int parentIndex = 0; parentIndex < parentCount - 1; parentIndex++)
					{
						TreeNodeBase node = treeNodes[parentIndex];
						TreeNodeBase childNode = treeNodes[parentIndex + 1];

						CompositeNode compositeNode = node as CompositeNode;
						int interruptSlot = (compositeNode != null)? compositeNode.OnInterruput(childNode) : 0;
						if (interruptSlot != 0)
						{
							Push(interruptSlot);
						}
						else
						{
							Push(childNode);
						}

						childNode.Activate(true, true, childNode == revaluationNode);

						TreeBehaviourNode treeBehaviourNode = childNode as TreeBehaviourNode;
						if (treeBehaviourNode != null && treeBehaviourNode.breakPoint)
						{
							_IsBreakPoint = true;
						}
					}

					_InterruptCount++;

					return true;
				}
			}

			return false;
		}

		bool Revaluation()
		{
			int count = _Revaluators.Count;
			for (int i = 0; i < count; ++i)
			{
				TreeNodeBase revaluator = _Revaluators[i];
				if (Revaluation(revaluator))
				{
					return true;
				}
			}
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// CompositeNodeを生成。
		/// </summary>
		/// <param name="position">ノードの位置</param>
		/// <param name="nodeID">ノードID</param>
		/// <param name="classType">CompositeBehaviourの型</param>
		/// <returns>生成したCompositeNode。</returns>
#else
		/// <summary>
		/// Create Composite.
		/// </summary>
		/// <param name="position">Node position</param>
		/// <param name="nodeID">Node ID</param>
		/// <param name="classType">CompositeBehaviour type</param>
		/// <returns>The created coomposite node.</returns>
#endif
		public CompositeNode CreateComposite(Vector2 position, int nodeID, System.Type classType)
		{
			if (!IsUniqueNodeID(nodeID))
			{
				Debug.LogWarning("CreateComposite id(" + nodeID + ") is not unique.");
				return null;
			}

			CompositeNode compositeNode = new CompositeNode(this, nodeID, classType);
			compositeNode.position = new Rect(position.x, position.y, Node.defaultWidth, 0);

			ComponentUtility.RecordObject(this, "Created CompositeNode");

			_CompositeNodes.Add(compositeNode);
			RegisterNode(compositeNode);

			ComponentUtility.SetDirty(this);

			return compositeNode;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// CompositeNodeを生成。
		/// </summary>
		/// <param name="position">ノードの位置</param>
		/// <param name="classType">CompositeBehaviourの型</param>
		/// <returns>生成したCompositeNode。</returns>
#else
		/// <summary>
		/// Create Composite.
		/// </summary>
		/// <param name="position">Node position</param>
		/// <param name="classType">CompositeBehaviour type</param>
		/// <returns>The created coomposite node.</returns>
#endif
		public CompositeNode CreateComposite(Vector2 position, System.Type classType)
		{
			return CreateComposite(position, GetUniqueNodeID(), classType);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ActionNodeを生成。
		/// </summary>
		/// <param name="position">ノードの位置</param>
		/// <param name="nodeID">ノードID</param>
		/// <param name="classType">ActionBehaviourの型</param>
		/// <returns>生成したActionNode。</returns>
#else
		/// <summary>
		/// Create ActionNode.
		/// </summary>
		/// <param name="position">Node position</param>
		/// <param name="nodeID">Node ID</param>
		/// <param name="classType">ActionBehaviour type</param>
		/// <returns>The created action node.</returns>
#endif
		public ActionNode CreateAction(Vector2 position, int nodeID, System.Type classType)
		{
			if (!IsUniqueNodeID(nodeID))
			{
				Debug.LogWarning("CreateAction id(" + nodeID + ") is not unique.");
				return null;
			}

			ActionNode actionNode = new ActionNode(this, nodeID, classType);
			actionNode.position = new Rect(position.x, position.y, Node.defaultWidth, 0);

			ComponentUtility.RecordObject(this, "Created Action");

			_ActionNodes.Add(actionNode);
			RegisterNode(actionNode);

			ComponentUtility.SetDirty(this);

			return actionNode;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ActionNodeを生成。
		/// </summary>
		/// <param name="position">ノードの位置</param>
		/// <param name="classType">ActionBehaviourの型</param>
		/// <returns>生成したActionNode。</returns>
#else
		/// <summary>
		/// Create ActionNode.
		/// </summary>
		/// <param name="position">Node position</param>
		/// <param name="classType">ActionBehaviour type</param>
		/// <returns>The created action node.</returns>
#endif
		public ActionNode CreateAction(Vector2 position, System.Type classType)
		{
			return CreateAction(position, GetUniqueNodeID(), classType);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Resetもしくは生成時のコールバック。
		/// </summary>
#else
		/// <summary>
		/// Reset or create callback.
		/// </summary>
#endif
		protected sealed override void OnReset()
		{
			graphName = "New BehaviourTree";

			_RootNode = new RootNode(this, 1);
			_RootNode.position = new Rect(0, 0, Node.defaultWidth, 0);

			RegisterNode(_RootNode);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードの優先度を計算する。
		/// </summary>
#else
		/// <summary>
		/// Calculate priority of nodes.
		/// </summary>
#endif
		public void CalculatePriority()
		{
			ComponentUtility.RecordObject(this, "Calculate Priority");

			int nodeCount = this.nodeCount;
			for (int nodeIndex = 0; nodeIndex < nodeCount; nodeIndex++)
			{
				TreeNodeBase treeNode = GetNodeFromIndex(nodeIndex) as TreeNodeBase;
				if (treeNode != null)
				{
					treeNode.ClearPriority();
				}
			}

			_RootNode.CalculatePriority(0);

			ComponentUtility.SetDirty(this);
		}

		void DisconnectBranch(TreeNodeBase treeNode)
		{
			ComponentUtility.RecordObject(this, "Disconnect NodeBranch");

			List<NodeBranch> branchies = new List<NodeBranch>();

			int branchCount = _NodeBranchies.count;
			for (int branchIndex = 0; branchIndex < branchCount; branchIndex++)
			{
				NodeBranch branch = _NodeBranchies[branchIndex];
				if (branch.parentNodeID == treeNode.nodeID || branch.childNodeID == treeNode.nodeID)
				{
					branchies.Add(branch);
				}
			}

			branchCount = branchies.Count;
			for (int branchIndex = 0; branchIndex < branchCount; branchIndex++)
			{
				NodeBranch branch = branchies[branchIndex];
				DisconnectBranch(branch);
			}

			ComponentUtility.SetDirty(this);
		}

		void DeleteActionNode(ActionNode actionNode)
		{
			ComponentUtility.RegisterCompleteObjectUndo(this, "Delete Nodes");

			ComponentUtility.RecordObject(this, "Delete Nodes");
			actionNodes.Remove(actionNode);
			RemoveNode(actionNode);
			DisconnectBranch(actionNode);

			actionNode.DestroyAllBehaviour();

			_Revaluators.Remove(actionNode);

			ComponentUtility.SetDirty(this);
		}

		void DeleteCompositeNode(CompositeNode compositeNode)
		{
			ComponentUtility.RegisterCompleteObjectUndo(this, "Delete Nodes");

			ComponentUtility.RecordObject(this, "Delete Nodes");
			compositeNodes.Remove(compositeNode);
			RemoveNode(compositeNode);
			DisconnectBranch(compositeNode);

			compositeNode.DestroyAllBehaviour();

			_Revaluators.Remove(compositeNode);

			ComponentUtility.SetDirty(this);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードの削除。
		/// </summary>
		/// <param name="node">削除するノード</param>
		/// <returns>削除した場合はtrue、していなければfalseを返す。</returns>
#else
		/// <summary>
		/// Delete node.
		/// </summary>
		/// <param name="node">The node to delete</param>
		/// <returns>Returns true if deleted, false otherwise.</returns>
#endif
		protected sealed override bool OnDeleteNode(Node node)
		{
			ActionNode actionNode = node as ActionNode;
			if (actionNode != null)
			{
				DeleteActionNode(actionNode);
				return true;
			}

			CompositeNode compositeNode = node as CompositeNode;
			if (compositeNode != null)
			{
				DeleteCompositeNode(compositeNode);
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードが変更された際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when the node is changed.
		/// </summary>
#endif
		public override void OnValidateNodes()
		{
			CalculatePriority();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードの接続がループしているかチェックする。
		/// </summary>
		/// <param name="parentNode">親ノード</param>
		/// <param name="childNode">子ノード</param>
		/// <returns>ループしている場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Check whether the connection of the node is looping.
		/// </summary>
		/// <param name="parentNode">Parent node</param>
		/// <param name="childNode">Child node</param>
		/// <returns>Returns true if it is looping.</returns>
#endif
		public bool CheckLoop(TreeNodeBase parentNode, TreeNodeBase childNode)
		{
			if (parentNode == null || childNode == null)
			{
				return false;
			}

			TreeNodeBase current = parentNode;
			while (current != null && current is IParentLinkSlotHolder)
			{
				TreeNodeBase p = current.parentNode;
				if (p == null)
				{
					break;
				}

				if (p.nodeID == childNode.nodeID)
				{
					return true;
				}

				current = p;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBranchの接続
		/// </summary>
		/// <param name="branchID">作成するNodeBranchのID</param>
		/// <param name="parentNode">親ノード</param>
		/// <param name="childNode">子ノード</param>
		/// <returns>接続したNodeBranch</returns>
#else
		/// <summary>
		/// Connect NodeBranch.
		/// </summary>
		/// <param name="branchID">ID of the NodeBranch to be created</param>
		/// <param name="parentNode">Parent node.</param>
		/// <param name="childNode">Child node.</param>
		/// <returns>Connected NodeBranch</returns>
#endif
		public NodeBranch ConnectBranch(int branchID, TreeNodeBase parentNode, TreeNodeBase childNode)
		{
			if (_NodeBranchies.GetFromID(branchID) != null)
			{
				Debug.LogError("It already exists branchID.");
				return null;
			}

			if (CheckLoop(parentNode, childNode))
			{
				Debug.LogError("Node has become an infinite loop.");
				return null;
			}

			NodeBranch branch = new NodeBranch();
			branch.branchID = branchID;
			branch.parentNodeID = parentNode.nodeID;
			branch.childNodeID = childNode.nodeID;

			ComponentUtility.RecordObject(this, "Connect NodeBranch");

			_NodeBranchies.Add(branch);

			var childLinkSlotHolder = parentNode as IChildLinkSlotHolder;
			if (childLinkSlotHolder != null)
			{
				childLinkSlotHolder.ConnectChildLinkSlot(branch.branchID);
			}

			var parentLinkHolder = childNode as IParentLinkSlotHolder;
			if (parentLinkHolder != null)
			{
				var parentLink = parentLinkHolder.GetParentLinkSlot();
				NodeBranch oldBranch = _NodeBranchies.GetFromID(parentLink.branchID);
				if (oldBranch != null)
				{
					DisconnectBranch(oldBranch);

					oldBranch = null;
				}
				parentLink.SetBranch(branchID);
			}

			ComponentUtility.SetDirty(this);

			return branch;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBranchの接続
		/// </summary>
		/// <param name="parentNode">親ノード</param>
		/// <param name="childNode">子ノード</param>
		/// <returns>接続したNodeBranch</returns>
#else
		/// <summary>
		/// Connect NodeBranch.
		/// </summary>
		/// <param name="parentNode">Parent node.</param>
		/// <param name="childNode">Child node.</param>
		/// <returns>Connected NodeBranch</returns>
#endif
		public NodeBranch ConnectBranch(TreeNodeBase parentNode, TreeNodeBase childNode)
		{
			return ConnectBranch(nodeBranchies.GetUniqueBranchID(), parentNode, childNode);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBranchの切断
		/// </summary>
		/// <param name="branch">切断するNodeBranch</param>
#else
		/// <summary>
		/// Disconnect NodeBranch
		/// </summary>
		/// <param name="branch">Disconnect NodeBranch</param>
#endif
		public void DisconnectBranch(NodeBranch branch)
		{
			TreeNodeBase parentNode = GetNodeFromID(branch.parentNodeID) as TreeNodeBase;
			TreeNodeBase childNode = GetNodeFromID(branch.childNodeID) as TreeNodeBase;

			ComponentUtility.RecordObject(this, "Disconnect NodeBranch");

			var childLinkSlotHolder = parentNode as IChildLinkSlotHolder;
			if (childLinkSlotHolder != null)
			{
				childLinkSlotHolder.DisconnectChildLinkSlot(branch.branchID);
			}

			var parentLinkHolder = childNode as IParentLinkSlotHolder;
			if (parentLinkHolder != null)
			{
				var parentLink = parentLinkHolder.GetParentLinkSlot();
				parentLink.RemoveBranch(branch.branchID);
			}

			_NodeBranchies.Remove(branch);

			ComponentUtility.SetDirty(this);
		}

		internal TreeNodeBase Push(int branchID)
		{
			NodeBranch branch = _NodeBranchies.GetFromID(branchID);
			if (branch == null)
			{
				return null;
			}

			TreeNodeBase node = GetNodeFromID(branch.childNodeID) as TreeNodeBase;
			if (node == null)
			{
				return null;
			}

			branch.isActive = true;

			Push(node);
			return node;
		}

		void Push(TreeNodeBase node)
		{
			if (_CurrentNode == node)
			{
				return;
			}

			_CurrentNode = node;
			StateChanged();

			if (_CurrentNode == null)
			{
				return;
			}

			_ActiveNodes.Add(_CurrentNode);
		}

		void Pop()
		{
			if (_CurrentNode == null)
			{
				return;
			}

			_ActiveNodes.Remove(_CurrentNode);
			_CurrentNode.Activate(false, false, false);

			var parentLinkHolder = _CurrentNode as IParentLinkSlotHolder;
			if (parentLinkHolder != null)
			{
				TreeNodeBase parentNode = null;

				NodeBranch branch = parentLinkHolder.GetParentBranch();
				if (branch != null)
				{
					branch.isActive = false;
					parentNode = GetNodeFromID(branch.parentNodeID) as TreeNodeBase;
				}
				_CurrentNode = parentNode;
			}
			else
			{
				_CurrentNode = null;
			}

			StateChanged();
		}

		void Pop(NodeStatus childStatus)
		{
			Pop();

			if (_CurrentNode != null)
			{
				_CurrentNode.OnChildExecuted(childStatus);
			}
		}

		private TreeNodeBase CommonAncestorNode(TreeNodeBase node1, TreeNodeBase node2)
		{
			if (node1 == node2.parentNode)
			{
				return node1;
			}
			else if (node2 == node1.parentNode)
			{
				return node2;
			}

			HashSet<TreeNodeBase> parentNodes = new HashSet<TreeNodeBase>();

			TreeNodeBase parent1 = node1.parentNode;
			while (parent1 != null)
			{
				parentNodes.Add(parent1);
				parent1 = parent1.parentNode;
			}

			TreeNodeBase parent2 = node2.parentNode;
			TreeNodeBase current = parent2;
			while (current != null && parent2 != null && !parentNodes.Contains(current))
			{
				parent2 = parent2.parentNode;
				current = parent2;
			}

			return current;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 手動で実行する。
		/// UpdateSettings.typeがManualの場合に任意のタイミングでこのメソッドを呼んでください。
		/// </summary>
#else
		/// <summary>
		/// Execute manually.
		/// Please call this method at any timing when UpdateSettings.type is Manual.
		/// </summary>
#endif
		[System.Obsolete("use ExecuteUpdate")]
		public void Execute()
		{
			base.ExecuteUpdate();
		}

		/// <summary>
		/// Register nodes
		/// </summary>
		protected sealed override void OnRegisterNodes()
		{
			RegisterNode(_RootNode);

			for (int i = 0; i < _CompositeNodes.count; i++)
			{
				RegisterNode(_CompositeNodes[i]);
			}

			for (int i = 0; i < _ActionNodes.count; i++)
			{
				RegisterNode(_ActionNodes[i]);
			}
		}

		private class NodeLinkSlotComparer : IComparer<int>
		{
			public BehaviourTreeInternal behaviourTree
			{
				get;
				private set;
			}

			public NodeLinkSlotComparer(BehaviourTreeInternal behaviourTreeInternal)
			{
				behaviourTree = behaviourTreeInternal;
			}

			public int Compare(int a, int b)
			{
				TreeNodeBase a_node = null;
				TreeNodeBase b_node = null;

				if (a == b)
				{
					return 0;
				}

				NodeBranch a_branch = behaviourTree.nodeBranchies.GetFromID(a);
				if (a_branch != null)
				{
					a_node = behaviourTree.GetNodeFromID(a_branch.childNodeID) as TreeNodeBase;
				}

				NodeBranch b_branch = behaviourTree.nodeBranchies.GetFromID(b);
				if (b_branch != null)
				{
					b_node = behaviourTree.GetNodeFromID(b_branch.childNodeID) as TreeNodeBase;
				}

				if (a_node == null || b_node == null)
				{
					return 0;
				}

				if (a_node.position.x == b_node.position.x)
				{
					return -1;
				}

				return a_node.position.x.CompareTo(b_node.position.x);
			}
		}

		private NodeLinkSlotComparer _NodeLinkSlotComparer = null;

		internal void SortNodeLinkSlot(ChildrenLinkSlot childrenLink)
		{
			if (_NodeLinkSlotComparer == null || _NodeLinkSlotComparer.behaviourTree != this)
			{
				_NodeLinkSlotComparer = new NodeLinkSlotComparer(this);
			}
			childrenLink.branchIDs.Sort(_NodeLinkSlotComparer);
		}
	}
}