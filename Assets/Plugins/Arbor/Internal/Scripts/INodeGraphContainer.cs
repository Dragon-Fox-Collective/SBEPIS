//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// NodeBehaviourがNodeGraphの入れ物である場合に使用するインターフェイス
	/// </summary>
#else
	/// <summary>
	/// Interface to use if NodeBehaviour is a NodeGraph container
	/// </summary>
#endif
	[Internal.DocumentManual("/manual/scripting/behaviourinterface/INodeGraphContainer.md")]
	public interface INodeGraphContainer
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// NodeGraphの個数を取得。
		/// </summary>
		/// <returns>NodeGraphの個数</returns>
#else
		/// <summary>
		/// Get count of NodeGraph.
		/// </summary>
		/// <returns>Count of NodeGraph</returns>
#endif
		int GetNodeGraphCount();

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeGraphを取得する。
		/// </summary>
		/// <typeparam name="T">NodeGraphの型。</typeparam>
		/// <param name="index">インデックス</param>
		/// <returns>NodeGraph</returns>
#else
		/// <summary>
		/// Get NodeGraph.
		/// </summary>
		/// <typeparam name="T">NodeGraph type.</typeparam>
		/// <param name="index">Index</param>
		/// <returns>NodeGraph</returns>
#endif
		T GetNodeGraph<T>(int index) where T : Object;

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeGraphを設定する。
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <param name="graph">NodeGraph</param>
		/// <remarks>指定したインデックスに元からあったNodeGraphの破棄は保証されないため、追加や削除は各NodeBehaviourを参照。</remarks>
#else
		/// <summary>
		/// Set NodeGraph.
		/// </summary>
		/// <param name="index">Index</param>
		/// <param name="graph">NodeGraph</param>
		/// <remarks>Since the destruction of the NodeGraph originally from the specified index is not guaranteed, refer to each NodeBehaviour for addition and deletion.</remarks>
#endif
		void SetNodeGraph(int index, NodeGraph graph);

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数はNodeGraphが完了した時に呼ばれる。
		/// </summary>
		/// <param name="graph">NodeGraph</param>
		/// <param name="success">成功であるかどうか</param>
#else
		/// <summary>
		/// This function is called when NodeGraph finishes.
		/// </summary>
		/// <param name="graph">NodeGraph</param>
		/// <param name="success">Whether it is successful or not</param>
#endif
		void OnFinishNodeGraph(NodeGraph graph, bool success);
	}
}