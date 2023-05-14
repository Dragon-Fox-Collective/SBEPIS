//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 再生状態
	/// </summary>
#else
	/// <summary>
	/// Play state
	/// </summary>
#endif
	[Internal.Documentable]
	public enum PlayState
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 停止中
		/// </summary>
#else
		/// <summary>
		/// Stopping
		/// </summary>
#endif
		Stopping,

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生中
		/// </summary>
#else
		/// <summary>
		/// Playing
		/// </summary>
#endif
		Playing,

#if ARBOR_DOC_JA
		/// <summary>
		/// ポーズ中
		/// </summary>
#else
		/// <summary>
		/// Pausing
		/// </summary>
#endif
		Pausing,

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生中に無効化したためポーズ中
		/// </summary>
#else
		/// <summary>
		/// Pause because disabled during playback
		/// </summary>
#endif
		InactivePausing,
	}
}