//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;


namespace Arbor.BehaviourTree.Actions
{
	[AddComponentMenu("")]
	[HideBehaviour]
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
		protected FlexibleAgentUpdateType _UpdateType = new FlexibleAgentUpdateType(AgentUpdateType.Time);

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
		protected FlexibleTimeType _TimeType = new FlexibleTimeType(TimeType.Normal);

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

		#endregion // Serialize fields

		protected abstract bool OnIntervalUpdate(AgentController agentController);

		private bool _IsStartExecuted = false;
		private Timer _Timer = new Timer();
		private float _NextInterval = 0f;
		private bool _IsDone = false;

		private AgentUpdateType _CacheUpdateType;

		protected override void OnStart()
		{
			base.OnStart();

			_IsStartExecuted = false;

			_CacheUpdateType = _UpdateType.value;

			_Timer.timeType = _TimeType.value;
			_Timer.Stop();
			_NextInterval = 0f;
			_IsDone = false;
		}

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

		protected override void OnEnd()
		{
			base.OnEnd();

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
	}
}