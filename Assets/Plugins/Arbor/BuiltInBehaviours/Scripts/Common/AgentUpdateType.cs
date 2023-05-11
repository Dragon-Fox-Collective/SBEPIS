//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Agentの更新タイプ
	/// </summary>
#else
	/// <summary>
	/// Time type
	/// </summary>
#endif
	[Internal.Documentable]
	public enum AgentUpdateType
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 時間指定で更新。
		/// </summary>
#else
		/// <summary>
		/// Update by time designation.
		/// </summary>
#endif
		Time,

#if ARBOR_DOC_JA
		/// <summary>
		/// 完了したら更新。
		/// </summary>
#else
		/// <summary>
		/// Update when done。
		/// </summary>
#endif
		Done,

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始時のみ更新
		/// </summary>
#else
		/// <summary>
		/// Update only at start
		/// </summary>
#endif
		StartOnly,

#if ARBOR_DOC_JA
		/// <summary>
		/// 常に更新
		/// </summary>
#else
		/// <summary>
		/// Always updated
		/// </summary>
#endif
		Always,
	}
}