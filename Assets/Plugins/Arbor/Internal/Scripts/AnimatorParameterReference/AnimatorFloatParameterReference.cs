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
	/// Animatorのfloat型パラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// A reference to the float parameter of Animator.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.AnimatorParameterType(AnimatorControllerParameterType.Float)]
	public sealed class AnimatorFloatParameterReference : AnimatorParameterReference
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
		public void Set(float value)
		{
			Animator animator = this.animator;
			if (animator != null && nameHash.HasValue && AnimatorUtility.CheckExistsParameter(animator, name))
			{
				animator.SetFloat(nameHash.Value, value);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値をセットする。
		/// </summary>
		/// <param name="value">値</param>
		/// <param name="dampTime">ダンプ時間</param>
		/// <param name="deltaTime">ダンプの更新時間</param>
#else
		/// <summary>
		/// Set the value.
		/// </summary>
		/// <param name="value">Value</param>
		/// <param name="dampTime">Damp Time</param>
		/// <param name="deltaTime">Delta Time</param>
#endif
		public void Set(float value, float dampTime, float deltaTime)
		{
			Animator animator = this.animator;
			if (animator != null && nameHash.HasValue && AnimatorUtility.CheckExistsParameter(animator, name))
			{
				animator.SetFloat(nameHash.Value, value, dampTime, deltaTime);
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
		public float Get()
		{
			Animator animator = this.animator;
			if (animator != null && nameHash.HasValue && AnimatorUtility.CheckExistsParameter(animator, name))
			{
				return animator.GetFloat(nameHash.Value);
			}
			return 0.0f;
		}
	}
}
