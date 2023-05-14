//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ノードのリスト
	/// </summary>
	/// <typeparam name="T">ノードの型</typeparam>
#else
	/// <summary>
	/// Node list
	/// </summary>
	/// <typeparam name="T">Node type</typeparam>
#endif
	[System.Serializable]
	public class NodeList<T> : ISerializationCallbackReceiver where T : Node
	{
		#region Serialize fields

		[SerializeField]
#if !ARBOR_DEBUG
		[HideInInspector]
#endif
		private List<T> _Nodes = new List<T>();

		#endregion // Serialize fields

		[System.NonSerialized]
		private Dictionary<int, T> _DicNodes = new Dictionary<int, T>();

#if ARBOR_DOC_JA
		/// <summary>
		/// Nodeの数を取得。
		/// </summary>
#else
		/// <summary>
		///  Get a count of Node.
		/// </summary>
#endif
		public int count
		{
			get
			{
				return _Nodes.Count;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Nodeをインデックスから取得
		/// </summary>
		/// <param name="index">インデックス</param>
#else
		/// <summary>
		/// Get Node from index.
		/// </summary>
		/// <param name="index">Index</param>
#endif
		public T this[int index]
		{
			get
			{
				return _Nodes[index];
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Nodeのインデックスを取得
		/// </summary>
		/// <param name="node">Node</param>
		/// <returns>インデックス。ない場合は-1を返す。</returns>
#else
		/// <summary>
		/// Get Node index.
		/// </summary>
		/// <param name="node">Node</param>
		/// <returns>Index. If not, it returns -1.</returns>
#endif
		public int IndexOf(T node)
		{
			return _Nodes.IndexOf(node);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードIDを指定してノードを取得する。
		/// </summary>
		/// <param name="nodeID">ノードID</param>
		/// <returns>見つかったノード。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Gets Node from the node identifier.
		/// </summary>
		/// <param name="nodeID">The node identifier.</param>
		/// <returns>Found Node. Returns null if not found.</returns>
#endif
		public T GetFromID(int nodeID)
		{
			T result = null;
			if (_DicNodes.TryGetValue(nodeID, out result))
			{
				return result;
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードを追加する。
		/// </summary>
		/// <param name="node">追加するノード。</param>
#else
		/// <summary>
		/// Add a node.
		/// </summary>
		/// <param name="node">The node to be added.</param>
#endif
		public void Add(T node)
		{
			_Nodes.Add(node);
			_DicNodes.Add(node.nodeID, node);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードを削除する。
		/// </summary>
		/// <param name="node">削除するノード。</param>
		/// <returns>削除できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Remove a node.
		/// </summary>
		/// <param name="node">The node to be removed.</param>
		/// <returns>Returns true if removed.</returns>
#endif
		public bool Remove(T node)
		{
			_DicNodes.Remove(node.nodeID);
			return _Nodes.Remove(node);
		}

		#region ISerializationCallbackReceiver implementation

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			_DicNodes.Clear();

			int nodeCount = _Nodes.Count;
			for (int nodeIndex = 0; nodeIndex < nodeCount; nodeIndex++)
			{
				T node = _Nodes[nodeIndex];
				_DicNodes.Add(node.nodeID, node);
			}
		}

		#endregion
	}
}