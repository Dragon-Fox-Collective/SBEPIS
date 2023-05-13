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
	[System.Serializable]
	internal sealed class BehaviourInfoCache
	{
		public Object behaviour;
		public bool expanded;
	}

	public sealed class BehaviourEditorUtility : ScriptableSingleton<BehaviourEditorUtility>, ISerializationCallbackReceiver
	{
		public static bool GetExpanded(Object behaviour, bool defaultExpanded)
		{
			return instance.GetExpandedInternal(behaviour, defaultExpanded);
		}

		public static void SetExpanded(Object behaviour, bool expanded)
		{
			instance.SetExpandedInternal(behaviour, expanded);
		}

		[SerializeField]
		private List<BehaviourInfoCache> _Caches = new List<BehaviourInfoCache>();

		private Dictionary<Object, BehaviourInfoCache> _DicCaches = new Dictionary<Object, BehaviourInfoCache>();

		bool GetExpandedInternal(Object behaviour, bool defaultExpanded)
		{
			BehaviourInfoCache cache = null;
			if (behaviour != null && _DicCaches.TryGetValue(behaviour, out cache) && cache != null && cache.behaviour != null)
			{
				return cache.expanded;
			}
			return defaultExpanded;
		}

		void SetExpandedInternal(Object behaviour, bool expanded)
		{
			if (behaviour == null)
			{
				return;
			}

			BehaviourInfoCache cache = null;
			if (!_DicCaches.TryGetValue(behaviour, out cache))
			{
				cache = new BehaviourInfoCache();
				cache.behaviour = behaviour;

				_DicCaches.Add(behaviour, cache);
			}

			cache.expanded = expanded;
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			_Caches.Clear();

			var values = _DicCaches.Values;
			for (int i = 0, count = values.Count; i < count; i++)
			{
				var cache = values.ElementAt(i);
				if (cache.behaviour != null)
				{
					_Caches.Add(cache);
				}
			}
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			_DicCaches.Clear();

			for (int i = 0, count = _Caches.Count; i < count; i++)
			{
				var cache = _Caches[i];
				if (cache.behaviour != null)
				{
					_DicCaches.Add(cache.behaviour, cache);
				}
			}
		}
	}
}