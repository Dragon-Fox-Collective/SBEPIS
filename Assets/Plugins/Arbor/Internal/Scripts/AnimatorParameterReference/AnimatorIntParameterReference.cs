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
	/// Animatorのint型パラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// A reference to the int parameter of Animator.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.AnimatorParameterType(AnimatorControllerParameterType.Int)]
	public sealed class AnimatorIntParameterReference : AnimatorParameterReference
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
		public void Set(int value)
		{
			Animator animator = this.animator;
			if (animator != null && nameHash.HasValue && AnimatorUtility.CheckExistsParameter(animator, name))
			{
				animator.SetInteger(nameHash.Value, value);
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
		public int Get()
		{
			Animator animator = this.animator;
			if (animator != null && nameHash.HasValue && AnimatorUtility.CheckExistsParameter(animator, name))
			{
				return animator.GetInteger(nameHash.Value);
			}
			return 0;
		}
	}
}
