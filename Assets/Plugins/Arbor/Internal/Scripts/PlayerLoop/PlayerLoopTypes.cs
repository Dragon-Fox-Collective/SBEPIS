//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor.PlayerLoop
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Arborが差し込むPlayerLoopのタイプ
	/// </summary>
#else
	/// <summary>
	/// The type of Player Loop that Arbor plugs in
	/// </summary>
#endif
	public static class PlayerLoopTypes
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="UnityEngine.PlayerLoop.FixedUpdate.ScriptRunDelayedFixedFrameRate"/>の後に差し込むPlayerLoopのタイプ。
		/// </summary>
#else
		/// <summary>
		/// The type of PlayerLoop to insert after <see cref="UnityEngine.PlayerLoop.FixedUpdate.ScriptRunDelayedFixedFrameRate"/>.
		/// </summary>
#endif
		public struct ArborDelayedFixedFrameRate 
		{
		}
	}
}