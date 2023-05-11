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
	/// Tweenを行う基本クラス
	/// </summary>
#else
	/// <summary>
	/// Base class for Tweening
	/// </summary>
#endif
	[AddComponentMenu("")]
	[HideBehaviour]
	public class TweenBase : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region enum

		[Internal.Documentable]
		public enum Type
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// １回のみ
			/// </summary>
#else
			/// <summary>
			/// Only once
			/// </summary>
#endif
			Once,

#if ARBOR_DOC_JA
			/// <summary>
			/// 繰り返し
			/// </summary>
#else
			/// <summary>
			/// Repeat
			/// </summary>
#endif
			Loop,

#if ARBOR_DOC_JA
			/// <summary>
			/// 終端で折り返し
			/// </summary>
#else
			/// <summary>
			/// Turn back at the end
			/// </summary>
#endif
			PingPong,
		};

		[System.Serializable]
		public sealed class FlexibleTweenType : FlexibleField<Type>
		{
			public FlexibleTweenType()
			{
			}

			public FlexibleTweenType(Type value) : base(value)
			{
			}

			public FlexibleTweenType(AnyParameterReference parameter) : base(parameter)
			{
			}

			public FlexibleTweenType(InputSlotAny slot) : base(slot)
			{
			}

			public static explicit operator Type(FlexibleTweenType flexible)
			{
				return flexible.value;
			}

			public static explicit operator FlexibleTweenType(Type value)
			{
				return new FlexibleTweenType(value);
			}
		}

		#endregion // enum

		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 実行するメソッドのタイプ。
		/// </summary>
		/// <remarks>
		/// Updateを選択: Unityコールバックメソッドで実行する。Tween更新とStateの更新タイミングがずれる点に注意。
		/// StateUpdateを選択: Stateコールバックメソッドで実行する。UpdateSettingsの設定によって毎フレーム呼び出されない点に注意。
		/// </remarks>
#else
		/// <summary>
		/// Execute callback type.
		/// </summary>
		/// <remarks>
		/// Select Update: Execute in the Unity callback method, noting that the timing of the tween update and the state update will be different.
		/// Select StateUpdate: Execute in the State callback method, note that it is not called every frame depending on the setting of UpdateSettings.
		/// </remarks>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(UpdateMethodType))]
		private FlexibleUpdateMethodType _UpdateMethod = new FlexibleUpdateMethodType(UpdateMethodType.Update);

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生タイプ。
		/// </summary>
#else
		/// <summary>
		/// Play type.
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(Type))]
		private FlexibleTweenType _Type = new FlexibleTweenType(Type.Once);

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生時間。
		/// </summary>
#else
		/// <summary>
		/// Playback time.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Duration = new FlexibleFloat(1.0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// 時間に対する適用度の変化曲線
		/// </summary>
#else
		/// <summary>
		/// Change curve of applicability with respect to time
		/// </summary>
#endif
		[SerializeField] private FlexibleAnimationCurve _Curve = new FlexibleAnimationCurve(AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f));

#if ARBOR_DOC_JA
		/// <summary>
		/// Time.timeScaleの影響を受けずリアルタイムに進行するフラグ。
		/// </summary>
#else
		/// <summary>
		/// Flag to progress in real time without the influence of Time.timeScale.
		/// </summary>
#endif
		[SerializeField] private FlexibleBool _UseRealtime = new FlexibleBool(false);

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移するまでの繰り返し回数(Loop、PingPongのみ)
		/// </summary>
#else
		/// <summary>
		/// Number of repetitions until the transition (Loop, PingPong only)
		/// </summary>
#endif
		[SerializeField] private FlexibleInt _RepeatUntilTransition = new FlexibleInt(1);

#if ARBOR_DOC_JA
		/// <summary>
		/// 時間経過後の遷移先。<br/>
		/// (Loop、Pingpongの場合、Repeat Until Transitionで指定した回数だけ繰り返してから遷移する)<br />
		/// 遷移メソッド : Update, FixedUpdate
		/// </summary>
#else
		/// <summary>
		/// Transition destination after time.<br/>
		/// (In the case of Loop and Pingpong, to transition from repeated as many times as specified in the Repeat Until Transition)<br />
		/// Transition Method : Update, FixedUpdate
		/// </summary>
