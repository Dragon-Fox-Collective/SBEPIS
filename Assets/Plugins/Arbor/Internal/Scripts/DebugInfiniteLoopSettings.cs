//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 無限ループのデバッグ設定
	/// </summary>
#else
	/// <summary>
	/// Debug setting of infinite loop
	/// </summary>
#endif
	[System.Serializable]
	[Internal.Documentable]
	public sealed class DebugInfiniteLoopSettings
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 最大ループカウント
		/// </summary>
#else
		/// <summary>
		/// Maximum loop count
		/// </summary>
#endif
		public int maxLoopCount = 10000;

#if ARBOR_DOC_JA
		/// <summary>
		/// ログを有効。ループカウントがmaxLoopCount以上になった時にログ出力します。
		/// </summary>
#else
		/// <summary>
		/// Enable logging. Log output when the loop count is over maxLoopCount.
		/// </summary>
#endif
		public bool enableLogging = true;

#if ARBOR_DOC_JA
		/// <summary>
		/// Debug.Breakを有効化。ループカウントがmaxLoopCount以上になった時にDebug.Breakします。
		/// </summary>
#else
		/// <summary>
		/// Enable Break. Debug.Break when the loop count is over maxLoopCount.
		/// </summary>
#endif
		public bool enableBreak = false;
	}
}