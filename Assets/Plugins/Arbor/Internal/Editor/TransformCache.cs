//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace ArborEditor
{
	[System.Serializable]
	internal sealed class TransformCache : ScriptableSingleton<TransformCache>
	{
		[System.Serializable]
		class GlobalTransformDictionary : SerializableDictionary<string, TransformData>
		{
		}

		[System.Serializable]
		class LocalTransformDictionary : SerializableDictionary<int, TransformData>
		{

		}

		[SerializeField]
		private GlobalTransformDictionary _GlobalTransforms = new GlobalTransformDictionary();

		[SerializeField]
		private LocalTransformDictionary _LocalTransforms = new LocalTransformDictionary();

		private void OnEnable()
		{
			EditorSceneManager.sceneSaved += OnSceneSaved;
		}

		private void OnDisable()
		{
			EditorSceneManager.sceneSaved -= OnSceneSaved;
		}

		void OnSceneSaved(Scene scene)
		{
			foreach (var pair in _LocalTransforms)
			{
				var obj =  EditorUtility.InstanceIDToObject(pair.Key);
				if (obj == null)
				{
					continue;
				}

				var globalObjectId = GlobalObjectId.GetGlobalObjectIdSlow(obj);
				if (globalObjectId.assetGUID.Empty())
				{
					continue;
				}

				var key = globalObjectId.ToString();
				_GlobalTransforms[key] = pair.Value;
			}

			_LocalTransforms.Clear();
		}

		public bool TryGet(Object obj, out TransformData transform)
		{
			if (obj != null)
			{
				var globalObjectId = GlobalObjectId.GetGlobalObjectIdSlow(obj);

				if (globalObjectId.assetGUID.Empty())
				{
					return _LocalTransforms.TryGetValue(obj.GetInstanceID(), out transform);
				}
				else
				{
					var key = globalObjectId.ToString();

					return _GlobalTransforms.TryGetValue(key, out transform);
				}
			}

			transform = default;
			return false;
		}

		public void Set(Object obj, TransformData transform)
		{
			if (obj == null)
			{
				return;
			}

			var globalObjectId = GlobalObjectId.GetGlobalObjectIdSlow(obj);

			if (globalObjectId.assetGUID.Empty())
			{
				int instanceID = obj.GetInstanceID();
				_LocalTransforms[instanceID] = transform;
			}
			else
			{
				var key = globalObjectId.ToString();
				_GlobalTransforms[key] = transform;
			}
		}
	}
}