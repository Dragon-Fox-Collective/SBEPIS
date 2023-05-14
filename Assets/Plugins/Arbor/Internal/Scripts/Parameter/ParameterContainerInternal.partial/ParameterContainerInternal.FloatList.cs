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
		internal List<FloatListParameter> _FloatListParameters = new List<FloatListParameter>();

		#region FloatList

		private bool SetFloatList(Parameter parameter, IList<float> value)
		{
			return parameter != null && parameter.SetFloatList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FloatList型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the FloatList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetFloatList(string name, IList<float> value)
		{
			Parameter parameter = GetParam(name);
			return SetFloatList(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FloatList型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the FloatList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetFloatList(int id, IList<float> value)
		{
			Parameter parameter = GetParam(id);
			return SetFloatList(parameter, value);
		}

		private bool TryGetFloatList(Parameter parameter, out IList<float> value)
		{
			if (parameter != null)
			{
				return parameter.TryGetFloatList(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FloatList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the FloatList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetFloatList(string name, out IList<float> value)
		{
			Parameter parameter = GetParam(name);
			return TryGetFloatList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FloatList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the FloatList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetFloatList(int id, out IList<float> value)
		{
			Parameter parameter = GetParam(id);
			return TryGetFloatList(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// FloatList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the FloatList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<float> GetFloatList(int id)
		{
			IList<float> value = null;
			if (TryGetFloatList(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FloatList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the FloatList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<float> GetFloatList(string name)
		{
			IList<float> value = null;
			if (TryGetFloatList(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // FloatList
	}
}