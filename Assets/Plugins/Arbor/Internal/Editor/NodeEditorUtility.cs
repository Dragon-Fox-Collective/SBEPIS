//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace ArborEditor
{
	using Arbor;

	[System.Serializable]
	struct NodeKey : System.IEquatable<NodeKey>
	{
		public NodeGraph graph;
		public int nodeID;

		private int _HashCode;

		public NodeKey(Node node)
		{
			graph = node.nodeGraph;
			nodeID = node.nodeID;

			_HashCode = ((graph != null) ? graph.GetHashCode() : 0) ^ nodeID;
		}

		public override int GetHashCode()
		{
			return _HashCode;
		}

		bool System.IEquatable<NodeKey>.Equals(NodeKey other)
		{
			if (graph == null || other.graph == null)
			{
				return false;
			}

			return graph == other.graph && nodeID == other.nodeID;
		}
	}

	[System.Serializable]
	internal sealed class NodeInfoCache
	{
		public NodeKey nodeKey;
		public bool showComment;
	}

	internal sealed class NodeEditorUtility : ScriptableSingleton<NodeEditorUtility>, ISerializationCallbackReceiver
	{
		public static bool GetShowComment(Node node)
		{
			return instance.GetShowCommentInternal(node);
		}

		public static void SetShowComment(Node node, bool showComment)
		{
			instance.SetShowCommentInternal(node, showComment);
		}

		public static void DeleteShowComment(Node node)
		{
			instance.DeleteShowCommentInternal(node);
		}

		[SerializeField]
		private List<NodeInfoCache> _Caches = new List<NodeInfoCache>();

		private Dictionary<NodeKey, NodeInfoCache> _DicCaches = new Dictionary<NodeKey, NodeInfoCache>();

		bool GetShowCommentInternal(Node node)
		{
			NodeInfoCache cache = null;
			NodeKey nodeKey = new NodeKey(node);
			if (_DicCaches.TryGetValue(nodeKey, out cache))
			{
				return cache.showComment;
			}
			return node.showComment;
		}

		void SetShowCommentInternal(Node node, bool showComment)
		{
			NodeInfoCache cache = null;
			NodeKey nodeKey = new NodeKey(node);
			if (!_DicCaches.TryGetValue(nodeKey, out cache))
			{
				cache = new NodeInfoCache();
				cache.nodeKey = nodeKey;

				_DicCaches.Add(nodeKey, cache);
			}

			Undo.RecordObject(this, "Change Show Comment");

			cache.showComment = showComment;

			EditorUtility.SetDirty(this);
		}

		void DeleteShowCommentInternal(Node node)
		{
			NodeKey nodeKey = new NodeKey(node);
			_DicCaches.Remove(nodeKey);
		}
		
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			_DicCaches.Clear();

			for (int i = 0, count = _Caches.Count; i < count; i++)
			{
				var cache = _Caches[i];
				if (cache.nodeKey.graph != null)
				{
					_DicCaches.Add(cache.nodeKey, cache);
				}
			}
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			_Caches.Clear();

			var values = _DicCaches.Values;
			for (int i = 0, count = values.Count; i < count; i++)
			{
				var cache = values.ElementAt(i);
				if (cache.nodeKey.graph != null)
				{
					_Caches.Add(cache);
				}
			}
		}
	}
}