//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.BehaviourTree
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ルートノード
	/// </summary>
#else
	/// <summary>
	/// Root Node
	/// </summary>
#endif
	[System.Serializable]
	public sealed class RootNode : TreeNodeBase, IChildLinkSlotHolder
	{
		#region Serialize field

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
		[FormerlySerializedAs("childNodeLink")]
		private ChildLinkSlot _ChildNodeLink = new ChildLinkSlot();

		#endregion // Serialize field

#if ARBOR_DOC_JA
		/// <summary>
		/// 子ノードを取得。
		/// </summary>
#else
		/// <summary>
		/// Get child node.
		/// </summary>
#endif
		public TreeNodeBase childNode
		{
			get
			{
				NodeBranch branch = behaviourTree.nodeBranchies.GetFromID(_ChildNodeLink.branchID);
				if (branch != null)
				{
					return behaviourTree.GetNodeFromID(branch.childNodeID) as TreeNodeBase;
				}
				return null;
			}
		}

		internal RootNode(NodeGraph nodeGraph, int nodeID) : base(nodeGraph, nodeID)
		{
			name = "Root";
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ChildLinkSlotを取得する。
		/// </summary>
		/// <returns>ChildLinkSlotを返す。</returns>
#else
		/// <summary>
		/// Get ChildLinkSlot.
		/// </summary>
		/// <returns>Returns a ChildLinkSlot.</returns>
#endif
		public ChildLinkSlot GetChildLinkSlot()
		{
			return _ChildNodeLink;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 子ノードに接続しているNodeBranchを取得する。
		/// </summary>
		/// <returns>子ノードに接続しているNodeBranchを返す。</returns>
#else
		/// <summary>
		/// Get the NodeBranch connected to the child node.
		/// </summary>
		/// <returns>Returns a NodeBranch connected to a child node.</returns>
#endif
		public NodeBranch GetChildBranch()
		{
			return behaviourTree.nodeBranchies.GetFromID(_ChildNodeLink.branchID);
		}

		void IChildLinkSlotHolder.ConnectChildLinkSlot(int branchID)
		{
			NodeBranch oldBranch = behaviourTree.nodeBranchies.GetFromID(_ChildNodeLink.branchID);
			if (oldBranch != null)
			{
				behaviourTree.DisconnectBranch(oldBranch);

				oldBranch = null;
			}
			_ChildNodeLink.SetBranch(branchID);
		}

		void IChildLinkSlotHolder.DisconnectChildLinkSlot(int branchID)
		{
			_ChildNodeLink.RemoveBranch(branchID);
		}

		int IChildLinkSlotHolder.OnCalculateChildPriority(int order)
		{
			TreeNodeBase childNode = this.childNode;
			if (childNode != null)
			{
				order = childNode.CalculatePriority(order);
			}
			return order;
		}

		private bool _IsChildExecute = false;

		internal override bool OnActivate(bool active, bool interrupt, bool isRevaluator)
		{
			if (!base.OnActivate(active, interrupt, isRevaluator))
			{
				return false;
			}

			if (active)
			{
				_IsChildExecute = false;
			}

			return true;
		}

		private NodeStatus _ChildNodeStatus = NodeStatus.Running;

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
			if (_IsChildExecute)
			{
				FinishExecute(_ChildNodeStatus == NodeStatus.Success);
				return;
			}
			else
			{
				_IsChildExecute = true;

				TreeNodeBase childNode = behaviourTree.Push(_ChildNodeLink.branchID);
				if (childNode == null)
				{
					FinishExecute(false);
					return;
				}
			}
		}

		internal override void OnChildExecuted(NodeStatus status)
		{
			_ChildNodeStatus = status;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 削除できるかどうかを返す。
		/// </summary>
		/// <returns>削除できる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether or not it can be deleted.
		/// </summary>
		/// <returns>Returns true if it can be deleted.</returns>
#endif
		public override bool IsDeletable()
		{
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Arbor3.9.0より前のノード名を取得する。
		/// </summary>
		/// <returns>旧ノード名</returns>
#else
		/// <summary>
		/// Get the node name before Arbor 3.9.0.
		/// </summary>
		/// <returns>Old node name</returns>
#endif
		protected override string GetOldName()
		{
			return "Root";
		}
	}
}