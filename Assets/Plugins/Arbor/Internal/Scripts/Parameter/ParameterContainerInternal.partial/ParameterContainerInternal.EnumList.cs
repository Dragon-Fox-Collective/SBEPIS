//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
	using Internal;

	public partial class ParameterContainerInternal : ParameterContainerBase, ISerializationCallbackReceiver
	{
		[SerializeField]
		[HideInDocument]
		internal List<EnumListParameter> _EnumListParameters = new List<EnumListParameter>();

		#region EnumList

		private bool SetEnumList<TEnum>(Parameter parameter, IList<TEnum> value) where TEnum : System.Enum
		{
			return parameter != null && parameter.SetEnumList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// EnumList型の値を設定する。
		/// </summary>
		/// <typeparam name="TEnum">設定するenumの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the EnumList type.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to set</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetEnumList<TEnum>(string name, IList<TEnum> value) where TEnum : System.Enum
		{
			Parameter parameter = GetParam(name);
			return SetEnumList(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// EnumList型の値を設定する。
		/// </summary>
		/// <typeparam name="TEnum">設定するenumの型</typeparam>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the EnumList type.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to set</typeparam>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetEnumList<TEnum>(int id, IList<TEnum> value) where TEnum : System.Enum
		{
			Parameter parameter = GetParam(id);
			return SetEnumList(parameter, value);
		}

		private bool TryGetEnumList<TEnum>(Parameter parameter, out IList<TEnum> value) where TEnum : System.Enum
		{
			if (parameter != null)
			{
				return parameter.TryGetEnumList(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// EnumList型の値を取得する。
		/// </summary>
		/// <typeparam name="TEnum">取得するenumの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the EnumList type.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to get</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetEnumList<TEnum>(string name, out IList<TEnum> value) where TEnum : System.Enum
		{
			Parameter parameter = GetParam(name);
			return TryGetEnumList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// EnumList型の値を取得する。
		/// </summary>
		/// <typeparam name="TEnum">取得するenumの型</typeparam>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Enum type.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to get</typeparam>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetEnumList<TEnum>(int id, out IList<TEnum> value) where TEnum : System.Enum
		{
			Parameter parameter = GetParam(id);
			return TryGetEnumList(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// EnumList型の値を取得する。
		/// </summary>
		/// <typeparam name="TEnum">取得するenumの型</typeparam>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the EnumList type.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to get</typeparam>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<TEnum> GetEnumList<TEnum>(int id) where TEnum : System.Enum
		{
			IList<TEnum> value = null;
			if (TryGetEnumList(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// EnumList型の値を取得する。
		/// </summary>
		/// <typeparam name="TEnum">取得するenumの型</typeparam>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the EnumList type.
		/// </summary>
		/// <typeparam name="TEnum">Type of enum to get</typeparam>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<TEnum> GetEnumList<TEnum>(string name) where TEnum : System.Enum
		{
			IList<TEnum> value = null;
			if (TryGetEnumList(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // EnumList
	}
}