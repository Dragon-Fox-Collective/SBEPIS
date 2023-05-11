//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
	using Arbor.TaskSystem;

#if ARBOR_DOC_JA
	/// <summary>
	/// 時間経過後にステートを遷移する。
	/// </summary>
#else
	/// <summary>
	/// It will transition the state after the lapse of time.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/TimeTransition")]
	[BuiltInBehaviour]
	public sealed class TimeTransition : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
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
		/// 遷移するまでの秒数。
		/// </summary>
#else
		/// <summary>
		/// The number of seconds until the transition.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Seconds = new FlexibleFloat(0.0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先ステート。<br />
		/// 遷移メソッド : OnStateUpdate
		/// </summary>
#else
		/// <summary>
		/// Transition destination state.<br />
		/// Transition Method : OnStateUpdate
		/// </summary>
#endif
		[SerializeField]
		private StateLink _NextState = new StateLink();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[FormerlySerializedAs("_Seconds")]
		[SerializeField]
		[HideInInspector]
		private float _OldSeconds = 0;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_TimeType")]
		private TimeType _OldTimeType = TimeType.Normal;

		#endregion

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 2;

		private System.Action _OnComplete;

		public override void OnStateBegin()
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
			Transition(_NextState);
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Seconds = (FlexibleFloat)_OldSeconds;
		}

		void SerializeVer2()
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
					case 1:
						SerializeVer2();
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
