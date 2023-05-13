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
	/// TreeNodeBaseの派生クラスが親ノードへの接続スロットを持っていることを定義するインターフェイス
	/// </summary>
#else
	/// <summary>
	/// An interface that defines that a TreeNodeBase derived class has a connection slot to the parent node
	/// </summary>
#endif
	public interface IParentLinkSlotHolder
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// ParentLinkSlotを取得する。
		/// </summary>
		/// <returns>ParentLinkSlotを返す。</returns>
#else
		/// <summary>
		/// Get ParentLinkSlot.
		/// </summary>
		/// <returns>Returns a ParentLinkSlot.</returns>
#endif
		ParentLinkSlot GetParentLinkSlot();

#if ARBOR_DOC_JA
		/// <summary>
		/// 親ノードに接続しているNodeBranchを取得する。
		/// </summary>
		/// <returns>親ノードに接続しているNodeBranchを返す。接続していない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the NodeBranch connected to the parent node.
		/// </summary>
		/// <returns>Returns the NodeBranch connected to the parent node. Returns null if not connected.</returns>
#endif
		NodeBranch GetParentBranch();
	}
}