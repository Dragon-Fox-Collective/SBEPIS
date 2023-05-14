//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.BehaviourTree
{
#if ARBOR_DOC_JA
	/// <summary>
	/// TreeNodeBaseの派生クラスが子ノードへの接続スロットを持っていることを定義するインターフェイス
	/// </summary>
#else
	/// <summary>
	/// An interface that defines that a TreeNodeBase derived class has a connection slot to a child node
	/// </summary>
#endif
	public interface IChildLinkSlotHolder
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 子スロットにNodeBranchを接続する
		/// </summary>
		/// <param name="branchID">接続するNodeBranchのID</param>
#else
		/// <summary>
		/// Connect a NodeBranch to a child slot
		/// </summary>
		/// <param name="branchID">ID of the NodeBranch to connect to</param>
#endif
		void ConnectChildLinkSlot(int branchID);

#if ARBOR_DOC_JA
		/// <summary>
		/// 子ノードからNodeBranchを切断する。
		/// </summary>
		/// <param name="branchID">切断するNodeBranchのID</param>
#else
		/// <summary>
		/// Disconnect the NodeBranch from the child node.
		/// </summary>
		/// <param name="branchID">ID of the NodeBranch to disconnect</param>
#endif
		void DisconnectChildLinkSlot(int branchID);

#if ARBOR_DOC_JA
		/// <summary>
		/// 子ノードの優先順位を計算する。
		/// </summary>
		/// <param name="priority">最初の子の優先度</param>
		/// <returns>終端の次の優先度を返す。</returns>
#else
		/// <summary>
		/// Calculate the priority of child nodes.
		/// </summary>
		/// <param name="priority">First child priority</param>
		/// <returns>Returns the next priority of termination.</returns>
#endif
		int OnCalculateChildPriority(int priority);
	}
}