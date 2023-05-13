//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.Playables
{
#if ARBOR_DOC_JA
	/// <summary>
	/// PlayableBehaviourの拡張クラス。PlayableGraphを継承して自作グラフを作成する場合に各種メソッド呼び出しに使用する。
	/// </summary>
#else
	/// <summary>
	/// An extension class for PlayableBehaviour. Used for various method calls when creating a self-made graph by inheriting PlayableGraph.
	/// </summary>
#endif
	public static class PlayableBehaviourExtensions
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 挙動がアクティブであるかどうか。
		/// </summary>
		/// <param name="playableBehaviour">PlayableBehaviour</param>
		/// <returns>アクティブである場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Whether the behavior is active.
		/// </summary>
		/// <param name="playableBehaviour">PlayableBehaviour</param>
		/// <returns>Returns true if active.</returns>
#endif
		public static bool IsActive(this PlayableBehaviour playableBehaviour)
		{
			return playableBehaviour.isActive;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 挙動のアクティブ状態を設定する。
		/// </summary>
		/// <param name="playableBehaviour">PlayableBehaviour</param>
		/// <param name="active">設定するアクティブ状態</param>
		/// <param name="changeState">状態を変更するフラグ</param>
#else
		/// <summary>
		/// Set the active state of behavior.
		/// </summary>
		/// <param name="playableBehaviour">PlayableBehaviour</param>
		/// <param name="active">Active state to set</param>
		/// <param name="changeState">Flag to change state</param>
#endif
		public static void Activate(this PlayableBehaviour playableBehaviour, bool active, bool changeState)
		{
			playableBehaviour.ActivateInternal(active, changeState);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 一時停止する。
		/// </summary>
		/// <param name="playableBehaviour">PlayableBehaviour</param>
#else
		/// <summary>
		/// Pause.
		/// </summary>
		/// <param name="playableBehaviour">PlayableBehaviour</param>
#endif
		public static void Pause(this PlayableBehaviour playableBehaviour)
		{
			playableBehaviour.PauseInternal();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再開する。
		/// </summary>
		/// <param name="playableBehaviour">PlayableBehaviour</param>
#else
		/// <summary>
		/// Resume.
		/// </summary>
		/// <param name="playableBehaviour">PlayableBehaviour</param>
#endif
		public static void Resume(this PlayableBehaviour playableBehaviour)
		{
			playableBehaviour.ResumeInternal();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 停止する。
		/// </summary>
		/// <param name="playableBehaviour">PlayableBehaviour</param>
#else
		/// <summary>
		/// Stop.
		/// </summary>
		/// <param name="playableBehaviour">PlayableBehaviour</param>
#endif
		public static void Stop(this PlayableBehaviour playableBehaviour)
		{
			playableBehaviour.StopInternal();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 更新する。
		/// </summary>
		/// <param name="playableBehaviour">PlayableBehaviour</param>
#else
		/// <summary>
		/// Update.
		/// </summary>
		/// <param name="playableBehaviour">PlayableBehaviour</param>
#endif
		public static void Update(this PlayableBehaviour playableBehaviour)
		{
			playableBehaviour.UpdateInternal();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// LateUpdateで更新する。
		/// </summary>
		/// <param name="playableBehaviour">PlayableBehaviour</param>
#else
		/// <summary>
		/// Update with LateUpdate.
		/// </summary>
		/// <param name="playableBehaviour">PlayableBehaviour</param>
#endif
		public static void LateUpdate(this PlayableBehaviour playableBehaviour)
		{
			playableBehaviour.LateUpdateInternal();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FixedUpdateで更新する。
		/// </summary>
		/// <param name="playableBehaviour">PlayableBehaviour</param>
#else
		/// <summary>
		/// Update with FixedUpdate.
		/// </summary>
		/// <param name="playableBehaviour">PlayableBehaviour</param>
#endif
		public static void FixedUpdate(this PlayableBehaviour playableBehaviour)
		{
			playableBehaviour.FixedUpdateInternal();
		}
	}
}