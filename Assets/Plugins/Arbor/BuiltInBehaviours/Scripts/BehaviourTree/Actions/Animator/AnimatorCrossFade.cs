//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.BehaviourTree.Actions
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
	public sealed class AnimatorCrossFade : AnimatorBase, INodeBehaviourSerializationCallbackReceiver
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
		/// Start time of the current destination state.
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
		/// 遷移完了まで待つかどうか。待たない場合は即時成功を返す。遷移中に他のステートに遷移した場合はその時点で終了する。
		/// </summary>
#else
		/// <summary>
		/// Whether to wait for the transition to complete. If you don't wait, return immediately.　When transitioning to another state during the transition, it ends at that point.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _WaitTransition = new FlexibleBool(false);

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移成功かを判定する。falseの場合は必ず成功を返す。
		/// </summary>
#else
		/// <summary>
		/// Determine if the transition was successful. If false, it always returns success.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _CheckResult = new FlexibleBool(false);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_TransitionDuration")]
		private float _OldTransitionDuration = 0f;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_NormalizedTime")]
		private float _OldTimeOffset = float.NegativeInfinity;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_LayerName")]
		private string _OldLayerName = string.Empty;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_StateName")]
		private string _OldStateName = string.Empty;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 2;

		enum Status
		{
			WaitOneFrame,
			WaitTransition,
			Success,
			Failure,
		}

		private Animator _CacheAnimator;
		private bool _CacheCheckResult;
		private int _LayerIndex;
		private Status _Status;

		private string _CacheLayerName;
		private string _CacheStateName;

		protected override void OnStart()
		{
			base.OnStart();

			_CacheCheckResult = _CheckResult.value;

			_CacheLayerName = _LayerName.value;
			_CacheStateName = _StateName.value;

			_CacheAnimator = cachedAnimator;
			if (_CacheAnimator == null)
			{
				if (_CacheCheckResult)
				{
					_Status = Status.Failure;
				}
				else
				{
					_Status = Status.Success;
				}
				return;
			}

			_LayerIndex = AnimatorUtility.GetLayerIndex(_CacheAnimator, _CacheLayerName);
			if (_LayerIndex < 0)
			{
				if (_CacheCheckResult)
				{
					_Status = Status.Failure;
				}
				else
				{
					_Status = Status.Success;
				}
				return;
			}

			if (!_CacheAnimator.IsInTransition(_LayerIndex))
			{
				var currentState = _CacheAnimator.GetCurrentAnimatorStateInfo(_LayerIndex);
				if (currentState.IsName(_CacheStateName))
				{
					_Status = Status.Success;
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
				if (_WaitTransition.value)
				{
					_Status = Status.WaitOneFrame;
				}
				else
				{
					_Status = Status.Success;
				}
			}
			else
			{
				_Status = Status.Success;
			}
		}

		protected override void OnExecute()
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
								FinishExecute(!_CacheCheckResult);
							}
						}
						else
						{
							if (_CacheCheckResult)
							{
								AnimatorStateInfo currentState = _CacheAnimator.GetCurrentAnimatorStateInfo(_LayerIndex);
								FinishExecute(currentState.IsName(_CacheStateName));
							}
							else
							{
								FinishExecute(true);
							}
						}
					}
					break;
				case Status.Failure:
					{
						FinishExecute(false);
					}
					break;
				case Status.Success:
					{
						FinishExecute(true);
					}
					break;
			}
		}

		private void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
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