#endif
		[Internal.DocumentOrder(1000)]
		[SerializeField] private StateLink _NextState = new StateLink();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersionBase = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_Duration")]
		[HideInInspector]
		private float _OldDuration = 1.0f;

		[SerializeField]
		[FormerlySerializedAs("_Curve")]
		[HideInInspector]
		private AnimationCurve _OldCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

		[SerializeField]
		[FormerlySerializedAs("_UseRealtime")]
		[HideInInspector]
		private bool _OldUseRealtime = false;

		[SerializeField]
		[FormerlySerializedAs("_RepeatUntilTransition")]
		[HideInInspector]
		private int _OldRepeatUntilTransition = 1;

		[SerializeField]
		[FormerlySerializedAs("_Type")]
		[HideInInspector]
		private Type _OldType = Type.Once;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersionBase = 2;

		public virtual bool fixedUpdate
		{
			get
			{
				return false;
			}
		}

		public virtual bool forceRealtime
		{
			get
			{
				return false;
			}
		}

		private AnimationCurve _CurrentCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
		private bool _CurrentUseRealtime = false;
		private int _CurrentRepeatUntilTransition = 1;

		private float _FromAdvance = 0.0f;
		private float _ToAdvance = 1.0f;

		public int repeatUntilTransition
		{
			get
			{
				return _CurrentRepeatUntilTransition;
			}
		}

		private int _RepeatCount = 0;
		public int repeatCount
		{
			get
			{
				return _RepeatCount;
			}
		}

		TimeType GetTimeType()
		{
			if (fixedUpdate)
			{
				return TimeType.FixedTime;
			}

			if (_CurrentUseRealtime)
			{
				return TimeType.Realtime;
			}

			return TimeType.Normal;
		}

		private System.Action _OnComplete;

		public override void OnStateAwake()
		{
			base.OnStateAwake();

			schedulerUpdateTiming = SchedularUpdateTiming.Manual;
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			_CurrentCurve = _Curve.value;
			if (_CurrentCurve == null)
			{
				_CurrentCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
			}
			_CurrentUseRealtime = forceRealtime || _UseRealtime.value;
			_CurrentRepeatUntilTransition = _RepeatUntilTransition.value;

			_FromAdvance = 0.0f;
			_ToAdvance = 1.0f;
			_RepeatCount = 0;

			OnTweenBegin();

			var scheduler = GetOrCreateScheduler();
			if (_OnComplete == null)
			{
				_OnComplete = OnComplete;
			}
			scheduler.onComplete -= _OnComplete;
			scheduler.onComplete += _OnComplete;

			AddTask(scheduler);

			scheduler.Play();
			
			OnProgress(0.0f);
		}

		void AddTask(TaskScheduler scheduler)
		{
			using (var timerTask = TimerTask.GetPooled(GetTimeType(), _Duration.value))
			{
				timerTask.onProgress += OnProgress;
				scheduler.Add(timerTask);
			}
		}

		protected virtual void OnTweenBegin()
		{
		}
		protected virtual void OnTweenUpdate(float factor)
		{
		}

		void OnProgress(float t)
		{
			float factor = Mathf.Lerp(_FromAdvance, _ToAdvance, Mathf.Clamp01(t));
			float curveFactor = _CurrentCurve.Evaluate(factor);

			OnTweenUpdate(curveFactor);
		}

		void OnComplete()
		{
			Type type = _Type.value;
			switch (type)
			{
				case Type.Once:
					break;
				case Type.Loop:
					{
						var scheduler = this.scheduler;
						scheduler.Stop();
						AddTask(scheduler);
						scheduler.Play();
					}
					break;
				case Type.PingPong:
					{
						float temp = _FromAdvance;
						_FromAdvance = _ToAdvance;
						_ToAdvance = temp;

						var scheduler = this.scheduler;
						scheduler.Stop();
						AddTask(scheduler);
						scheduler.Play();
					}
					break;
			}

			if (type == Type.Once)
			{
				Transition(_NextState);
			}
			else
			{
				_RepeatCount++;
				if (_RepeatCount >= _CurrentRepeatUntilTransition)
				{
					Transition(_NextState);
				}
			}
		}

		void DoUpdate(UpdateMethodType updateMethodType, bool fixedUpdate)
		{
			if (_UpdateMethod.value != updateMethodType || this.fixedUpdate != fixedUpdate)
			{
				return;
			}

			scheduler?.Update();
		}

		[Internal.ExcludeTest]
		protected virtual void Update()
		{
			using (CalculateScope.OpenScope())
			{
				DoUpdate(UpdateMethodType.Update, false);
			}
		}

		[Internal.ExcludeTest]
		protected virtual void FixedUpdate()
		{
			using (CalculateScope.OpenScope())
			{
				DoUpdate(UpdateMethodType.Update, true);
			}
		}

		// Update is called once per frame
		public override void OnStateUpdate()
		{
			DoUpdate(UpdateMethodType.StateUpdate, false);
		}

		public override void OnStateFixedUpdate()
		{
			DoUpdate(UpdateMethodType.StateUpdate, true);
		}

		protected virtual void Reset()
		{
			_SerializeVersionBase = kCurrentSerializeVersionBase;

			_UpdateMethod = new FlexibleUpdateMethodType(UpdateMethodType.StateUpdate);
		}

		void SerializeVer1()
		{
			_Duration = (FlexibleFloat)_OldDuration;
			_Curve = (FlexibleAnimationCurve)_OldCurve;
			_UseRealtime = (FlexibleBool)_OldUseRealtime;
			_RepeatUntilTransition = (FlexibleInt)_OldRepeatUntilTransition;
		}

		void SerializeVer2()
		{
			_Type = (FlexibleTweenType)_OldType;
		}

		void Serialize()
		{
			while (_SerializeVersionBase != kCurrentSerializeVersionBase)
			{
				switch (_SerializeVersionBase)
				{
					case 0:
						SerializeVer1();
						_SerializeVersionBase++;
						break;
					case 1:
						SerializeVer2();
						_SerializeVersionBase++;
						break;
					default:
						_SerializeVersionBase = kCurrentSerializeVersionBase;
						break;
				}
			}
		}

		public virtual void OnBeforeSerialize()
		{
			Serialize();
		}

		public virtual void OnAfterDeserialize()
		{
			Serialize();
		}
	}
}
