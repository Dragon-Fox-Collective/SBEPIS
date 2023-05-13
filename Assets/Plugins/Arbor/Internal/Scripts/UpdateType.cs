//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 更新タイプ。
	/// </summary>
#else
	/// <summary>
	/// Update type.
	/// </summary>
#endif
	[Internal.Documentable]
	public enum UpdateType
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 毎フレーム更新。
		/// </summary>
#else
		/// <summary>
		/// Update every frame.
		/// </summary>
#endif
		EveryFrame,

#if ARBOR_DOC_JA
		/// <summary>
		/// 秒を指定して更新。
		/// </summary>
#else
		/// <summary>
		/// Updated by specifying seconds.
		/// </summary>
#endif
		SpecifySeconds,

#if ARBOR_DOC_JA
		/// <summary>
		/// スクリプトから手動で更新。<br/>
		/// 更新メソッドは<a href="https://arbor-docs.caitsithware.com/ja/scriptreference.html">スクリプトリファレンス</a>の該当コンポーネントのページを参照してください。
		/// </summary>
#else
		/// <summary>
		/// Manually updated from the script.<br/>
		/// For the update method, refer to the page of the corresponding component in the <a href="https://arbor-docs.caitsithware.com/en/scriptreference.html">script reference</a>.
		/// </summary>
#endif
		Manual,
	}
}