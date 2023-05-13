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
	/// NodeBranchのリスト
	/// </summary>
#else
	/// <summary>
	/// List of NodeBranch
	/// </summary>
#endif
	[System.Serializable]
	public sealed class NodeBranchies
	{
		#region Serialize fields

		[SerializeField]
#if !ARBOR_DEBUG
		[HideInInspector]
#endif
		private List<NodeBranch> _NodeBranchies = new List<NodeBranch>();

		#endregion // Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBranchの数を取得。
		/// </summary>
#else
		/// <summary>
		///  Get a count of NodeBranch.
		/// </summary>
#endif
		public int count
		{
			get
			{
				return _NodeBranchies.Count;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBranchをインデックスから取得
		/// </summary>
		/// <param name="index">インデックス</param>
#else
		/// <summary>
		/// Get NodeBranch from index.
		/// </summary>
		/// <param name="index">Index</param>
#endif
		public NodeBranch this[int index]
		{
			get
			{
				return _NodeBranchies[index];
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBranchのインデックスを取得
		/// </summary>
		/// <param name="branch">NodeBranch</param>
		/// <returns>インデックス。ない場合は-1を返す。</returns>
#else
		/// <summary>
		/// Get NodeBranch index.
		/// </summary>
		/// <param name="branch">NodeBranch</param>
		/// <returns>Index. If not, it returns -1.</returns>
#endif
		public int IndexOf(NodeBranch branch)
		{
			return _NodeBranchies.IndexOf(branch);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 使用されていないNodeBranchのIDを取得
		/// </summary>
		/// <returns>一意のBranchIDを返す。</returns>
#else
		/// <summary>
		/// Get the ID of NodeBranch that is not being used
		/// </summary>
		/// <returns>Returns a unique BranchID.</returns>
#endif
		public int GetUniqueBranchID()
		{
			int count = _NodeBranchies.Count;

			System.Random random = new System.Random(count);

			while (true)
			{
				int branchID = random.Next();

				if (branchID != 0 && GetFromID(branchID) == null)
				{
					return branchID;
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ブランチIDを指定して<see cref="Arbor.BehaviourTree.NodeBranch" />を取得する。
		/// </summary>
		/// <param name="branchID">ブランチID</param>
		/// <returns>見つかった<see cref="Arbor.BehaviourTree.NodeBranch" />。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Gets <see cref="Arbor.BehaviourTree.NodeBranch" /> from the branch identifier.
		/// </summary>
		/// <param name="branchID">The branch identifier.</param>
		/// <returns>Found <see cref = "Arbor.BehaviourTree.NodeBranch" />. Returns null if not found.</returns>
#endif
		public NodeBranch GetFromID(int branchID)
		{
			if (branchID == 0)
			{
				return null;
			}

			int branchCount = _NodeBranchies.Count;
			for (int branchIndex = 0; branchIndex < branchCount; branchIndex++)
			{
				NodeBranch branch = _NodeBranchies[branchIndex];
				if (branch.branchID == branchID)
				{
					return branch;
				}
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBranchを追加する。
		/// </summary>
		/// <param name="nodeBranch">追加するNodeBranch。</param>
#else
		/// <summary>
		/// Add a NodeBranch.
		/// </summary>
		/// <param name="nodeBranch">The NodeBranch to be added.</param>
#endif
		public void Add(NodeBranch nodeBranch)
		{
			_NodeBranchies.Add(nodeBranch);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBranchを取り除く。
		/// </summary>
		/// <param name="nodeBranch">取り除くNodeBranch。</param>
		/// <returns>取り除けた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Remove a node.
		/// </summary>
		/// <param name="nodeBranch">The NodeBranch to be removed.</param>
		/// <returns>Returns true if removed.</returns>
#endif
		public bool Remove(NodeBranch nodeBranch)
		{
			return _NodeBranchies.Remove(nodeBranch);
		}
	}
}