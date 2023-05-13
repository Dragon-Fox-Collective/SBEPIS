//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor.Internal
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ParameterReferenceの派生クラスにParameter.Typeを指定する属性
	/// </summary>
#else
	/// <summary>
	/// An attribute that specifies Parameter.Type as a derived class of ParameterReference
	/// </summary>
#endif
	[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class ParameterTypeAttribute : ParameterConstraintAttributeBase
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 指定されたParameter.Type
		/// </summary>
#else
		/// <summary>
		/// The specified Parameter.Type
		/// </summary>
#endif
		public Parameter.Type parameterType
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Parameter.Typeの指定
		/// </summary>
		/// <param name="parameterType">Parameter.Type</param>
#else
		/// <summary>
		/// Specifying Parameter.Type
		/// </summary>
		/// <param name="parameterType">Parameter.Type</param>
#endif
		public ParameterTypeAttribute(Parameter.Type parameterType)
		{
			this.parameterType = parameterType;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約を満たすか判定する
		/// </summary>
		/// <param name="parameter">判定するパラメータ</param>
		/// <returns>制約を満たしているときtrueを返す。</returns>
#else
		/// <summary>
		/// Determine whether the constraint is satisfied
		/// </summary>
		/// <param name="parameter">Determining parameter</param>
		/// <returns>Returns true if the constraint is satisfied.</returns>
#endif
		public override bool IsConstraintSatisfied(Parameter parameter)
		{
			return parameterType == parameter.type;
		}
	}
}