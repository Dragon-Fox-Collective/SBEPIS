//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.BehaviourTree.Actions
{
	using Arbor.TaskSystem;

#if ARBOR_DOC_JA
	/// <summary>
	/// 時間経過を待つ
	/// </summary>
#else
	/// <summary>
	/// Wait for the passage of time
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Wait")]
	[BuiltInBehaviour]
	public sealed class Wait : ActionBehaviour, INodeBehaviourSerializationCallbackReceiver
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
		/// 待つ秒数
		/// </summary>
#else
		/// <summary>
		/// Number of seconds to wait
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

		private System.Action _OnComplete;

		protected override void OnStart()
		{
			var scheduler = GetOrCreateScheduler();
			if (_OnComplete == null)
			{
				_OnComplete = OnComplete;
			}
			scheduler.onComplete -= _OnComplete;
			scheduler.onComplete += _OnComplete;

			using (var timerTask = TimerTask.GetPooled(_TimeType.value, _Seconds.value))
			{
				scheduler.Add(timerTask);
			}

			scheduler.Play();
		}

		void OnComplete()
		{
			FinishExecute(true);
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