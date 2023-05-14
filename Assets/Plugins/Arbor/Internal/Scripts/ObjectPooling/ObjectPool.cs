//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Arbor.PlayerLoop;

namespace Arbor.ObjectPooling
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ObjectPoolの管理クラス
	/// </summary>
#else
	/// <summary>
	/// ObjectPool management class
	/// </summary>
#endif
	public static class ObjectPool
	{
		private const int kDefaultAdvancedRatePerFrame = 10;
		private const int kDefaultAdvancedFrameRate = -1;

		private static int s_AdvancedRatePerFrame = kDefaultAdvancedRatePerFrame;
		private static int s_AdvancedFrameRate = kDefaultAdvancedFrameRate;

#if ARBOR_DOC_JA
		/// <summary>
		/// 事前プールの処理フレームレート(画面のリフレッシュレートに対する倍率)
		/// </summary>
		/// <remarks>
		/// このフレームレートを超えて処理時間がかかった場合は次フレームまで待機する。<br/>
		/// デフォルト値は10。<br/>
		/// 0以下を指定した場合は<see cref="advancedFrameRate"/>を使用する。
		/// </remarks>
#else
		/// <summary>
		/// Advanced Pooling processing frame rate (multiplication factor relative to the screen refresh rate)
		/// </summary>
		/// <remarks>
		/// If processing time is exceeded beyond this frame rate, it waits until the next frame.<br/>
		/// The default value is 10.<br/>
		/// If 0 or less is specified, use <see cref="advancedFrameRate"/>.
		/// </remarks>
#endif
		public static int advancedRatePerFrame
		{
			get
			{
				return s_AdvancedRatePerFrame;
			}
			set
			{
				s_AdvancedRatePerFrame = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 事前プールの処理フレームレート
		/// </summary>
		/// <remarks>
		/// このフレームレートを超えて処理時間がかかった場合は次フレームまで待機する。<br/>
		/// advancedFrameRateと<see cref="advancedRatePerFrame"/>の両方に0以下を指定した場合は、全てのプールが完了するまで待機しない<br/>
		/// デフォルト値は0。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Advanced Pooling processing frame rate
		/// </summary>
		/// <remarks>
		/// If processing time is exceeded beyond this frame rate, it waits until the next frame.<br/>
		/// When 0 or less is specified for both advancedFrameRate and <see cref="advancedRatePerFrame"/>, do not wait until all pools are completed.<br/>
		/// The default value is 0.<br/>
		/// </remarks>
#endif
		public static int advancedFrameRate
		{
			get
			{
				return s_AdvancedFrameRate;
			}
			set
			{
				s_AdvancedFrameRate = value;
			}
		}

		internal static GameObject gameObject
		{
			get
			{
				return ObjectPoolSingleton.instance.gameObject;
			}
		}

		internal static Transform transform
		{
			get
			{
				return ObjectPoolSingleton.instance.transform;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 事前プールが完了しているかを返す。
		/// </summary>
#else
		/// <summary>
		/// Returns whether Advanced pooling is completed.
		/// </summary>
#endif
		public static bool isReady
		{
			get
			{
				return ObjectPoolSingleton.instance.isReady;
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void OnSubsystemRegistration()
		{
			s_AdvancedRatePerFrame = kDefaultAdvancedRatePerFrame;
			s_AdvancedFrameRate = kDefaultAdvancedFrameRate;
			if (ObjectPoolSingleton.exists)
			{
				ObjectPoolSingleton.instance.ClearPools();
			}
		}

		private static void CheckOriginalArgument(Object original)
		{
			if (original == null)
				throw new System.ArgumentException("The Object you want to instantiate is null.");

			if (!(original is GameObject || original is Component))
				throw new System.ArgumentException($"Cannot instantiate a Asset Object : {original.GetType().Name}");
		}

		static PoolQueue GetPool(Object original)
		{
			return ObjectPoolSingleton.instance.GetPool(original);
		}

		private static Transform GetTransform(Object original)
		{
			Transform transform = original as Transform;
			if (transform != null)
			{
				return transform;
			}

			Component component = original as Component;
			if (component != null)
			{
				return component.transform;
			}

			GameObject gameObject = original as GameObject;
			if (gameObject != null)
			{
				return gameObject.transform;
			}

			return null;
		}

		static PoolObject CreatePoolObject(Object original, LifeTimeFlags lifeTimeFlags, float lifeDuration)
		{
			Object instance = ComponentUtility.InstantiatePrefab(original);

			GameObject gameObject = instance as GameObject;
			if (gameObject == null)
			{
				Component component = instance as Component;
				if (component != null)
				{
					gameObject = component.gameObject;
				}
			}

			PoolObject poolObject = null;
			if (!gameObject.TryGetComponent<PoolObject>(out poolObject))
			{
				poolObject = gameObject.AddComponent<PoolObject>();
			}
			poolObject.Initialize(original, instance, lifeTimeFlags, lifeDuration);

			return poolObject;
		}

		static Object CreateInstante(Object original, LifeTimeFlags lifeTimeFlags, float lifeDuration)
		{
			PoolObject poolObject = CreatePoolObject(original, lifeTimeFlags, lifeDuration);

			return poolObject.instance;
		}

		internal static void CreatePool(Object original, LifeTimeFlags lifeTimeFlags, float lifeDuration)
		{
			PoolObject poolObject = CreatePoolObject(original, lifeTimeFlags, lifeDuration);

			poolObject.OnPoolSleep();

			PoolQueue poolQueue = GetPool(original);
			poolQueue.Enqueue(poolObject);		
		}

		private static PoolObject GetPoolObject(Object original, LifeTimeFlags lifeTimeFlags, float lifeDuration)
		{
			PoolQueue poolQueue = GetPool(original);

			while (poolQueue.Count > 0)
			{
				PoolObject poolObject = poolQueue.Dequeue();
				if (poolObject == null || !poolObject.isValid)
				{
					continue;
				}

				poolObject.lifeTimeFlags = lifeTimeFlags;
				poolObject.lifeDuration = lifeDuration;

				return poolObject;
			}

			return null;
		}

		private static Object DoInstantiate(Object original, Transform parent, bool worldPositionStays, LifeTimeFlags lifeTimeFlags, float lifeDuration)
		{
			PoolObject poolObject = GetPoolObject(original, lifeTimeFlags, lifeDuration);

			Object instance = poolObject?.instance ?? CreateInstante(original, lifeTimeFlags, lifeDuration);

			Transform transform = GetTransform(instance);
			if (transform != null)
			{
				transform.SetParent(parent, worldPositionStays);
				if (parent == null)
				{
					SceneManager.MoveGameObjectToScene(transform.gameObject, SceneManager.GetActiveScene());
				}
			}

			if (poolObject != null)
			{
				poolObject.OnPoolResume();
			}

			return instance;
		}

		private static Object DoInstantiate(Object original, Transform parent, Vector3 pos, Quaternion rot, Space space, LifeTimeFlags lifeTimeFlags, float lifeDuration)
		{
			PoolObject poolObject = GetPoolObject(original, lifeTimeFlags, lifeDuration);

			Object instance = poolObject?.instance ?? CreateInstante(original, lifeTimeFlags, lifeDuration);

			Transform transform = GetTransform(instance);
			if (transform != null)
			{
				transform.SetParent(parent, false);
				if (parent == null)
				{
					SceneManager.MoveGameObjectToScene(transform.gameObject, SceneManager.GetActiveScene());
				}
				switch (space)
				{
					case Space.World:
						transform.position = pos;
						transform.rotation = rot;
						break;
					case Space.Self:
						transform.localPosition = pos;
						transform.localRotation = rot;
						break;
				}
			}

			if (poolObject != null)
			{
				poolObject.OnPoolResume();
			}

			return instance;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 事前プールする。
		/// </summary>
		/// <param name="items">プールするオブジェクトリスト</param>
#else
		/// <summary>
		/// Pool in advance.
		/// </summary>
		/// <param name="items">List of objects to pool</param>
#endif
		public static void AdvancedPool(IList<PoolingItem> items)
		{
			ObjectPoolSingleton.instance.AddItems(items);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 事前プールする。
		/// </summary>
		/// <param name="items">プールするオブジェクトリスト</param>
#else
		/// <summary>
		/// Pool in advance.
		/// </summary>
		/// <param name="items">List of objects to pool</param>
#endif
		public static void AdvancedPool(IEnumerable<PoolingItem> items)
		{
			ObjectPoolSingleton.instance.AddItems(items);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをインスタンス化する。
		/// </summary>
		/// <param name="original">オリジナルのオブジェクト</param>
		/// <param name="position">位置</param>
		/// <param name="rotation">回転</param>
		/// <returns>インスタンス化されたオブジェクト</returns>
		/// <remarks>
		/// プールされているオブジェクトがある場合はそのオブジェクトを再開させる。<br/>
		/// プールがない場合はObject.Instantiateにより新たにインスタンス化する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Instantiate an object.
		/// </summary>
		/// <param name="original">Original object</param>
		/// <param name="position">Position</param>
		/// <param name="rotation">Rotation</param>
		/// <returns>Instantiated object</returns>
		/// <remarks>
		/// If there is a pooled object, resume that object.<br/>
		/// If there is no pool, it is newly instantiated by Object.Instantiate.
		/// </remarks>
#endif
		public static Object Instantiate(Object original, Vector3 position, Quaternion rotation)
		{
			return Instantiate(original, position, rotation, null);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをインスタンス化する。
		/// </summary>
		/// <param name="original">オリジナルのオブジェクト</param>
		/// <param name="position">位置</param>
		/// <param name="rotation">回転</param>
		/// <param name="parent">親トランスフォーム</param>
		/// <returns>インスタンス化されたオブジェクト</returns>
		/// <remarks>
		/// プールされているオブジェクトがある場合はそのオブジェクトを再開させる。<br/>
		/// プールがない場合はObject.Instantiateにより新たにインスタンス化する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Instantiate an object.
		/// </summary>
		/// <param name="original">Original object</param>
		/// <param name="position">Position</param>
		/// <param name="rotation">Rotation</param>
		/// <param name="parent">Parent Transform</param>
		/// <returns>Instantiated object</returns>
		/// <remarks>
		/// If there is a pooled object, resume that object.<br/>
		/// If there is no pool, it is newly instantiated by Object.Instantiate.
		/// </remarks>
#endif
		public static Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent)
		{
			return Instantiate(original, position, rotation, parent, Space.World);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをインスタンス化する。
		/// </summary>
		/// <param name="original">オリジナルのオブジェクト</param>
		/// <param name="position">位置</param>
		/// <param name="rotation">回転</param>
		/// <param name="parent">親トランスフォーム</param>
		/// <param name="space">座標系</param>
		/// <returns>インスタンス化されたオブジェクト</returns>
		/// <remarks>
		/// プールされているオブジェクトがある場合はそのオブジェクトを再開させる。<br/>
		/// プールがない場合はObject.Instantiateにより新たにインスタンス化する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Instantiate an object.
		/// </summary>
		/// <param name="original">Original object</param>
		/// <param name="position">Position</param>
		/// <param name="rotation">Rotation</param>
		/// <param name="parent">Parent Transform</param>
		/// <param name="space">Coordinate system</param>
		/// <returns>Instantiated object</returns>
		/// <remarks>
		/// If there is a pooled object, resume that object.<br/>
		/// If there is no pool, it is newly instantiated by Object.Instantiate.
		/// </remarks>
#endif
		public static Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent, Space space)
		{
			CheckOriginalArgument(original);

			return DoInstantiate(original, parent, position, rotation, space, LifeTimeFlags.SceneUnloaded, 0f);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをインスタンス化する。
		/// </summary>
		/// <param name="original">オリジナルのオブジェクト</param>
		/// <param name="position">位置</param>
		/// <param name="rotation">回転</param>
		/// <param name="parent">親トランスフォーム</param>
		/// <param name="space">座標系</param>
		/// <param name="lifeTimeFlags">プールされたオブジェクトのライフタイムフラグ</param>
		/// <param name="lifeDuration">プールされたオブジェクトの生存時間</param>
		/// <returns>インスタンス化されたオブジェクト</returns>
		/// <remarks>
		/// プールされているオブジェクトがある場合はそのオブジェクトを再開させる。<br/>
		/// プールがない場合はObject.Instantiateにより新たにインスタンス化する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Instantiate an object.
		/// </summary>
		/// <param name="original">Original object</param>
		/// <param name="position">Position</param>
		/// <param name="rotation">Rotation</param>
		/// <param name="parent">Parent Transform</param>
		/// <param name="space">Coordinate system</param>
		/// <param name="lifeTimeFlags">Lifetime of pooled objects</param>
		/// <param name="lifeDuration">Time to live of pooled objects</param>
		/// <returns>Instantiated object</returns>
		/// <remarks>
		/// If there is a pooled object, resume that object.<br/>
		/// If there is no pool, it is newly instantiated by Object.Instantiate.
		/// </remarks>
#endif
		public static Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent, Space space, LifeTimeFlags lifeTimeFlags, float lifeDuration)
		{
			CheckOriginalArgument(original);

			return DoInstantiate(original, parent, position, rotation, space, lifeTimeFlags, lifeDuration);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをインスタンス化する。
		/// </summary>
		/// <param name="original">オリジナルのオブジェクト</param>
		/// <returns>インスタンス化されたオブジェクト</returns>
		/// <remarks>
		/// プールされているオブジェクトがある場合はそのオブジェクトを再開させる。<br/>
		/// プールがない場合はObject.Instantiateにより新たにインスタンス化する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Instantiate an object.
		/// </summary>
		/// <param name="original">Original object</param>
		/// <returns>Instantiated object</returns>
		/// <remarks>
		/// If there is a pooled object, resume that object.<br/>
		/// If there is no pool, it is newly instantiated by Object.Instantiate.
		/// </remarks>
#endif
		public static Object Instantiate(Object original)
		{
			return Instantiate(original, LifeTimeFlags.SceneUnloaded, 0f);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オフジェクトをインスタンス化する。
		/// </summary>
		/// <param name="original">オリジナルのオブジェクト</param>
		/// <param name="lifeTimeFlags">プールされたオブジェクトのライフタイムフラグ</param>
		/// <param name="lifeDuration">プールされたオブジェクトの生存時間</param>
		/// <returns>インスタンス化されたオブジェクト</returns>
		/// <remarks>
		/// プールされているオブジェクトがある場合はそのオブジェクトを再開させる。<br/>
		/// プールがない場合はObject.Instantiateにより新たにインスタンス化する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Instantiate an object.
		/// </summary>
		/// <param name="original">Original object</param>
		/// <param name="lifeTimeFlags">Lifetime of pooled objects</param>
		/// <param name="lifeDuration">Time to live of pooled objects</param>
		/// <returns>Instantiated object</returns>
		/// <remarks>
		/// If there is a pooled object, resume that object.<br/>
		/// If there is no pool, it is newly instantiated by Object.Instantiate.
		/// </remarks>
#endif
		public static Object Instantiate(Object original, LifeTimeFlags lifeTimeFlags, float lifeDuration)
		{
			CheckOriginalArgument(original);

			Transform transform = GetTransform(original);
			return DoInstantiate(original, null, transform.position, transform.rotation, Space.World, lifeTimeFlags, lifeDuration);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをインスタンス化する。
		/// </summary>
		/// <param name="original">オリジナルのオブジェクト</param>
		/// <param name="parent">親トランスフォーム</param>
		/// <returns>インスタンス化されたオブジェクト</returns>
		/// <remarks>
		/// プールされているオブジェクトがある場合はそのオブジェクトを再開させる。<br/>
		/// プールがない場合はObject.Instantiateにより新たにインスタンス化する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Instantiate an object.
		/// </summary>
		/// <param name="original">Original object</param>
		/// <param name="parent">Parent Transform</param>
		/// <returns>Instantiated object</returns>
		/// <remarks>
		/// If there is a pooled object, resume that object.<br/>
		/// If there is no pool, it is newly instantiated by Object.Instantiate.
		/// </remarks>
#endif
		public static Object Instantiate(Object original, Transform parent)
		{
			return Instantiate(original, parent, false);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをインスタンス化する。
		/// </summary>
		/// <param name="original">オリジナルのオブジェクト</param>
		/// <param name="parent">親トランスフォーム</param>
		/// <param name="instantiateInWorldSpace">parent を指定するときに、元のワールドの位置が維持されるかどうか</param>
		/// <returns>インスタンス化されたオブジェクト</returns>
		/// <remarks>
		/// プールされているオブジェクトがある場合はそのオブジェクトを再開させる。<br/>
		/// プールがない場合はObject.Instantiateにより新たにインスタンス化する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Instantiate an object.
		/// </summary>
		/// <param name="original">Original object</param>
		/// <param name="parent">Parent Transform</param>
		/// <param name="instantiateInWorldSpace">If when assigning the parent the original world position should be maintained.</param>
		/// <returns>Instantiated object</returns>
		/// <remarks>
		/// If there is a pooled object, resume that object.<br/>
		/// If there is no pool, it is newly instantiated by Object.Instantiate.
		/// </remarks>
#endif
		public static Object Instantiate(Object original, Transform parent, bool instantiateInWorldSpace)
		{
			return Instantiate(original, parent, instantiateInWorldSpace, LifeTimeFlags.SceneUnloaded, 0f);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをインスタンス化する。
		/// </summary>
		/// <param name="original">オリジナルのオブジェクト</param>
		/// <param name="parent">親トランスフォーム</param>
		/// <param name="instantiateInWorldSpace">parent を指定するときに、元のワールドの位置が維持されるかどうか</param>
		/// <param name="lifeTimeFlags">プールされたオブジェクトのライフタイムフラグ</param>
		/// <param name="lifeDuration">プールされたオブジェクトの生存時間</param>
		/// <returns>インスタンス化されたオブジェクト</returns>
		/// <remarks>
		/// プールされているオブジェクトがある場合はそのオブジェクトを再開させる。<br/>
		/// プールがない場合はObject.Instantiateにより新たにインスタンス化する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Instantiate an object.
		/// </summary>
		/// <param name="original">Original object</param>
		/// <param name="parent">Parent Transform</param>
		/// <param name="instantiateInWorldSpace">If when assigning the parent the original world position should be maintained.</param>
		/// <param name="lifeTimeFlags">Lifetime of pooled objects</param>
		/// <param name="lifeDuration">Time to live of pooled objects</param>
		/// <returns>Instantiated object</returns>
		/// <remarks>
		/// If there is a pooled object, resume that object.<br/>
		/// If there is no pool, it is newly instantiated by Object.Instantiate.
		/// </remarks>
#endif
		public static Object Instantiate(Object original, Transform parent, bool instantiateInWorldSpace, LifeTimeFlags lifeTimeFlags, float lifeDuration)
		{
			CheckOriginalArgument(original);

			return DoInstantiate(original, parent, instantiateInWorldSpace, lifeTimeFlags, lifeDuration);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをインスタンス化する。
		/// </summary>
		/// <typeparam name="T">オブジェクトの型</typeparam>
		/// <param name="original">オリジナルのオブジェクト</param>
		/// <returns>インスタンス化されたオブジェクト</returns>
		/// <remarks>
		/// プールされているオブジェクトがある場合はそのオブジェクトを再開させる。<br/>
		/// プールがない場合はObject.Instantiateにより新たにインスタンス化する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Instantiate an object.
		/// </summary>
		/// <typeparam name="T">Object type</typeparam>
		/// <param name="original">Original object</param>
		/// <returns>Instantiated object</returns>
		/// <remarks>
		/// If there is a pooled object, resume that object.<br/>
		/// If there is no pool, it is newly instantiated by Object.Instantiate.
		/// </remarks>
#endif
		public static T Instantiate<T>(T original) where T : Object
		{
			return (T)Instantiate((Object)original);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オフジェクトをインスタンス化する。
		/// </summary>
		/// <typeparam name="T">オブジェクトの型</typeparam>
		/// <param name="original">オリジナルのオブジェクト</param>
		/// <param name="lifeTimeFlags">プールされたオブジェクトのライフタイムフラグ</param>
		/// <param name="lifeDuration">プールされたオブジェクトの生存時間</param>
		/// <returns>インスタンス化されたオブジェクト</returns>
		/// <remarks>
		/// プールされているオブジェクトがある場合はそのオブジェクトを再開させる。<br/>
		/// プールがない場合はObject.Instantiateにより新たにインスタンス化する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Instantiate an object.
		/// </summary>
		/// <typeparam name="T">Object type</typeparam>
		/// <param name="original">Original object</param>
		/// <param name="lifeTimeFlags">Lifetime of pooled objects</param>
		/// <param name="lifeDuration">Time to live of pooled objects</param>
		/// <returns>Instantiated object</returns>
		/// <remarks>
		/// If there is a pooled object, resume that object.<br/>
		/// If there is no pool, it is newly instantiated by Object.Instantiate.
		/// </remarks>
#endif
		public static T Instantiate<T>(T original, LifeTimeFlags lifeTimeFlags, float lifeDuration) where T : Object
		{
			return (T)Instantiate((Object)original, lifeTimeFlags, lifeDuration);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをインスタンス化する。
		/// </summary>
		/// <typeparam name="T">オブジェクトの型</typeparam>
		/// <param name="original">オリジナルのオブジェクト</param>
		/// <param name="position">位置</param>
		/// <param name="rotation">回転</param>
		/// <returns>インスタンス化されたオブジェクト</returns>
		/// <remarks>
		/// プールされているオブジェクトがある場合はそのオブジェクトを再開させる。<br/>
		/// プールがない場合はObject.Instantiateにより新たにインスタンス化する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Instantiate an object.
		/// </summary>
		/// <typeparam name="T">Object type</typeparam>
		/// <param name="original">Original object</param>
		/// <param name="position">Position</param>
		/// <param name="rotation">Rotation</param>
		/// <returns>Instantiated object</returns>
		/// <remarks>
		/// If there is a pooled object, resume that object.<br/>
		/// If there is no pool, it is newly instantiated by Object.Instantiate.
		/// </remarks>
#endif
		public static T Instantiate<T>(T original, Vector3 position, Quaternion rotation) where T : Object
		{
			return (T)Instantiate((Object)original, position, rotation, null);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをインスタンス化する。
		/// </summary>
		/// <typeparam name="T">オブジェクトの型</typeparam>
		/// <param name="original">オリジナルのオブジェクト</param>
		/// <param name="position">位置</param>
		/// <param name="rotation">回転</param>
		/// <param name="parent">親トランスフォーム</param>
		/// <returns>インスタンス化されたオブジェクト</returns>
		/// <remarks>
		/// プールされているオブジェクトがある場合はそのオブジェクトを再開させる。<br/>
		/// プールがない場合はObject.Instantiateにより新たにインスタンス化する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Instantiate an object.
		/// </summary>
		/// <typeparam name="T">Object type</typeparam>
		/// <param name="original">Original object</param>
		/// <param name="position">Position</param>
		/// <param name="rotation">Rotation</param>
		/// <param name="parent">Parent Transform</param>
		/// <returns>Instantiated object</returns>
		/// <remarks>
		/// If there is a pooled object, resume that object.<br/>
		/// If there is no pool, it is newly instantiated by Object.Instantiate.
		/// </remarks>
#endif
		public static T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
		{
			return (T)Instantiate((Object)original, position, rotation, parent);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをインスタンス化する。
		/// </summary>
		/// <typeparam name="T">オブジェクトの型</typeparam>
		/// <param name="original">オリジナルのオブジェクト</param>
		/// <param name="position">位置</param>
		/// <param name="rotation">回転</param>
		/// <param name="parent">親トランスフォーム</param>
		/// <param name="lifeTimeFlags">プールされたオブジェクトのライフタイムフラグ</param>
		/// <param name="lifeDuration">プールされたオブジェクトの生存時間</param>
		/// <returns>インスタンス化されたオブジェクト</returns>
		/// <remarks>
		/// プールされているオブジェクトがある場合はそのオブジェクトを再開させる。<br/>
		/// プールがない場合はObject.Instantiateにより新たにインスタンス化する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Instantiate an object.
		/// </summary>
		/// <typeparam name="T">Object type</typeparam>
		/// <param name="original">Original object</param>
		/// <param name="position">Position</param>
		/// <param name="rotation">Rotation</param>
		/// <param name="parent">Parent Transform</param>
		/// <param name="lifeTimeFlags">Lifetime of pooled objects</param>
		/// <param name="lifeDuration">Time to live of pooled objects</param>
		/// <returns>Instantiated object</returns>
		/// <remarks>
		/// If there is a pooled object, resume that object.<br/>
		/// If there is no pool, it is newly instantiated by Object.Instantiate.
		/// </remarks>
#endif
		public static T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent, LifeTimeFlags lifeTimeFlags, float lifeDuration) where T : Object
		{
			return (T)Instantiate((Object)original, position, rotation, parent, lifeTimeFlags, lifeDuration);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをインスタンス化する。
		/// </summary>
		/// <typeparam name="T">オブジェクトの型</typeparam>
		/// <param name="original">オリジナルのオブジェクト</param>
		/// <param name="parent">親トランスフォーム</param>
		/// <returns>インスタンス化されたオブジェクト</returns>
		/// <remarks>
		/// プールされているオブジェクトがある場合はそのオブジェクトを再開させる。<br/>
		/// プールがない場合はObject.Instantiateにより新たにインスタンス化する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Instantiate an object.
		/// </summary>
		/// <typeparam name="T">Object type</typeparam>
		/// <param name="original">Original object</param>
		/// <param name="parent">Parent Transform</param>
		/// <returns>Instantiated object</returns>
		/// <remarks>
		/// If there is a pooled object, resume that object.<br/>
		/// If there is no pool, it is newly instantiated by Object.Instantiate.
		/// </remarks>
#endif
		public static T Instantiate<T>(T original, Transform parent) where T : Object
		{
			return (T)Instantiate((Object)original, parent, false);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをインスタンス化する。
		/// </summary>
		/// <typeparam name="T">オブジェクトの型</typeparam>
		/// <param name="original">オリジナルのオブジェクト</param>
		/// <param name="parent">親トランスフォーム</param>
		/// <param name="instantiateInWorldSpace">parent を指定するときに、元のワールドの位置が維持されるかどうか</param>
		/// <returns>インスタンス化されたオブジェクト</returns>
		/// <remarks>
		/// プールされているオブジェクトがある場合はそのオブジェクトを再開させる。<br/>
		/// プールがない場合はObject.Instantiateにより新たにインスタンス化する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Instantiate an object.
		/// </summary>
		/// <typeparam name="T">Object type</typeparam>
		/// <param name="original">Original object</param>
		/// <param name="parent">Parent Transform</param>
		/// <param name="instantiateInWorldSpace">If when assigning the parent the original world position should be maintained.</param>
		/// <returns>Instantiated object</returns>
		/// <remarks>
		/// If there is a pooled object, resume that object.<br/>
		/// If there is no pool, it is newly instantiated by Object.Instantiate.
		/// </remarks>
#endif
		public static T Instantiate<T>(T original, Transform parent, bool instantiateInWorldSpace) where T : Object
		{
			return (T)Instantiate((Object)original, parent, instantiateInWorldSpace);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトをインスタンス化する。
		/// </summary>
		/// <typeparam name="T">Object type</typeparam>
		/// <param name="original">オリジナルのオブジェクト</param>
		/// <param name="parent">親トランスフォーム</param>
		/// <param name="instantiateInWorldSpace">parent を指定するときに、元のワールドの位置が維持されるかどうか</param>
		/// <param name="lifeTimeFlags">プールされたオブジェクトのライフタイムフラグ</param>
		/// <param name="lifeDuration">プールされたオブジェクトの生存時間</param>
		/// <returns>インスタンス化されたオブジェクト</returns>
		/// <remarks>
		/// プールされているオブジェクトがある場合はそのオブジェクトを再開させる。<br/>
		/// プールがない場合はObject.Instantiateにより新たにインスタンス化する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Instantiate an object.
		/// </summary>
		/// <typeparam name="T">Object type</typeparam>
		/// <param name="original">Original object</param>
		/// <param name="parent">Parent Transform</param>
		/// <param name="instantiateInWorldSpace">If when assigning the parent the original world position should be maintained.</param>
		/// <param name="lifeTimeFlags">Lifetime of pooled objects</param>
		/// <param name="lifeDuration">Time to live of pooled objects</param>
		/// <returns>Instantiated object</returns>
		/// <remarks>
		/// If there is a pooled object, resume that object.<br/>
		/// If there is no pool, it is newly instantiated by Object.Instantiate.
		/// </remarks>
#endif
		public static T Instantiate<T>(T original, Transform parent, bool instantiateInWorldSpace, LifeTimeFlags lifeTimeFlags, float lifeDuration) where T : Object
		{
			return (T)Instantiate((Object)original, parent, instantiateInWorldSpace, lifeTimeFlags, lifeDuration);
		}

		private static List<PoolObject> _DestroyMarkers = new List<PoolObject>();

		static void DoDestroyMarkers()
		{
			foreach (var marker in _DestroyMarkers)
			{
				if (marker != null && marker.isValid)
				{
					DestroyImmediate(marker);
				}
			}

			_DestroyMarkers.Clear();
			PlayerLoopHelper.onDelayedFixedFrameRate -= DoDestroyMarkers;
		}
		
		internal static void Destroy(PoolObject poolObject)
		{
			if (_DestroyMarkers.Count == 0)
			{
				PlayerLoopHelper.onDelayedFixedFrameRate += DoDestroyMarkers;
			}
			_DestroyMarkers.Add(poolObject);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectを破棄する。
		/// </summary>
		/// <param name="gameObject">破棄するGameObject</param>
		/// <remarks>
		/// プール管理下のGameObjectであれば、プールへ返却する。<br/>
		/// 管理下でなければObject.Destroyにより破棄する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Destroy GameObject.
		/// </summary>
		/// <param name="gameObject">GameObject to destroy</param>
		/// <remarks>
		/// If it is a GameObject under pool management, return it to the pool.<br/>
		/// If it is not under management, it is destroyed by Object.Destroy.<br/>
		/// </remarks>
#endif
		public static void Destroy(GameObject gameObject)
		{
			PoolObject poolObject = null;
			if (gameObject.TryGetComponent<PoolObject>(out poolObject) && poolObject.isValid)
			{
				Destroy(poolObject);
			}
			else
			{
				Object.Destroy(gameObject);
			}
		}

		internal static void DestroyImmediate(PoolObject poolObject)
		{
			poolObject.OnPoolSleep();

			Object original = poolObject.original;
			PoolQueue poolQueue = GetPool(original);
			poolQueue.Enqueue(poolObject);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectを即座に破棄する。
		/// </summary>
		/// <param name="gameObject">破棄するGameObject</param>
		/// <remarks>
		/// プール管理下のGameObjectであれば、プールへ返却する。<br/>
		/// 管理下でなければObject.DestroyImmediateにより破棄する。<br/>
		/// </remarks>
#else
		/// <summary>
		/// Destroy GameObject immediately.
		/// </summary>
		/// <param name="gameObject">GameObject to destroy</param>
		/// <remarks>
		/// If it is a GameObject under pool management, return it to the pool.<br/>
		/// If it is not under management, it is destroyed by Object.DestroyImmediate.<br/>
		/// </remarks>
#endif
		public static void DestroyImmediate(GameObject gameObject)
		{
			PoolObject poolObject = null;
			if (gameObject.TryGetComponent<PoolObject>(out poolObject) && poolObject.isValid)
			{
				DestroyImmediate(poolObject);
			}
			else
			{
				Object.DestroyImmediate(gameObject);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Objectが生きているかを確認する。
		/// </summary>
		/// <param name="obj">オブジェクト</param>
		/// <returns>オブジェクトがDestroyされておらず、プールにも格納されていなければtrueを返す。</returns>
#else
		/// <summary>
		/// Check if the Object is alive.
		/// </summary>
		/// <param name="obj">Object</param>
		/// <returns>Returns true if the object is not Destroyed and is not stored in the pool either.</returns>
#endif
		public static bool IsAlive(Object obj)
		{
			if (obj == null)
			{
				return false;
			}

			var transform = GetTransform(obj);
			while (transform != null)
			{
				if (transform.TryGetComponent<PoolObject>(out var poolObject))
				{
					if (poolObject != null && poolObject.isPooled)
					{
						return false;
					}
				}

				transform = transform.parent;
			}

			return true;
		}
	}
}