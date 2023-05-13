//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Arborリファレンスに関するユーティリティクラス
	/// </summary>
#else
	/// <summary>
	/// Arbor reference utility class
	/// </summary>
#endif
	public static class ArborReferenceUtility
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// ドキュメントのURL
		/// </summary>	
#else
		/// <summary>
		/// Document URL
		/// </summary>
#endif
#if ARBOR_BETA
		public const string docUrl = "https://arbor-docs.caitsithware.com/beta/";
#else
		public const string docUrl = "https://arbor-docs.caitsithware.com/";
#endif

#if ARBOR_DOC_JA
		/// <summary>
		/// Inspectorリファレンスのディレクトリ名
		/// </summary>	
#else
		/// <summary>
		/// Directory name of Inspector Reference
		/// </summary>
#endif
		public const string inspectorDirectoryName = "inspector";

#if ARBOR_DOC_JA
		/// <summary>
		/// ComponentドキュメントのURL
		/// </summary>	
#else
		/// <summary>
		/// Component Document URL
		/// </summary>
#endif
		public const string componentUrl = docUrl + inspectorDirectoryName + "/components/";
	}
}