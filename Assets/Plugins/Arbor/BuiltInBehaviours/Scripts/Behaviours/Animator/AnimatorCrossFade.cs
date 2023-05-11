//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
	using Arbor.Utilities;

#if ARBOR_DOC_JA
	/// <summary>
	/// Animatorのステートを遷移させる。
	/// </summary>
#else
	/// <summary>
	/// Transit the state of Animator.
	/// </summary>
	/// <remarks></remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Animator/AnimatorCrossFade")]
	[BuiltInBehaviour]
	public sealed class AnimatorCrossFade : AnimatorBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// レイヤー名。
		/// </summary>
#else
		/// <summary>
		/// Layer Name
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _LayerName = new FlexibleString(string.Empty);

#if ARBOR_DOC_JA
		/// <summary>
		/// ステート名。
		/// </summary>
#else
		/// <summary>
		/// State Name
		/// </summary>
#endif
		[SerializeField]
		[FormerlySerializedAs("stateName")]
		private FlexibleString _StateName = new FlexibleString(string.Empty);

#if ARBOR_DOC_JA
		/// <summary>
		/// Animator.CrossFadeInFixedTime()を使用するフラグ。falseの場合はCrossFade()を使用する。
		/// </summary>
#else
		/// <summary>
		/// Flag that uses Animator.CrossFadeInFixedTime(). If false, use CrossFade().
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _InFixedTime = new FlexibleBool(false);

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移の継続時間
		/// </summary>
#else
		/// <summary>
		/// Transition duration
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _TransitionDuration = new FlexibleFloat(0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// 現在の遷移先のステートの開始時間。
		/// </summary>
#else
		/// <summary>
		/// The start time of the current transition destination state.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _TimeOffset = new FlexibleFloat(float.NegativeInfinity);

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定したステートへ遷移中かチェックする。もし遷移中であればCrossFadeを処理しない。
		/// </summary>
#else
		/// <summary>
		/// Check if it is in transition to the specified state. If it is in transition, do not process CrossFade.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _CheckInTransition = new FlexibleBool(false);

#if ARBOR_DOC_JA
		/// <summary>
		/// Animatorの遷移が完了した時のステート遷移。
		/// </summary>
#else
		/// <summary>
		/// State transition when the Animator transition is completed.
		/// </summary>
#endif
		[SerializeField]
		private StateLink _SuccessLink = new StateLink();

#if ARBOR_DOC_JA
		/// <summary>
		/// Animatorの遷移が失敗した時のステート遷移。
		/// </summary>
#else
		/// <summary>
		/// State transition when Animator transition fails.
		/// </summary>
#endif
		[SerializeField]
		private StateLink _FailureLink = new StateLink();

		[SerializeField]
		[HideInInspector]
		private int _CrossFade_SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("transitionDuration")]
		[FormerlySerializedAs("_TransitionDuration")]
		private float _OldTransitionDuration = 0f;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("normalizedTime")]
		[FormerlySerializedAs("_NormalizedTime")]
		private float _OldTimeOffset = float.NegativeInfinity;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("layerName")]
		[FormerlySerializedAs("_LayerName")]
		private string _OldLayerName = string.Empty;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("stateName")]
		[FormerlySerializedAs("_StateName")]
		private string _OldStateName = string.Empty;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 2;

		enum Status
		{
			Nothing,
			WaitOneFrame,
			WaitTransition,
		}

		private Animator _CacheAnimator;
		private Status _Status = Status.Nothing;
		private int _LayerIndex;

		private string _CacheLayerName;
		private string _CacheStateName;

		// Use this for enter state
		public override void OnStateBegin()
		{
			_Status = Status.Nothing;

			_CacheLayerName = _LayerName.value;
			_CacheStateName = _StateName.value;

			_CacheAnimator = cachedAnimator;
			if (_CacheAnimator == null)
			{
				Transition(_FailureLink);
				return;
			}

			_LayerIndex = AnimatorUtility.GetLayerIndex(_CacheAnimator, _CacheLayerName);
			if (_LayerIndex < 0)
			{
				Transition(_FailureLink);
				return;
			}

			if (!_CacheAnimator.IsInTransition(_LayerIndex))
			{
				var currentState = _CacheAnimator.GetCurrentAnimatorStateInfo(_LayerIndex);
				if (currentState.IsName(_CacheStateName))
				{
					Transition(_SuccessLink);
					return;
				}
			}

			bool doCrossFade = !(_CheckInTransition.value && AnimatorUtility.IsInTransition(_CacheAnimator, _LayerIndex, _CacheStateName));
			if (doCrossFade)
			{
				if (_InFixedTime.value)
				{
					_CacheAnimator.CrossFadeInFixedTime(_CacheStateName, _TransitionDuration.value, _LayerIndex, _TimeOffset.value);
				}
				else
				{
					_CacheAnimator.CrossFade(_CacheStateName, _TransitionDuration.value, _LayerIndex, _TimeOffset.value);
				}
				_Status = Status.WaitOneFrame;
			}
			else
			{
				Transition(_SuccessLink);
				return;
			}
		}

		public override void OnStateUpdate()
		{
			switch (_Status)
			{
				case Status.WaitOneFrame:
					{
						_Status = Status.WaitTransition;
					}
					break;
				case Status.WaitTransition:
					{
						if (_CacheAnimator.IsInTransition(_LayerIndex))
						{
							AnimatorStateInfo nextState = _CacheAnimator.GetNextAnimatorStateInfo(_LayerIndex);
							if (!nextState.IsName(_CacheStateName))
							{
								Transition(_FailureLink);
								_Status = Status.Nothing;
							}
						}
						else
						{
							AnimatorStateInfo currentState = _CacheAnimator.GetCurrentAnimatorStateInfo(_LayerIndex);
							if (currentState.IsName(_CacheStateName))
							{
								Transition(_SuccessLink);
							}
							else
							{
								Transition(_FailureLink);
							}

							_Status = Status.Nothing;
						}
					}
					break;
			}
		}

		protected override void Reset()
		{
			base.Reset();

			_CrossFade_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_TransitionDuration = new FlexibleFloat(_OldTransitionDuration);
			_TimeOffset = new FlexibleFloat(_OldTimeOffset);
		}

		void SerializeVer2()
		{
			_LayerName = (FlexibleString)_OldLayerName;
			_StateName = (FlexibleString)_OldStateName;
		}

		void Serialize()
		{
			while (_CrossFade_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_CrossFade_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_CrossFade_SerializeVersion++;
						break;
					case 1:
						SerializeVer2();
						_CrossFade_SerializeVersion++;
						break;
					default:
						_CrossFade_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
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