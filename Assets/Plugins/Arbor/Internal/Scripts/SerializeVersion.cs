//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Serializable属性のクラスでのバージョン管理を行う。
	/// </summary>
#else
	/// <summary>
	/// Perform version management with the class of Serializable attribute.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class SerializeVersion
	{
		[SerializeField]
		private int _Version = 0;

		[SerializeField]
		private bool _IsInitialized = true;

		[SerializeField]
		private bool _IsVersioning = false;

		private ISerializeVersionCallbackReceiver _Callback;

#if ARBOR_DOC_JA
		/// <summary>
		/// バージョン番号
		/// </summary>
#else
		/// <summary>
		/// Version number
		/// </summary>
#endif
		public int version
		{
			get
			{
				return _Version;
			}
			set
			{
				_Version = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 初期化フラグ。
		/// </summary>
#else
		/// <summary>
		/// Initialization flag.
		/// </summary>
#endif
		public bool isInitialized
		{
			get
			{
				return _IsInitialized;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// バージョン管理フラグ
		/// </summary>
#else
		/// <summary>
		/// Version control flag
		/// </summary>
#endif
		public bool isVersioning
		{
			get
			{
				return _IsVersioning;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 初期化を行う。SerializeVersionを持つ型は、コンストラクターでこのメソッドを呼ぶ必要がある。
		/// </summary>
		/// <param name="callback">コールバックレシーバー</param>
		/// <returns>初期化を行った場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Perform initialization. Types with SerializeVersion need to call this method in the constructor.
		/// </summary>
		/// <param name="callback">Callback receiver</param>
		/// <returns>Returns true when initialization is performed.</returns>
#endif
		public bool Initialize(ISerializeVersionCallbackReceiver callback)
		{
			_Callback = callback;

			_IsInitialized = false;
			return InitializeIfNecessary();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 初期化が必要であれば初期化を行う。
		/// </summary>
		/// <returns>初期化を行った場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Perform initialization if necessary.
		/// </summary>
		/// <returns>Returns true when initialization is performed.</returns>
#endif
		public bool InitializeIfNecessary()
		{
			if (_IsInitialized)
			{
				return false;
			}

			_IsInitialized = true;
			if (_Callback != null)
			{
				_Callback.OnInitialize();
			}

			return true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 必要であればバージョン管理に移行する。
		/// </summary>
		/// <returns>バージョン管理に移行した場合にtrueを返す。</returns>
#else
		/// <summary>
		/// If necessary to migrate to version control.
		/// </summary>
		/// <returns>Returns true if the version has been migrated.</returns>
#endif
		public bool VersioningIfNesessory()
		{
			if (!_IsVersioning && _IsInitialized)
			{
				_Version = 0;
				_IsVersioning = true;

				if (_Callback != null)
				{
					_Callback.OnVersioning();
				}

				return true;
			}

			return false;
		}

		void Serialize()
		{
			if (_Callback == null)
			{
				return;
			}

			int newestVersion = _Callback.newestVersion;

			while (this.version < newestVersion)
			{
				_Callback.OnSerialize(this.version);
				this.version++;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// デシリアライズ後の処理。SerializeVersionを持つ型は、ISerializationCallbackReceiver.OnAfterDeserialize()からこのメソッドを呼ぶ必要がある。
		/// </summary>
#else
		/// <summary>
		/// Processing after deserialization. Types with SerializeVersion need to call this method from ISerializationCallbackReceiver.OnAfterDeserialize().
		/// </summary>
#endif
		public void AfterDeserialize()
		{
			VersioningIfNesessory();

			InitializeIfNecessary();

			Serialize();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// シリアライズ前の処理。SerializeVersionを持つ型は、ISerializationCallbackReceiver.OnBeforeDeserialize()からこのメソッドを呼ぶ必要がある。
		/// </summary>
#else
		/// <summary>
		/// Processing before serialization. Types with SerializeVersion need to call this method from ISerializationCallbackReceiver.OnBeforeDeserialize().
		/// </summary>
#endif
		public void BeforeDeserialize()
		{
			if (!_IsVersioning)
			{
				_IsVersioning = true;
			}

			Serialize();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// バージョン情報を文字列で返す。
		/// </summary>
		/// <returns>バージョン情報の文字列。</returns>
#else
		/// <summary>
		/// Returns version information as a string.
		/// </summary>
		/// <returns>Version information string.</returns>
#endif
		public override string ToString()
		{
			return string.Format("(version {0}, initialized {1}, versioning {2})", _Version, _IsInitialized, _IsVersioning);
		}
	}
}