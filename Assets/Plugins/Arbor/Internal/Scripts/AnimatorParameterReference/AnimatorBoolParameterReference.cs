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
	[Internal.AnimatorParameterType(AnimatorControllerParameterType.Bool)]
	public sealed class AnimatorBoolParameterReference : AnimatorParameterReference
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 値をセットする。
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// Set the value.
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public void Set(bool value)
		{
			Animator animator = this.animator;
			if (animator != null && nameHash.HasValue && AnimatorUtility.CheckExistsParameter(animator, name))
			{
				animator.SetBool(nameHash.Value, value);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を取得する。
		/// </summary>
		/// <returns>値</returns>
#else
		/// <summary>
		/// Get the value.
		/// </summary>
		/// <returns>Value</returns>
#endif
		public bool Get()
		{
			Animator animator = this.animator;
			if (animator != null && nameHash.HasValue && AnimatorUtility.CheckExistsParameter(animator, name))
			{
				return animator.GetBool(nameHash.Value);
			}
			return false;
		}
	}
}
