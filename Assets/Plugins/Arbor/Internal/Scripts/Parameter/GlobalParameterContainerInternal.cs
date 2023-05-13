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
	/// シーンをまたいでもアクセス可能なParameterContainerを扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class dealing with the accessible ParameterContainer even across the scene.
	/// </summary>
#endif
	[AddComponentMenu("")]
	public class GlobalParameterContainerInternal : ParameterContainerBase
	{
		static Dictionary<ParameterContainerInternal, ParameterContainerInternal> s_Instancies = new Dictionary<ParameterContainerInternal, ParameterContainerInternal>();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void OnSubsystemRegistration()
		{
			s_Instancies.Clear();
		}

		#region Serialize fields

		/// <summary>
		/// シーンを跨ぐ際に使用する共通のParameterContainerのプレハブを指定する。
		/// </summary>
		[SerializeField] private ParameterContainerInternal _Prefab = null;

		#endregion // Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 元のParameterContainerを返す。
		/// </summary>
#else
		/// <summary>
		///It returns the original ParameterContainer.
		/// </summary>
#endif
		public ParameterContainerInternal prefab
		{
			get
			{
				return _Prefab;
			}
		}

		private ParameterContainerInternal _Instance = null;
		private bool _IsInitialized = false;

#if ARBOR_DOC_JA
		/// <summary>
		/// 実体のParameterContainerを返す。
		/// </summary>
#else
		/// <summary>
		/// It returns the ParameterContainer entity.
		/// </summary>
#endif
		public ParameterContainerInternal instance
		{
			get
			{
				MakeInstance();
				return _Instance;
			}
		}

		void MakeInstance()
		{
			if (_IsInitialized)
			{
				return;
			}

			_IsInitialized = true;

			if (_Prefab == null)
			{
				return;
			}

			if (s_Instancies.TryGetValue(_Prefab, out _Instance))
			{
				if (_Instance != null)
				{
					return;
				}

				Debug.LogWarning("The instance of ParameterContianer referenced from GlobalParameterContainer has been deleted.", this);
				s_Instancies.Remove(_Prefab);
			}

			_Instance = (ParameterContainerInternal)Instantiate(_Prefab);
			DontDestroyOnLoad(_Instance.gameObject);
			s_Instancies.Add(_Prefab, _Instance);
		}

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
			MakeInstance();
		}
	}
}
