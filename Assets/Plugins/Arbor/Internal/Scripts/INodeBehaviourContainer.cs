//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// NodeがNodeBehaviourの入れ物である場合に使用するインターフェイス
	/// </summary>
#else
	/// <summary>
	/// Interface to use if Node is a NodeBehaviour container
	/// </summary>
#endif
	public interface INodeBehaviourContainer
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBehaviourの個数を取得。
		/// </summary>
		/// <returns>NodeBehaviourの個数</returns>
#else
		/// <summary>
		/// Get count of NodeBehaviour.
		/// </summary>
		/// <returns>Count of NodeBehaviour</returns>
#endif
		int GetNodeBehaviourCount();

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBehaviourを取得する。
		/// </summary>
		/// <typeparam name="T">NodeBehaviourの型</typeparam>
		/// <param name="index">インデックス</param>
		/// <returns>NodeBehaviour</returns>
#else
		/// <summary>
		/// Get NodeBehaviour
		/// </summary>
		/// <typeparam name="T">NodeBehaviour type</typeparam>
		/// <param name="index">Index</param>
		/// <returns>NodeBehaviour</returns>
#endif
		T GetNodeBehaviour<T>(int index) where T : Object;

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBehaviourを設定する。
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <param name="behaviour">NodeBehaviour</param>
		/// <remarks>指定したインデックスに元からあったNodeBehaviourの破棄は保証されないため、追加や削除は各ノードを参照。</remarks>
#else
		/// <summary>
		/// Set NodeBehaviour
		/// </summary>
		/// <param name="index">Index</param>
		/// <param name="behaviour">NodeBehaviour</param>
		/// <remarks>Since destruction of the NodeBehavior which was originally in the specified index is not guaranteed, refer to each node for addition and deletion.</remarks>
#endif
		void SetNodeBehaviour(int index, NodeBehaviour behaviour);
	}
}