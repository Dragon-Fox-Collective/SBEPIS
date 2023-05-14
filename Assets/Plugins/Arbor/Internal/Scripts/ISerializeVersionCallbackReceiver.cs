//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// SerializeVersionクラスから呼び出されるコールバックのレシーバー
	/// </summary>
#else
	/// <summary>
	/// Callback receiver called from SerializeVersion class
	/// </summary>
#endif
	public interface ISerializeVersionCallbackReceiver
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 最新バージョン
		/// </summary>
#else
		/// <summary>
		/// newest version
		/// </summary>
#endif
		int newestVersion
		{
			get;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 初期化時に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called at initialization.
		/// </summary>
#endif
		void OnInitialize();

#if ARBOR_DOC_JA
		/// <summary>
		/// シリアライズ済みデータがバージョン管理されるように切り替わった際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when the serialized data is switched to version control.
		/// </summary>
#endif
		void OnVersioning();

#if ARBOR_DOC_JA
		/// <summary>
		/// バージョンが更新される際に呼ばれる。
		/// </summary>
		/// <param name="version">現在のバージョン</param>
#else
		/// <summary>
		/// Called when a version is updated.
		/// </summary>
		/// <param name="version">Current version</param>
#endif
		void OnSerialize(int version);
	}
}