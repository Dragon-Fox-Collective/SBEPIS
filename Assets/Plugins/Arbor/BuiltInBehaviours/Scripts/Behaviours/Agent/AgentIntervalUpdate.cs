//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
	[AddComponentMenu("")]
	[HideBehaviour()]
	public abstract class AgentIntervalUpdate : AgentUpdateBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// Agentの更新タイプ。
		/// </summary>
#else
		/// <summary>
		/// Agent update type.
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(AgentUpdateType))]
		private FlexibleAgentUpdateType _UpdateType = new FlexibleAgentUpdateType(AgentUpdateType.Time);

#if ARBOR_DOC_JA
		/// <summary>
		/// Intervalの時間タイプ。
		/// </summary>
#else
		/// <summary>
		/// Interval time type.
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(TimeType))]
		private FlexibleTimeType _TimeType = new FlexibleTimeType(TimeType.Normal);

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動先を変更するまでのインターバル(秒)。(UpdateTypeがTime、Doneの時のみ使用) <br />
		/// AgentUpdateType.Doneの場合は到達後のインターバル。
		/// </summary>
#else
		/// <summary>
		/// Interval (seconds) before moving destination is changed. (Used only when UpdateType is Time, Done)<br />
		/// The interval after arrival for AgentUpdateType.Done.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Interval = new FlexibleFloat();

		[SerializeField]
		[HideInInspector]
		private int _IntervalUpdate_SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		private float _MinInterval = 0f;

		[SerializeField]
		[HideInInspector]
		private float _MaxInterval = 0f;

		[SerializeField]
		[FormerlySerializedAs("_UpdateType")]
		[HideInInspector]
		private AgentUpdateType _OldUpdateType = AgentUpdateType.Time;

		[SerializeField]
		[FormerlySerializedAs("_TimeType")]
		[HideInInspector]
		private TimeType _OldTimeType = TimeType.Normal;

		[SerializeField]
		[FormerlySerializedAs("_StopOnStateEnd")]
		[HideInInspector]
		private bool _OldStopOnStateEnd = false;

		#endregion // old

		#endregion // Serialize fields

		private const int kIntervalUpdate_SerializeVersion = 2;

		private bool _IsStartExecuted = false;
		private Timer _Timer = new Timer();
		private float _NextInterval = 0f;
		private bool _IsDone = false;

		private AgentUpdateType _CacheUpdateType;
		
		// Use this for enter state
		public override void OnStateBegin()
		{
			_IsStartExecuted = false;

			_CacheUpdateType = _UpdateType.value;

			_Timer.timeType = _TimeType.value;
			_Timer.Stop();
			_NextInterval = 0f;
			_IsDone = false;

			base.OnStateBegin();
		}

		// Use this for exit state
		public override void OnStateEnd()
		{
			base.OnStateEnd();

			_Timer.Stop();
		}

		protected override void OnGraphPause()
		{
			_Timer.Pause();
		}

		protected override void OnGraphResume()
		{
			_Timer.Resume();
		}

		protected abstract bool OnIntervalUpdate(AgentController agentController);

		protected sealed override bool OnUpdateAgent(AgentController agentController)
		{
			var updateType = _UpdateType.value;
			if (updateType != _CacheUpdateType)
			{
				_IsDone = false;
				switch (updateType)
				{
					case AgentUpdateType.Time:
						if (_Timer.playState == Timer.PlayState.Stopping)
						{
							_Timer.Start();
							_NextInterval = _Interval.value;
						}
						break;
				}

				_CacheUpdateType = updateType;
			}

			_Timer.timeType = _TimeType.value;

			bool result = true;

			switch (_CacheUpdateType)
			{
				case AgentUpdateType.Time:
					{
						if (!_IsStartExecuted || _Timer.elapsedTime >= _NextInterval)
						{
							result = OnIntervalUpdate(agentController);
							_Timer.Stop();
							_Timer.Start();
							_NextInterval = _Interval.value;

							_IsStartExecuted = true;
						}
					}
					break;
				case AgentUpdateType.Done:
					{
						if (_IsStartExecuted)
						{
							if (!_IsDone)
							{
								if (agentController.isDone)
								{
									_IsDone = true;
									_Timer.Stop();
									_Timer.Start();
									_NextInterval = _Interval.value;
								}
								else
								{
									_IsDone = false;
								}
							}
						}
						else
						{
							result = OnIntervalUpdate(agentController);
							_IsDone = false;
							_NextInterval = 0f;

							_IsStartExecuted = true;
						}

						if (_IsDone)
						{
							if (_Timer.elapsedTime >= _NextInterval)
							{
								result = OnIntervalUpdate(agentController);
								_Timer.Stop();
								_IsDone = false;
							}
						}
					}
					break;
				case AgentUpdateType.StartOnly:
					if (!_IsStartExecuted)
					{
						result = OnIntervalUpdate(agentController);
						_IsStartExecuted = true;
					}
					break;
				case AgentUpdateType.Always:
					{
						result = OnIntervalUpdate(agentController);

						_IsStartExecuted = true;
					}
					break;
			}

			return result;
		}

		void SerializeVer1()
		{
			_Interval = new FlexibleFloat(_MinInterval, _MaxInterval);
		}

		void SerializeVer2()
		{
			_UpdateType = (FlexibleAgentUpdateType)_OldUpdateType;
			_TimeType = (FlexibleTimeType)_OldTimeType;
			_StopOnStateEnd = (FlexibleBool)_OldStopOnStateEnd;
		}

		void Serialize()
		{
			while (_IntervalUpdate_SerializeVersion != kIntervalUpdate_SerializeVersion)
			{
				switch (_IntervalUpdate_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_IntervalUpdate_SerializeVersion++;
						break;
					case 1:
						SerializeVer2();
						_IntervalUpdate_SerializeVersion++;
						break;
					default:
						_IntervalUpdate_SerializeVersion = kIntervalUpdate_SerializeVersion;
						break;
				}
			}
		}

		protected override void Reset()
		{
			base.Reset();

			_IntervalUpdate_SerializeVersion = kIntervalUpdate_SerializeVersion;
		}

		public override void OnBeforeSerialize()
		{
			base.OnBeforeSerialize();

			Serialize();
		}

		public override void OnAfterDeserialize()
		{
			base.OnAfterDeserialize();

			Serialize();
		}
	}
}