//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Arbor.ObjectPooling
{
	[AddComponentMenu("")]
	internal sealed class ObjectPoolSingleton : ComponentSingleton<ObjectPoolSingleton>
	{
		private static class ResolutionUtility
		{
#if ARBOR_DLL
			private static readonly System.Reflection.PropertyInfo s_RefreshRateRatioProperty;
			private static readonly System.Reflection.PropertyInfo s_ValueProperty;
			private static readonly System.Reflection.PropertyInfo s_RefreshRateProperty;

			static ResolutionUtility()
			{
				var typeResolution = typeof(Resolution);
				s_RefreshRateRatioProperty = typeResolution.GetProperty("refreshRateRatio", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
				if (s_RefreshRateRatioProperty != null)
				{
					s_ValueProperty = s_RefreshRateRatioProperty.PropertyType.GetProperty("value", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
				}

				s_RefreshRateProperty = typeResolution.GetProperty("refreshRate", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
			}
#endif
			public static double GetRefreshRate()
			{
				Resolution currentResolution = Screen.currentResolution;
#if ARBOR_DLL
				if (s_RefreshRateRatioProperty != null)
				{
					object refreshRateRatio = s_RefreshRateRatioProperty.GetValue(currentResolution);
					return (double)s_ValueProperty.GetValue(refreshRateRatio);
				}
				else if (s_RefreshRateProperty != null)
				{
					return (int)s_RefreshRateProperty.GetValue(currentResolution);
				}
				return 60.0; // dummy
#elif UNITY_2022_2_OR_NEWER
				return currentResolution.refreshRateRatio.value;
#else
				return currentResolution.refreshRate;
#endif
			}
		}

		public static event System.Action onUpdateLifeTime;

		private Dictionary<Object, PoolQueue> _Pool = new Dictionary<Object, PoolQueue>();

		private Queue<PoolingItem> _PoolingItems = new Queue<PoolingItem>();

		private Coroutine _Coroutine;

		public bool isReady
		{
			get
			{
				return _PoolingItems.Count == 0;
			}
		}

		protected sealed override string GetGameObjectName()
		{
			return "ObjectPool";
		}

		private float _Time;

		private void Start()
		{
			_Time = Time.realtimeSinceStartup;
		}

		private void Update()
		{
			onUpdateLifeTime?.Invoke();
		}

		public PoolQueue GetPool(Object original)
		{
			PoolQueue pool = null;
			if (!_Pool.TryGetValue(original, out pool))
			{
				pool = new PoolQueue();
				_Pool.Add(original, pool);
			}

			return pool;
		}

		public void ClearPools()
		{
			_Pool.Clear();
		}

		public void AddItems(IList<PoolingItem> items)
		{
			for (int itemIndex = 0; itemIndex < items.Count; itemIndex++)
			{
				PoolingItem item = items[itemIndex];
				_PoolingItems.Enqueue(new PoolingItem(item));
			}

			if (_Coroutine == null)
			{
				_Coroutine = StartCoroutine(Load());
			}
		}

		public void AddItems(IEnumerable<PoolingItem> items)
		{
			foreach (PoolingItem item in items)
			{
				_PoolingItems.Enqueue(new PoolingItem(item));
			}

			if (_Coroutine == null)
			{
				_Coroutine = StartCoroutine(Load());
			}
		}

		static float GetMaxTimePerFrame()
		{
			int frameRate = 0;

			if (ObjectPool.advancedRatePerFrame > 0)
			{
				int vSyncCount = QualitySettings.vSyncCount;
				if (Application.isEditor || vSyncCount > 0)
				{
					vSyncCount = (vSyncCount > 0 ? vSyncCount : 1);
					frameRate = (int)(ResolutionUtility.GetRefreshRate() / vSyncCount);
				}
				else
				{
					frameRate = Application.targetFrameRate;
				}

				frameRate *= ObjectPool.advancedRatePerFrame;
			}
			else
			{
				frameRate = ObjectPool.advancedFrameRate;
			}

			if (frameRate > 0)
			{
				return (1f / frameRate);
			}

			return 0f;
		}

		private IEnumerator Load()
		{
			Timer timer = new Timer();
			timer.timeType = TimeType.Realtime;
			timer.Start();

			while (_PoolingItems.Count > 0)
			{
				PoolingItem item = _PoolingItems.Peek();

				if (item.amount > 0)
				{
					ObjectPool.CreatePool(item.original, item.lifeTimeFlags, item.lifeDuration);
					item.amount--;
				}

				if (item.amount == 0)
				{
					_PoolingItems.Dequeue();
				}

				float maxTime = GetMaxTimePerFrame();
				if (maxTime > 0f && timer.elapsedTime >= maxTime)
				{
					yield return null;
					timer.Stop();
					timer.Start();
				}
			}

			_Coroutine = null;
		}
	}
}