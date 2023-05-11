//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// <see cref="UnityEngine.AI.OffMeshLink"/>を通過するときの待機設定。
	/// </summary>
#else
	/// <summary>
	/// Wait settings when traversing <see cref="UnityEngine.AI.OffMeshLink"/>.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.Documentable]
	public struct OffMeshLinkTraverseWaitData
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 待機方法
		/// </summary>
#else
		/// <summary>
		/// How to wait
		/// </summary>
#endif
		[Internal.Documentable]
		public enum WaitType
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// 待機しない。
			/// </summary>
#else
			/// <summary>
			/// Don't wait.
			/// </summary>
#endif
			None,

#if ARBOR_DOC_JA
			/// <summary>
			/// 時間経過を待つ
			/// </summary>
#else
			/// <summary>
			/// Wait for the passage of time
			/// </summary>
#endif
			Time,

#if ARBOR_DOC_JA
			/// <summary>
			/// AnimationEventを待つ。<br/>
			/// Animatorコンポーネントと同じGameObjectにAnimationTriggerEventReceiverを設定し、AnimationEventでAnimationTriggerEventReceiver.Triggerを呼び出すことで待機終了。
			/// </summary>
#else
			/// <summary>
			/// Wait for the Animation Event.<br/>
			/// Set AnimationTriggerEventReceiver in the same GameObject as the Animator component, and call AnimationTriggerEventReceiver.Trigger in AnimationEvent to end the wait.
			/// </summary>
#endif
			AnimationEvent,
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 待機方法
		/// </summary>
#else
		/// <summary>
		/// How to wait
		/// </summary>
#endif
		public WaitType type;

#if ARBOR_DOC_JA
		/// <summary>
		/// 待機時間(typeをWaitType.Timeにした場合に使用)
		/// </summary>
#else
		/// <summary>
		/// Wait time (used when type is set to WaitType.Time)
		/// </summary>
#endif
		public float time;

#if ARBOR_DOC_JA
		/// <summary>
		/// AnimationEventの名前<br/>
		/// 呼び出すTriggerメソッドの引数の文字列と同一の名前を設定する。
		/// </summary>
#else
		/// <summary>
		/// The name of the Animation Event<br/>
		/// Set the same name as the argument string of the Trigger method to be called.
		/// </summary>
#endif
		public string eventName;
	}
}