//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ScriptableObjectをシングルトンにするクラス。
	/// </summary>
	/// <typeparam name="T">シングルトンにする型。継承した型をそのまま指定する。</typeparam>
#else
	/// <summary>
	/// Class that the ScriptableObject to Singleton.
	/// </summary>
	/// <typeparam name="T">A type to make a singleton. Specify the inherited type as it is.</typeparam>
#endif
	public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
	{
		private static T _Instance = null;

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
				if (_Instance == null)
				{
					T[] objects = Resources.FindObjectsOfTypeAll<T>();
					if (objects.Length > 0)
					{
						_Instance = objects[0];
					}
				}
				if (_Instance == null)
				{
					_Instance = ScriptableObject.CreateInstance<T>();
					_Instance.hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;
				}
				return _Instance;
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
				return _Instance != null;
			}
		}
	}
}
