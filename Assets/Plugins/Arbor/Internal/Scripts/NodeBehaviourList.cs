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
	/// NodeBehaviourのリスト
	/// </summary>
	/// <typeparam name="T">NodeBehaviourの型</typeparam>
#else
	/// <summary>
	/// NodeBehaviour list
	/// </summary>
	/// <typeparam name="T">NodeBehaviour type</typeparam>
#endif
	[System.Serializable]
	public class NodeBehaviourList<T> where T : NodeBehaviour
	{
		#region Serialize fields

		[SerializeField]
#if !ARBOR_DEBUG
		[HideInInspector]
#endif
		internal List<Object> _Objects = new List<Object>();

		#endregion // Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 個数
		/// </summary>
#else
		/// <summary>
		/// Count
		/// </summary>
#endif
		public int count
		{
			get
			{
				return _Objects.Count;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定したインデックスへのアクセス
		/// </summary>
		/// <param name="i">インデックス</param>
		/// <returns>インデックスに格納されているNodeBehaviour</returns>
#else
		/// <summary>
		/// Access to specified index
		/// </summary>
		/// <param name="i">Index</param>
		/// <returns>NodeBehaviour stored in the index</returns>
#endif
		public T this[int i]
		{
			get
			{
				return _Objects[i] as T;
			}
			set
			{
				_Objects[i] = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Behaviourを追加。
		/// </summary>
		/// <param name="behaviour">追加するNodeBehaviour</param>
#else
		/// <summary>
		/// Adds the Behaviour.
		/// </summary>
		/// <param name="behaviour">Add NodeBehaviour</param>
#endif
		public void Add(T behaviour)
		{
			ComponentUtility.RecordObject(behaviour.nodeGraph, "Add Behaviour");

			_Objects.Add(behaviour);

			ComponentUtility.SetDirty(behaviour.nodeGraph);

			behaviour.OnAttachToNode();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Behaviourを挿入。
		/// </summary>
		/// <param name="index">挿入先インデックス</param>
		/// <param name="behaviour">挿入するNodeBehaviour</param>
#else
		/// <summary>
		/// Insert the Behaviour.
		/// </summary>
		/// <param name="index">Insertion destination index</param>
		/// <param name="behaviour">Insert NodeBehaviour</param>
#endif
		public void Insert(int index, T behaviour)
		{
			ComponentUtility.RecordObject(behaviour.nodeGraph, "Insert Behaviour");

			_Objects.Insert(index, behaviour);

			ComponentUtility.SetDirty(behaviour.nodeGraph);

			behaviour.OnAttachToNode();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBehaviourのObjectをindexから取得。
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <returns>Object</returns>
#else
		/// <summary>
		/// Get Object of NodeBehaviour from index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>Object</returns>
#endif
		public Object GetObject(int index)
		{
			return _Objects[index];
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBehaviourのObjectをindexへ設定。
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <param name="obj">Object</param>
#else
		/// <summary>
		/// Set Object of NodeBehaviour to index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <param name="obj">Object</param>
#endif
		public void SetObject(int index, Object obj)
		{
			_Objects[index] = obj;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBehaviourのインデックスを返す。
		/// </summary>
		/// <param name="obj">検索するNodeBehaviour</param>
		/// <returns>見つかった場合はインデックス、ない場合は-1を返す。</returns>
#else
		/// <summary>
		/// Return index of NodeBehaviour.
		/// </summary>
		/// <param name="obj">The NodeBehaviour to locate in the State.</param>
		/// <returns>Returns an index if found, -1 otherwise.</returns>
#endif
		public int IndexOf(Object obj)
		{
			return _Objects.IndexOf(obj);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBehaviourの順番を入れ替える。
		/// </summary>
		/// <param name="fromIndex">入れ替えたいインデックス。</param>
		/// <param name="toIndex">入れ替え先インデックス。</param>
#else
		/// <summary>
		/// Swap the order of NodeBehaviour.
		/// </summary>
		/// <param name="fromIndex">The swapping want index.</param>
		/// <param name="toIndex">Exchange destination index.</param>
#endif
		public void Swap(int fromIndex, int toIndex)
		{
			Object decorator = _Objects[toIndex];
			_Objects[toIndex] = _Objects[fromIndex];
			_Objects[fromIndex] = decorator;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBehaviourの順番を移動する。
		/// </summary>
		/// <param name="fromIndex">移動させたいインデックス。</param>
		/// <param name="toIndex">移動先のインデックス。</param>
#else
		/// <summary>
		/// Move the order of NodeBehaviour.
		/// </summary>
		/// <param name="fromIndex">The moving want index.</param>
		/// <param name="toIndex">The destination index.</param>
#endif
		public void Move(int fromIndex, int toIndex)
		{
			Object decorator = _Objects[fromIndex];
			_Objects.RemoveAt(fromIndex);
			_Objects.Insert(toIndex, decorator);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBehaviourを別のノードへ移動する。
		/// </summary>
		/// <param name="fromIndex">移動元インデックス</param>
		/// <param name="toNode">移動先ノード</param>
		/// <param name="toList">移動先リスト</param>
		/// <param name="toIndex">移動先インデックス</param>
#else
		/// <summary>
		/// Move NodeBehaviour to another node.
		/// </summary>
		/// <param name="fromIndex">Source index</param>
		/// <param name="toNode">Destination node</param>
		/// <param name="toList">Destination list</param>
		/// <param name="toIndex">Destination index</param>
#endif
		public void Move(int fromIndex, Node toNode, NodeBehaviourList<T> toList, int toIndex)
		{
			Object behaviour = _Objects[fromIndex];

			T nodeBehaviour = behaviour as T;
			if (nodeBehaviour != null)
			{
				_Objects.RemoveAt(fromIndex);

				ComponentUtility.RecordObject(nodeBehaviour, "Move Behaviour");

				nodeBehaviour.Initialize(toNode.nodeGraph, toNode.nodeID);

				ComponentUtility.SetDirty(nodeBehaviour);

				toList._Objects.Insert(toIndex, behaviour);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBehaviourを破棄
		/// </summary>
		/// <param name="node">NodeBehaviourを持っているNode</param>
		/// <param name="behaviour">NodeBehaviourのObject</param>
#else
		/// <summary>
		/// Destroy NodeBehaviour
		/// </summary>
		/// <param name="node">Node that has NodeBehaviour</param>
		/// <param name="behaviour">Object of NodeBehaviour</param>
#endif
		public void Destroy(Node node, Object behaviour)
		{
			int behaviourIndex = IndexOf(behaviour);

			Destroy(node, behaviourIndex);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBehaviourを破棄
		/// </summary>
		/// <param name="node">NodeBehaviourを持っているNode</param>
		/// <param name="behaviourIndex">破棄するインデックス</param>
#else
		/// <summary>
		/// Destroy NodeBehaviour
		/// </summary>
		/// <param name="node">Node that has NodeBehaviour</param>
		/// <param name="behaviourIndex">The index to discard</param>
#endif
		public void Destroy(Node node, int behaviourIndex)
		{
			if (!(0 <= behaviourIndex && behaviourIndex < _Objects.Count))
			{
				return;
			}

			Object behaviour = _Objects[behaviourIndex];

			DestroyBehaviour(node, behaviour);

			ComponentUtility.RecordObject(node.nodeGraph, "Remove Behaviour");
			_Objects.RemoveAt(behaviourIndex);
			ComponentUtility.SetDirty(node.nodeGraph);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 全てのNodeBehaviourを破棄。
		/// </summary>
		/// <param name="node">NodeBehaviourを持っているNode</param>
#else
		/// <summary>
		/// Destroy All NodeBehaviour.
		/// </summary>
		/// <param name="node">Node that has NodeBehaviour</param>
#endif
		public void DestroyAll(Node node)
		{
			int count = _Objects.Count;
			for (int index = count - 1; index >= 0; --index)
			{
				Destroy(node, _Objects[index]);
			}
		}

		void DestroyBehaviour(Node node, Object behaviour)
		{
			if ((object)behaviour == null)
			{
				return;
			}
			node.nodeGraph.DisconnectDataBranch(behaviour);

			NodeBehaviour nodeBehaviour = behaviour as NodeBehaviour;
			if (nodeBehaviour != null)
			{
				NodeBehaviour.Destroy(nodeBehaviour);
			}
			else if (behaviour != null)
			{
				ComponentUtility.Destroy(behaviour);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBehaviourをnodeに移動する。Editorでのみ有効。
		/// </summary>
		/// <param name="node">移動先ノード</param>
#else
		/// <summary>
		/// Move NodeBehaviour to node. Valid only in Editor.
		/// </summary>
		/// <param name="node">Moving destination node</param>
#endif
		public void MoveBehaviour(Node node)
		{
			Object[] sourceBehaviours = _Objects.ToArray();
			_Objects.Clear();

			for (int objIndex = 0; objIndex < sourceBehaviours.Length; objIndex++)
			{
				Object obj = sourceBehaviours[objIndex];
				NodeBehaviour sourceBehaviour = obj as NodeBehaviour;
				if (sourceBehaviour != null)
				{
					ComponentUtility.MoveBehaviour(node, sourceBehaviour);
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// behaviourを含んでいるかを返す。
		/// </summary>
		/// <param name="behaviour">チェックする対象</param>
		/// <returns>含んでいる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether it contains behaviour.
		/// </summary>
		/// <param name="behaviour">Check target</param>
		/// <returns>Returns true if it contains.</returns>
#endif
		public bool Contains(T behaviour)
		{
			return ContainsObject(behaviour);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// objを含んでいるかを返す。
		/// </summary>
		/// <param name="obj">チェックする対象</param>
		/// <returns>含んでいる場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Returns whether it contains obj.
		/// </summary>
		/// <param name="obj">Check target</param>
		/// <returns>Returns true if it contains.</returns>
#endif
		public bool ContainsObject(Object obj)
		{
			int count = _Objects.Count;
			for (int index = 0; index < count; index++)
			{
				if (_Objects[index] == obj)
				{
					return true;
				}
			}

			return false;
		}
	}
}