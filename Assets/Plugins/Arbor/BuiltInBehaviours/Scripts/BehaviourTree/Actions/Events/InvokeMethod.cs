//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

namespace Arbor.BehaviourTree.Actions
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
	public sealed class InvokeMethod : ActionBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// アクションのイベントトリガーのタイプ
		/// </summary>
#else
		/// <summary>
		/// Action event trigger type
		/// </summary>
#endif
		[Arbor.Internal.Documentable]
		public enum ActionTriggerType
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// OnAwakeの時にイベントを呼ぶ
			/// </summary>
#else
			/// <summary>
			/// Call an event at OnAwake
			/// </summary>
#endif
			OnAwake,

#if ARBOR_DOC_JA
			/// <summary>
			/// OnStartの時にイベントを呼ぶ
			/// </summary>
#else
			/// <summary>
			/// Call an event at OnStart
			/// </summary>
#endif
			OnStart,

#if ARBOR_DOC_JA
			/// <summary>
			/// OnExecuteの時にイベントを呼ぶ
			/// </summary>
#else
			/// <summary>
			/// Call an event at OnExecute
			/// </summary>
#endif
			OnExecute,

#if ARBOR_DOC_JA
			/// <summary>
			/// OnEndの時にイベントを呼ぶ
			/// </summary>
#else
			/// <summary>
			/// Call an event at OnEnd
			/// </summary>
#endif
			OnEnd,
		}

		[System.Serializable]
		private sealed class EventEntry
		{
			public ActionTriggerType triggerType = ActionTriggerType.OnAwake;
			public ArborEvent callback = new ArborEvent();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 実行のタイプ
		/// </summary>
#else
		/// <summary>
		/// Execute type
		/// </summary>
#endif
		[Arbor.Internal.Documentable]
		public enum ExecuteType
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// 実行を維持。Decoratorにより失敗が返るか、他のノードが割り込むまで実行する。
			/// </summary>
#else
			/// <summary>
			/// Keep executing. Execute until the Decorator returns a failure or another node interrupts.
			/// </summary>
#endif
			Keep,

#if ARBOR_DOC_JA
			/// <summary>
			/// OnExecuteのイベント呼び出し後に失敗を返す。
			/// </summary>
#else
			/// <summary>
			/// Returns failure after calling the OnExecute event.
			/// </summary>
#endif
			ReturnFailure,

#if ARBOR_DOC_JA
			/// <summary>
			/// OnExecuteのイベント呼び出し後に成功を返す。
			/// </summary>
#else
			/// <summary>
			/// Returns success after calling the OnExecute event.
			/// </summary>
#endif
			ReturnSuccess,
		}

		[System.Serializable]
		public sealed class FlexibleExecuteType : FlexibleField<ExecuteType>
		{
			public FlexibleExecuteType()
			{
			}

			public FlexibleExecuteType(ExecuteType value) : base(value)
			{
			}

			public FlexibleExecuteType(AnyParameterReference parameter) : base(parameter)
			{
			}

			public FlexibleExecuteType(InputSlotAny slot) : base(slot)
			{
			}

			public static explicit operator ExecuteType(FlexibleExecuteType flexible)
			{
				return flexible.value;
			}

			public static explicit operator FlexibleExecuteType(ExecuteType value)
			{
				return new FlexibleExecuteType(value);
			}
		}

		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 実行タイプ。
		/// </summary>
#else
		/// <summary>
		/// Execute type.
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(ExecuteType))]
		private FlexibleExecuteType _ExecuteType = new FlexibleExecuteType(ExecuteType.Keep);

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
		[FormerlySerializedAs("_ExecuteType")]
		private ExecuteType _OldExecuteType = ExecuteType.Keep;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		void Awake()
		{
			LogWarning();
		}

		void Execute(ActionTriggerType triggerType)
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

		protected override void OnAwake()
		{
			Execute(ActionTriggerType.OnAwake);
		}

		protected override void OnStart()
		{
			Execute(ActionTriggerType.OnStart);
		}

		protected override void OnExecute()
		{
			Execute(ActionTriggerType.OnExecute);

			switch (_ExecuteType.value)
			{
				case ExecuteType.Keep:
					// do nothing.
					break;
				case ExecuteType.ReturnFailure:
					FinishExecute(false);
					break;
				case ExecuteType.ReturnSuccess:
					FinishExecute(true);
					break;
			}
		}

		protected override void OnEnd()
		{
			Execute(ActionTriggerType.OnEnd);
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

		void SerializeVer1()
		{
			_ExecuteType = (FlexibleExecuteType)_OldExecuteType;
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

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			Serialize();
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
			Serialize();
		}
	}
}