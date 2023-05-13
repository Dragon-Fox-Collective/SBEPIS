//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	using Arbor.Utilities;

#if ARBOR_DOC_JA
	/// <summary>
	/// Animatorのbool型パラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// A reference to the Boolean parameter of Animator.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.AnimatorParameterType(AnimatorControllerParameterType.Trigger)]
	public sealed class AnimatorTriggerParameterReference : AnimatorParameterReference
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// トリガーをセットする。
		/// </summary>
#else
		/// <summary>
		/// Set the trigger.
		/// </summary>
#endif
		public void Set()
		{
			Animator animator = this.animator;
			if (animator != null && nameHash.HasValue && AnimatorUtility.CheckExistsParameter(animator, name))
			{
				animator.SetTrigger(nameHash.Value);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// トリガーをリセットする。
		/// </summary>
#else
		/// <summary>
		/// Reset the trigger.
		/// </summary>
#endif
		public void Reset()
		{
			Animator animator = this.animator;
			if (animator != null && nameHash.HasValue && AnimatorUtility.CheckExistsParameter(animator, name))
			{
				animator.ResetTrigger(nameHash.Value);
			}
		}
	}
}
