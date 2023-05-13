//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;

namespace Arbor
{
	public sealed partial class AnyParameterReference
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// EnumListの値を設定
		/// </summary>
		/// <typeparam name="TEnum">設定するenumの型</typeparam>
		/// <param name="value">値</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Set EnumList value
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to set</typeparam>
		/// <param name="value">Value</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetEnumList<TEnum>(IList<TEnum> value) where TEnum : System.Enum
		{
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.SetEnumList<TEnum>(value);
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Enum型の値を取得する。
		/// </summary>
		/// <typeparam name="TEnum">取得するenumの型</typeparam>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to get</typeparam>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<TEnum> GetEnumList<TEnum>() where TEnum : System.Enum
		{
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.GetEnumList<TEnum>();
			}

			return null;
		}
	}
}