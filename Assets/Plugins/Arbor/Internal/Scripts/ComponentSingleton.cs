//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Component型のシングルトンクラス
	/// </summary>
	/// <typeparam name="T">このクラスを継承した型を指定する</typeparam>
#else
	/// <summary>
	/// Component type singleton class
	/// </summary>
	/// <typeparam name="T">Specify a type that inherits from this class</typeparam>
#endif
	[ExecuteInEditMode]
	public abstract class ComponentSingleton<T> : MonoBehaviour where T : ComponentSingleton<T>
	{
		private static T s_Instance = null;

#if ARBOR_DOC_JA
		/// <summary>
		/// インスタンスを取得する。
		/// </summary>
#else
		/// <summary>
		/// To get an instance.
		/// </summary>
#endif
		public static T instance
		{
			get
			{
				if (s_Instance == null)
				{
					return ObjectUtility.FindObjectOfType<T>() ?? CreateNewSingleton();
				}

				return s_Instance;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// インスタンスが存在するかを取得する。
		/// </summary>
#else
		/// <summary>
		/// Get if the instance exists.
		/// </summary>
#endif
		public static bool exists
		{
			get
			{
				return s_Instance != null;
			}
		}

		static T CreateNewSingleton()
		{
			var go = new GameObject();

			if (Application.isPlaying)
			{
				DontDestroyOnLoad(go);
				go.hideFlags |= HideFlags.NotEditable;
			}
			else
			{
				go.hideFlags |= HideFlags.HideInHierarchy;
			}

			var instance = go.AddComponent<T>();
			go.name = instance.GetGameObjectName();
			return instance;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// インスタンスを破棄する
		/// </summary>
#else
		/// <summary>
		/// Destroy the instance
		/// </summary>
#endif
		public static void DestroySingleton()
		{
			if (s_Instance != null)
			{
				DestroyImmediate(s_Instance.gameObject);
				s_Instance = null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 作成するGameObjectの名前を取得する。
		/// </summary>
		/// <returns>作成するGameObjectの名前</returns>
#else
		/// <summary>
		/// Get the name of the GameObject to create.
		/// </summary>
		/// <returns>The name of the GameObject to create</returns>
#endif
		protected virtual string GetGameObjectName() => typeof(T).Name;

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数はスクリプトのインスタンスがロードされたときに呼び出される。
		/// </summary>
#else
		/// <summary>
		/// This function is called when the script instance is being loaded.
		/// </summary>
#endif
		protected virtual void Awake()
		{
			if (s_Instance != null && s_Instance != this)
			{
				DestroyImmediate(gameObject);
				return;
			}
			s_Instance = this as T;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数はMonoBehaviourが破棄されるときに呼び出される。
		/// </summary>
#else
		/// <summary>
		/// This function is called when MonoBehaivour will be destroyed.
		/// </summary>
#endif
		protected virtual void OnDestroy()
		{
			if (s_Instance == this)
			{
				s_Instance = null;
			}
		}
	}
}