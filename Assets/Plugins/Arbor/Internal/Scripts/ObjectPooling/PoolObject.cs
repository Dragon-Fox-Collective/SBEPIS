//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Arbor.ObjectPooling
{
	using Arbor.Extensions;

	[AddComponentMenu("")]
	internal sealed class PoolObject : MonoBehaviour
	{
		private const HideFlags kPoolHideFlags = HideFlags.NotEditable;

		[SerializeField]
		[FormerlySerializedAs("original")]
		private Object _Original;

		[SerializeField]
		[FormerlySerializedAs("instance")]
		private Object _Instance;

		[SerializeField]
		private LifeTimeFlags _LifeTimeFlags;

		[SerializeField]
		private float _LifeDuration;

		public Object original
		{
			get
			{
				return _Original;
			}
		}

		public Object instance
		{
			get
			{
				return _Instance;
			}
		}

		public LifeTimeFlags lifeTimeFlags
		{
			get
			{
				return _LifeTimeFlags;
			}
			internal set
			{
				_LifeTimeFlags = value;
			}
		}

		public float lifeDuration
		{
			get
			{
				return _LifeDuration;
			}
			internal set
			{
				_LifeDuration = value;
			}
		}

		public bool isPooled
		{
			get;
			private set;
		}

		internal bool isValid
		{
			get
			{
				return gameObject != null && _Instance != null && _Original != null;
			}
		}

		internal LinkedListNode<PoolObject> node;

		internal void OnPoolResume()
		{
			gameObject.SetActive(true);
			gameObject.hideFlags &= ~kPoolHideFlags;
			hideFlags &= ~HideFlags.NotEditable;

			if (isPooled)
			{
				var rigidbodies = gameObject.GetComponentsInChildrenTemp<Rigidbody>();
				for (int rigidbodyIndex = 0; rigidbodyIndex < rigidbodies.Count; rigidbodyIndex++)
				{
					var rigidbody = rigidbodies[rigidbodyIndex];
					if (rigidbody != null)
					{
						rigidbody.WakeUp();
					}
				}

				var rigidbodies2D = gameObject.GetComponentsInChildrenTemp<Rigidbody2D>();
				for (int rigidbody2DIndex = 0; rigidbody2DIndex < rigidbodies2D.Count; rigidbody2DIndex++)
				{
					var rigidbody2D = rigidbodies2D[rigidbody2DIndex];
					if (rigidbody2D != null)
					{
						rigidbody2D.WakeUp();
					}
				}

				var receivers = gameObject.GetComponentsInChildrenTemp<IPoolCallbackReceiver>();
				for (int receiverIndex = 0; receiverIndex < receivers.Count; receiverIndex++)
				{
					IPoolCallbackReceiver receiver = receivers[receiverIndex];
					if (receiver != null)
					{
						receiver.OnPoolResume();
					}
				}
			}

			isPooled = false;

			SceneManager.sceneUnloaded -= OnSceneUnloaded;
			ObjectPoolSingleton.onUpdateLifeTime -= OnUpdateLifeTime;
		}

		private Scene _Scene;
		private float _SleepStartTime;
		
		internal void OnPoolSleep()
		{
			if (!isPooled)
			{
				var rigidbodies = gameObject.GetComponentsInChildrenTemp<Rigidbody>();
				for (int rigidbodyIndex = 0; rigidbodyIndex < rigidbodies.Count; rigidbodyIndex++)
				{
					var rigidbody = rigidbodies[rigidbodyIndex];
					if (rigidbody != null)
					{
						rigidbody.Sleep();
					}
				}

				var rigidbodies2D = gameObject.GetComponentsInChildrenTemp<Rigidbody2D>();
				for (int rigidbody2DIndex = 0; rigidbody2DIndex < rigidbodies2D.Count; rigidbody2DIndex++)
				{
					var rigidbody2D = rigidbodies2D[rigidbody2DIndex];
					if (rigidbody2D != null)
					{
						rigidbody2D.Sleep();
					}
				}

				var receivers = gameObject.GetComponentsInChildrenTemp<IPoolCallbackReceiver>();
				for (int receiverIndex = 0; receiverIndex < receivers.Count; receiverIndex++)
				{
					IPoolCallbackReceiver receiver = receivers[receiverIndex];
					if (receiver != null)
					{
						receiver.OnPoolSleep();
					}
				}
			}

			isPooled = true;

			_Scene = gameObject.scene;

			transform.SetParent(ObjectPool.transform, true);
			gameObject.hideFlags |= kPoolHideFlags;
			gameObject.SetActive(false);

			hideFlags |= HideFlags.NotEditable;

			if ((_LifeTimeFlags & LifeTimeFlags.SceneUnloaded) != 0)
			{
				SceneManager.sceneUnloaded += OnSceneUnloaded;
			}
			if ((_LifeTimeFlags & LifeTimeFlags.TimeElapsed) != 0)
			{
				ObjectPoolSingleton.onUpdateLifeTime += OnUpdateLifeTime;
			}

			_SleepStartTime = Time.realtimeSinceStartup;
		}

		void DestroyObject()
		{
			Destroy(gameObject);

			if (node != null)
			{
				node.List.Remove(node);
				node = null;
			}

			SceneManager.sceneUnloaded -= OnSceneUnloaded;
			ObjectPoolSingleton.onUpdateLifeTime -= OnUpdateLifeTime;
		}

		void OnSceneUnloaded(Scene scene)
		{
			if (_Scene == scene)
			{
				DestroyObject();
			}
		}

		void OnUpdateLifeTime()
		{
			if ((Time.realtimeSinceStartup - _SleepStartTime) > _LifeDuration)
			{
				DestroyObject();
			}
		}

		private void OnDestroy()
		{
			if (node != null)
			{
				node.List.Remove(node);
				node = null;
			}

			SceneManager.sceneUnloaded -= OnSceneUnloaded;
			ObjectPoolSingleton.onUpdateLifeTime -= OnUpdateLifeTime;
		}

		internal void Initialize(Object original, Object instance, LifeTimeFlags lifeTimeFlags, float lifeDuration)
		{
			this._Original = original;
			this._Instance = instance;
			this._LifeTimeFlags = lifeTimeFlags;
			this._LifeDuration = lifeDuration;
		}
	}
}