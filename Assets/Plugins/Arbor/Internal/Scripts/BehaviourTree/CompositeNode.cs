//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

namespace Arbor.BehaviourTree
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 子ノードの実行を制御するノード。
	/// </summary>
#else
	/// <summary>
	/// This node controls the execution of child nodes.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class CompositeNode : TreeBehaviourNode, IChildLinkSlotHolder, ISerializeVersionCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 子ノードへのリンク
		/// </summary>
#else
		/// <summary>
		/// Link to child nodes.
		/// </summary>
#endif
		[SerializeField]
		private ChildrenLinkSlot _ChildrenLink = new ChildrenLinkSlot();

		[SerializeField]
		private SerializeVersion _SerializeVersion = new SerializeVersion();

		#region old

		[SerializeField]
		[FormerlySerializedAs("childrenLink")]
		private List<NodeLinkSlotLegacy> _OldChildrenLink = new List<NodeLinkSlotLegacy>();

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		private int _CurrentIndex = 0;

		private CompositeNode() : base(null, 0)
		{
			// Initialize when calling from script.
			_SerializeVersion.Initialize(this);
		}		

		internal CompositeNode(NodeGraph nodeGraph, int nodeID, System.Type classType)
			: base(nodeGraph, nodeID)
		{
			name = "New Composite";
			CreateCompositeBehaviour(classType);
			// Initialize when calling from script.
			_SerializeVersion.Initialize(this);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// CompositeBehaviourを作成する。エディタで使用する。
		/// </summary>
		/// <param name="classType">CompositeBehaviourの型</param>
		/// <returns>作成したCompositeBehaviourを返す。</returns>
#else
		/// <summary>
		/// Create a CompositeBehaviour. Use it in the editor.
		/// </summary>
		/// <param name="classType">CompositeBehaviour type</param>
		/// <returns>Returns the created CompositeBehaviour.</returns>
#endif
		public CompositeBehaviour CreateCompositeBehaviour(System.Type classType)
		{
			CompositeBehaviour behaviour = CompositeBehaviour.Create(this, classType);
			SetBehaviour(behaviour);
			return behaviour;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ChildrenLinkSlotを取得する。
		/// </summary>
		/// <returns>ChildrenLinkSlotを返す。</returns>
#else
		/// <summary>
		/// Get ChildrenLinkSlot.
		/// </summary>
		/// <returns>Returns a ChildrenLinkSlot.</returns>
#endif
		public ChildrenLinkSlot GetChildrenLinkSlot()
		{
			return _ChildrenLink;
		}

		void IChildLinkSlotHolder.ConnectChildLinkSlot(int branchID)
		{
			_ChildrenLink.AddBranch(branchID);
		}

		void IChildLinkSlotHolder.DisconnectChildLinkSlot(int branchID)
		{
			_ChildrenLink.RemoveBranch(branchID);
		}

		int IChildLinkSlotHolder.OnCalculateChildPriority(int order)
		{
			int currentLink = (isActive && _ChildrenLink.branchIDs.Count > 0) ? _ChildrenLink.branchIDs[_CurrentIndex] : 0;

			behaviourTree.SortNodeLinkSlot(_ChildrenLink);

			for (int slotIndex = 0; slotIndex < _ChildrenLink.branchIDs.Count; slotIndex++)
			{
				int branchID = _ChildrenLink.branchIDs[slotIndex];
				NodeBranch branch = behaviourTree.nodeBranchies.GetFromID(branchID);
				if (branch != null)
				{
					TreeNodeBase childNode = behaviourTree.GetNodeFromID(branch.childNodeID) as TreeNodeBase;
					if (childNode != null)
					{
						order = childNode.CalculatePriority(order);
					}
				}
			}

			if (currentLink != 0)
			{
				_CurrentIndex = _ChildrenLink.branchIDs.IndexOf(currentLink);
			}

			return order;
		}

		private NodeStatus _ChildNodeStatus = NodeStatus.Running;

		void InitializeChildStatus(bool interrupt, bool isRevaluator)
		{
			if (!interrupt || isRevaluator)
			{
				CompositeBehaviour compositeBehaviour = behaviour as CompositeBehaviour;
				if (compositeBehaviour != null)
				{
					_CurrentIndex = compositeBehaviour.GetBeginIndex();
				}
				else
				{
					_CurrentIndex = -1;
				}
			}

			_ChildNodeStatus = NodeStatus.Running;
		}

		internal override bool OnActivate(bool active, bool interrupt, bool isRevaluator)
		{
			if (!base.OnActivate(active, interrupt, isRevaluator))
			{
				return false;
			}

			if (active)
			{
				InitializeChildStatus(interrupt, isRevaluator);
			}

			return true;
		}

		internal override void OnRestart()
		{
			InitializeChildStatus(false, false);

			base.OnRestart();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 実行する際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when executing.
		/// </summary>
#endif
		protected override void OnExecute()
		{
			CompositeBehaviour compositeBehaviour = behaviour as CompositeBehaviour;
			if (compositeBehaviour == null)
			{
				FinishExecute(false);
				return;
			}

			if (0 <= _CurrentIndex && _CurrentIndex < _ChildrenLink.branchIDs.Count && compositeBehaviour.CanExecute(_ChildNodeStatus))
			{
				int branchID = _ChildrenLink.branchIDs[_CurrentIndex];
				behaviourTree.Push(branchID);
			}
			else
			{
				FinishExecute(_ChildNodeStatus == NodeStatus.Success);
			}
		}

		internal int OnInterruput(TreeNodeBase node)
		{
			CompositeBehaviour compositeBehaviour = behaviour as CompositeBehaviour;
			if (compositeBehaviour != null)
			{
				_CurrentIndex = compositeBehaviour.GetInterruptIndex(node);
			}
			else
			{
				_CurrentIndex = -1;
			}

			if (0 <= _CurrentIndex && _CurrentIndex < _ChildrenLink.branchIDs.Count)
			{
				return _ChildrenLink.branchIDs[_CurrentIndex];
			}
			else
			{
				return 0;
			}
		}

		internal override void OnChildExecuted(NodeStatus childStatus)
		{
			CompositeBehaviour compositeBehaviour = behaviour as CompositeBehaviour;
			if (compositeBehaviour != null)
			{
				_CurrentIndex = compositeBehaviour.GetNextIndex(_CurrentIndex);
			}
			else
			{
				_CurrentIndex++;
			}

			_ChildNodeStatus = childStatus;
		}

		void SerializeVer1()
		{
			_ChildrenLink.Import(_OldChildrenLink);
		}

		#region ISerializeVersionCallbackReceiver

		int ISerializeVersionCallbackReceiver.newestVersion
		{
			get
			{
				return kCurrentSerializeVersion;
			}
		}

		void ISerializeVersionCallbackReceiver.OnInitialize()
		{
			_SerializeVersion.version = kCurrentSerializeVersion;
		}

		void ISerializeVersionCallbackReceiver.OnSerialize(int version)
		{
			switch (version)
			{
				case 0:
					SerializeVer1();
					break;
			}
		}

		void ISerializeVersionCallbackReceiver.OnVersioning()
		{
		}

		#endregion // ISerializeVersionCallbackReceiver

		#region ISerializationCallbackReceiver

#if ARBOR_DOC_JA
		/// <summary>
		/// ISerializationCallbackReceiver.OnAfterDeserializeから呼び出される。
		/// </summary>
#else
		/// <summary>
		/// Called from ISerializationCallbackReceiver.OnAfterDeserialize.
		/// </summary>
#endif
		protected override void OnAfterDeserialize()
		{
			_SerializeVersion.AfterDeserialize();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ISerializationCallbackReceiver.OnBeforeSerialize。
		/// </summary>
#else
		/// <summary>
		/// Called from ISerializationCallbackReceiver.OnBeforeSerialize.
		/// </summary>
#endif
		protected override void OnBeforeSerialize()
		{
			_SerializeVersion.BeforeDeserialize();
		}

		#endregion ISerializationCallbackReceiver
	}
}