//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Internal
{
#if ARBOR_DOC_JA
	/// <summary>
	/// AnimatorParameterReferenceの派生クラスにAnimatorControllerParameterTypeを指定する属性
	/// </summary>
#else
	/// <summary>
	/// An attribute that specifies AnimatorControllerParameterType as a derived class of AnimatorParameterReference
	/// </summary>
#endif
	[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class AnimatorParameterTypeAttribute : System.Attribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 指定されたAnimatorControllerParameterType
		/// </summary>
#else
		/// <summary>
		/// The specified AnimatorControllerParameterType
		/// </summary>
#endif
		public AnimatorControllerParameterType parameterType
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AnimatorControllerParameterTypeの指定
		/// </summary>
		/// <param name="parameterType">AnimatorControllerParameterType</param>
#else
		/// <summary>
		/// Specifying AnimatorControllerParameterType
		/// </summary>
		/// <param name="parameterType">AnimatorControllerParameterType</param>
#endif
		public AnimatorParameterTypeAttribute(AnimatorControllerParameterType parameterType)
		{
			this.parameterType = parameterType;
		}
	}
}