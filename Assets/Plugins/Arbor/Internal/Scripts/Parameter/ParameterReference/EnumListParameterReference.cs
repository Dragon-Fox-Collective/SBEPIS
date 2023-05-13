//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// EnumListパラメータの参照。
	/// </summary>
	/// <remarks>
	/// 使用可能な属性 : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeConstraintAttribute" /></description></item>
	/// <item><description><see cref="SlotTypeAttribute" /></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// Reference EnumList parameters.
	/// </summary>
	/// <remarks>
	/// Available Attributes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeConstraintAttribute" /></description></item>
	/// <item><description><see cref="SlotTypeAttribute" /></description></item>
	/// </list>
	/// </remarks>
#endif
	[System.Serializable]
	[Internal.ConstraintableEnum(true)]
	public sealed class EnumListParameterReference : ParameterReference
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 値を取得する。
		/// </summary>
		/// <typeparam name="TEnum">enumの型</typeparam>
		/// <returns>値。取得できなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value.
		/// </summary>
		/// <typeparam name="TEnum">enum type</typeparam>
		/// <returns>value. If it cannot be obtained, it returns null.</returns>
#endif
		public IList<TEnum> GetValue<TEnum>() where TEnum : System.Enum
		{
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.GetEnumList<TEnum>();
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を設定する。
		/// </summary>
		/// <typeparam name="TEnum">enumの型</typeparam>
		/// <param name="value">設定する値</param>
		/// <returns>設定出来た場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Set the value.
		/// </summary>
		/// <typeparam name="TEnum">enum type</typeparam>
		/// <param name="value">Value to set</param>
		/// <returns>Returns true if it can be set.</returns>
#endif
		public bool SetValue<TEnum>(IList<TEnum> value) where TEnum : System.Enum
		{
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.SetEnumList<TEnum>(value);
			}

			return false;
		}
	}
}
