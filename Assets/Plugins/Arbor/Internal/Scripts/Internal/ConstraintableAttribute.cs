//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;

namespace Arbor.Internal
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ParameterReferenceの派生クラスに制約可能であることを指定する属性
	/// </summary>
#else
	/// <summary>
	/// Attribute that specifies that it can be constrained to a derived class of ParameterReference
	/// </summary>
#endif
	[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class ConstraintableAttribute : ParameterConstraintAttributeBase, IConstraintableAttribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 制約の基本型
		/// </summary>
#else
		/// <summary>
		/// Base type of constraint
		/// </summary>
#endif
		public System.Type baseType
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// IList&lt;&gt;のみに制約するフラグ。
		/// </summary>
#else
		/// <summary>
		/// Flag that constrains to IList&lt;&gt; only.
		/// </summary>
#endif
		public bool isList = false;

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約可能であることを指定する
		/// </summary>
#else
		/// <summary>
		/// Specify that it can be constrained
		/// </summary>
#endif
		public ConstraintableAttribute()
		{
			this.baseType = null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約可能であることを指定する
		/// </summary>
		/// <param name="baseType">制約の基本型</param>
#else
		/// <summary>
		/// Specify that it can be constrained
		/// </summary>
		/// <param name="baseType">Base type of constraint</param>
#endif
		public ConstraintableAttribute(System.Type baseType)
		{
			this.baseType = baseType;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約を満たすか判定する
		/// </summary>
		/// <param name="valueType">判定する型</param>
		/// <returns>制約を満たしているときtrueを返す。</returns>
#else
		/// <summary>
		/// Determine whether the constraint is satisfied
		/// </summary>
		/// <param name="valueType">Determining type</param>
		/// <returns>Returns true if the constraint is satisfied.</returns>
#endif
		public bool IsConstraintSatisfied(System.Type valueType)
		{
			if (valueType == null)
			{
				return false;
			}

			if (isList)
			{
				if (!TypeUtility.IsGeneric(valueType, typeof(IList<>)))
				{
					return false;
				}
			}

			if (baseType == null)
			{
				return true;
			}

			if (isList)
			{
				var elementType = TypeUtility.GetGenericArguments(valueType)[0];
				return TypeUtility.IsAssignableFrom(baseType, elementType);
			}
			else
			{
				return TypeUtility.IsAssignableFrom(baseType, valueType);
			}
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
			return IsConstraintSatisfied(parameter.valueType);
		}
	}
}