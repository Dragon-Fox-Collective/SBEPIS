//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.BehaviourTree.Decorators
{
	using Arbor.TaskSystem;

#if ARBOR_DOC_JA
	/// <summary>
	/// Cooldownが追加されているノードが終了したタイミングから指定時間経過後に再度アクティブにする
	/// </summary>
#else
	/// <summary>
	/// Reactivate again after the specified time has elapsed from the timing when Cooldown is exited from the added node.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Cooldown")]
	[BuiltInBehaviour]
	public sealed class Cooldown : Decorator, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 時間の種類。
		/// </summary>
#else
		/// <summary>
		/// Type of time.
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(TimeType))]
		private FlexibleTimeType _TimeType = new FlexibleTimeType(TimeType.Normal);

#if ARBOR_DOC_JA
		/// <summary>
		/// クールダウンの秒数
		/// </summary>
#else
		/// <summary>
		/// Cool down seconds
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Seconds = new FlexibleFloat();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_TimeType")]
		private TimeType _OldTimeType = TimeType.Normal;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		protected override bool OnConditionCheck()
		{
			if (!treeNode.isActive)
			{
				var scheduler = this.scheduler;
				if (scheduler != null)
				{
					scheduler.Update();
					return scheduler.taskStatus == TaskStatus.Complete;
				}
			}

			return true;
		}

		protected override void OnAwake()
		{
			base.OnAwake();

			schedulerUpdateTiming = SchedularUpdateTiming.Manual;
			schedulerDestroyTiming = SchedulerDestroyTiming.OnStart;
		}

		protected override void OnEnd()
		{
			var scheduler = GetOrCreateScheduler();

			using (var timerTask = TimerTask.GetPooled(_TimeType.value, _Seconds.value))
			{
				scheduler.Add(timerTask);
			}

			scheduler.Play();
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_TimeType = (FlexibleTimeType)_OldTimeType;
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