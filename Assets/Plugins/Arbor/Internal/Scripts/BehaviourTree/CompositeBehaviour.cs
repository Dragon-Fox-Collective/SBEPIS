//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor.BehaviourTree
{
#if ARBOR_DOC_JA
	/// <summary>
	/// コンポジットの挙動を定義するクラス。継承して利用する。
	/// </summary>
#else
	/// <summary>
	/// Class that defines the behavior of the composite. Inherited and to use.
	/// </summary>
#endif
	[AddComponentMenu("")]
	public class CompositeBehaviour : TreeNodeBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// CompositeNodeを取得。
		/// </summary>
#else
		/// <summary>
		/// Get the CompositeNode.
		/// </summary>
#endif
		public CompositeNode compositeNode
		{
			get
			{
				return node as CompositeNode;
			}
		}

		internal static CompositeBehaviour Create(Node node, System.Type type)
		{
			System.Type classType = typeof(CompositeBehaviour);
			if (type != classType && !TypeUtility.IsSubclassOf(type, classType))
			{
				throw new System.ArgumentException("The type `" + type.Name + "' must be convertible to `CompositeBehaviour' in order to use it as parameter `type'", "type");
			}

			return CreateNodeBehaviour(node, type) as CompositeBehaviour;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始時に実行する子ノードのインデックスを取得する。
		/// </summary>
		/// <returns>子ノードのインデックス</returns>
#else
		/// <summary>
		/// Get the child node index to be executed at the start.
		/// </summary>
		/// <returns>Index of child node</returns>
#endif
		public virtual int GetBeginIndex()
		{
			return 0;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 次に実行する子ノードのインデックスを取得する。
		/// </summary>
		/// <param name="currentIndex">現在のインデックス</param>
		/// <returns>子ノードのインデックス</returns>
#else
		/// <summary>
		/// Get the child node index to be executed.
		/// </summary>
		/// <param name="currentIndex">Current index</param>
		/// <returns>Index of child node</returns>
#endif
		public virtual int GetNextIndex(int currentIndex)
		{
			return currentIndex + 1;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 割り込んだノードのインデックスを取得する。
		/// </summary>
		/// <param name="node">割り込んだノード</param>
		/// <returns>子ノードのインデックス</returns>
#else
		/// <summary>
		/// Get the index of the interrupted node.
		/// </summary>
		/// <param name="node">Interrupted node</param>
		/// <returns>Index of child node</returns>
#endif
		public virtual int GetInterruptIndex(TreeNodeBase node)
		{
			var childrenLinkSlot = compositeNode.GetChildrenLinkSlot();
			int childCount = childrenLinkSlot.branchIDs.Count;
			var nodeBrachies = compositeNode.behaviourTree.nodeBranchies;
			for (int childIndex = 0; childIndex < childCount; ++childIndex)
			{
				NodeBranch branch = nodeBrachies.GetFromID(childrenLinkSlot.branchIDs[childIndex]);
				if (branch.childNodeID == node.nodeID)
				{
					return childIndex;
				}
			}

			return -1;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 実行できるか判定する。
		/// </summary>
		/// <param name="childStatus">子ノードの状態</param>
		/// <returns>実行できる場合はtrueを返す。</returns>
#else
		/// <summary>
		/// It is judged whether it can be executed.
		/// </summary>
		/// <param name="childStatus">State of child node</param>
		/// <returns>Returns true if it can be executed.</returns>
#endif
		public virtual bool CanExecute(NodeStatus childStatus)
		{
			return true;
		}
	}
}