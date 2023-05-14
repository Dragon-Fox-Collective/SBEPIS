//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor.Internal
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ParameterReferenceに関連付けするための内部クラス。
	/// </summary>
#else
	/// <summary>
	/// Inner class for associating with ParameterReference.
	/// </summary>
#endif
	public abstract class ParameterConstraintAttributeBase : System.Attribute
	{
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
		public abstract bool IsConstraintSatisfied(Parameter parameter);
	}
}