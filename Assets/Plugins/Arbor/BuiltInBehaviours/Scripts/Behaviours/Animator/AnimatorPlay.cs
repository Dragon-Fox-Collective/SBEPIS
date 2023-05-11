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
	/// Animatorのステートを再生する。
	/// </summary>
#else
	/// <summary>
	/// Play the state of Animator.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Animator/AnimatorPlay")]
	[BuiltInBehaviour]
	public sealed class AnimatorPlay : AnimatorBase
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
		/// ステート名
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
		/// Animator.PlayInFixedTime()を使用するフラグ。falseの場合はPlay()を使用する。
		/// </summary>
#else
		/// <summary>
		/// Flag that uses Animator.PlayInFixedTime(). If false, use Play().
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _InFixedTime = new FlexibleBool(false);

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

		#endregion // Serialize fields

		// Use this for enter state
		public override void OnStateBegin()
		{
			Animator animator = cachedAnimator;
			if (animator != null)
			{
				if (_InFixedTime.value)
				{
					animator.PlayInFixedTime(_StateName.value, AnimatorUtility.GetLayerIndex(animator, _LayerName.value), _TimeOffset.value);
				}
				else
				{
					animator.Play(_StateName.value, AnimatorUtility.GetLayerIndex(animator, _LayerName.value), _TimeOffset.value);
				}
			}
		}
	}
}