//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor.Internal
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ParameterReferenceに関連付けするための内部インターフェイス。
	/// </summary>
#else
	/// <summary>
	/// Inner interface for associating with ParameterReference.
	/// </summary>
#endif
	public interface IConstraintableAttribute
	{
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
		bool IsConstraintSatisfied(System.Type valueType);
	}
}