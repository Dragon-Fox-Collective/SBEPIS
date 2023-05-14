//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor.StateMachine.StateBehaviours
{
	using Arbor.Events;

#if ARBOR_DOC_JA
	/// <summary>
	/// メソッドやフィールドを呼び出す。
	/// </summary>
	/// <remarks>
	/// 呼び出しはリフレクションを介して行われるため、ストリッピングにより呼び出し先メンバーがビルドから削除される可能性があります。<br />
	/// 詳しくは<a href="https://docs.unity3d.com/ja/current/Manual/ManagedCodeStripping.html">マネージコードストリッピング - Unity マニュアル</a>を参照してください。
	/// </remarks>
#else
	/// <summary>
	/// Call a method or field.
	/// </summary>
	/// <remarks>
	/// Since the call is made via reflection, stripping can cause the called member to be removed from the build.<br />
	/// See <a href="https://docs.unity3d.com/Manual/ManagedCodeStripping.html">Unity - Manual:  Managed code stripping</a> for more information.
	/// </remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Events/InvokeMethod")]
	[BuiltInBehaviour]
	[Internal.DocumentManual("/manual/dataflow/invoke.md")]
	public sealed class InvokeMethod : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// ステートのイベントトリガーのタイプ
		/// </summary>
#else
		/// <summary>
		/// State event trigger type
		/// </summary>
#endif
		[Arbor.Internal.Documentable]
		public enum StateTriggerType
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// OnStateAwakeの時にイベントを呼ぶ
			/// </summary>
#else
			/// <summary>
			/// Call an event at OnStateAwake
			/// </summary>
#endif
			OnStateAwake,

#if ARBOR_DOC_JA
			/// <summary>
			/// OnStateBeginの時にイベントを呼ぶ
			/// </summary>
#else
			/// <summary>
			/// Call an event at OnStateBegin
			/// </summary>
#endif
			OnStateBegin,

#if ARBOR_DOC_JA
			/// <summary>
			/// OnStateUpdateの時にイベントを呼ぶ
			/// </summary>
#else
			/// <summary>
			/// Call an event at OnStateUpdate
			/// </summary>
#endif
			OnStateUpdate,

#if ARBOR_DOC_JA
			/// <summary>
			/// OnStateLateUpdateの時にイベントを呼ぶ
			/// </summary>
#else
			/// <summary>
			/// Call an event at OnStateLateUpdate
			/// </summary>
#endif
			OnStateLateUpdate,

#if ARBOR_DOC_JA
			/// <summary>
			/// OnStateEndの時にイベントを呼ぶ
			/// </summary>
#else
			/// <summary>
			/// Call an event at OnStateLateUpdate
			/// </summary>
#endif
			OnStateEnd,
		}

		[System.Serializable]
		[Internal.Documentable]
		private sealed class EventEntry
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// ステートのイベントトリガーのタイプ
			/// </summary>
#else
			/// <summary>
			/// State event trigger type
			/// </summary>
#endif
			public StateTriggerType triggerType = StateTriggerType.OnStateAwake;

#if ARBOR_DOC_JA
			/// <summary>
			/// 呼び出すメンバーの設定
			/// </summary>
#else
			/// <summary>
			/// Setting of members to call
			/// </summary>
#endif
			public ArborEvent callback = new ArborEvent();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 登録しているイベントのリスト。<br/>
		/// 必要に応じて「Add New Event Type」ボタンをクリックすることでイベントのタイプを追加できます。
		/// </summary>
#else
		/// <summary>
		/// List of registered events.<br/>
		/// If necessary, you can add an event type by clicking the "Add New Event Type" button.
		/// </summary>
#endif
		[SerializeField]
		private List<EventEntry> _EventEntries = new List<EventEntry>();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		private ArborEvent _OnStateAwake = new ArborEvent();

		[SerializeField]
		[HideInInspector]
		private ArborEvent _OnStateBegin = new ArborEvent();

		[SerializeField]
		[HideInInspector]
		private ArborEvent _OnStateEnd = new ArborEvent();

		#endregion // old

		private const int kCurrentSerializeVersion = 1;

		void Awake()
		{
			LogWarning();
		}

		void Execute(StateTriggerType triggerType)
		{
			for (int i = 0, count = _EventEntries.Count; i < count; ++i)
			{
				EventEntry entry = _EventEntries[i];
				if (entry.triggerType == triggerType && entry.callback != null)
				{
					entry.callback.Invoke();
				}
			}
		}

		public override void OnStateAwake()
		{
			Execute(StateTriggerType.OnStateAwake);
		}

		public override void OnStateBegin()
		{
			Execute(StateTriggerType.OnStateBegin);
		}

		public override void OnStateUpdate()
		{
			Execute(StateTriggerType.OnStateUpdate);
		}

		public override void OnStateLateUpdate()
		{
			Execute(StateTriggerType.OnStateLateUpdate);
		}

		public override void OnStateEnd()
		{
			Execute(StateTriggerType.OnStateEnd);
		}

		[System.Diagnostics.Conditional("UNITY_EDITOR")]
		void LogWarning()
		{
			for (int i = 0, count = _EventEntries.Count; i < count; ++i)
			{
				EventEntry entry = _EventEntries[i];
				if (entry.callback != null)
				{
					string warningMessage = entry.callback.warningMessage;
					if (!string.IsNullOrEmpty(warningMessage))
					{
						Debug.LogWarningFormat(nodeGraph, "[{0} {1} Event]\n{2}", this, entry.triggerType, warningMessage);
					}
				}
			}
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void AddEntry(StateTriggerType triggerType, ArborEvent callback)
		{
			EventEntry entry = new EventEntry();
			entry.triggerType = triggerType;
			entry.callback = callback;

			_EventEntries.Add(entry);
		}

		void SerializeVer1()
		{
			AddEntry(StateTriggerType.OnStateAwake, _OnStateAwake);
			_OnStateAwake = null;

			AddEntry(StateTriggerType.OnStateBegin, _OnStateBegin);
			_OnStateBegin = null;

			AddEntry(StateTriggerType.OnStateEnd, _OnStateEnd);
			_OnStateEnd = null;
		}

		void Serialize()
		{
			while (_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_SerializeVersion++;
						break;
					default:
						_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
			Serialize();
		}

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			Serialize();
		}
	}
}