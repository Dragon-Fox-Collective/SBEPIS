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
		internal List<ColorListParameter> _ColorListParameters = new List<ColorListParameter>();

		#region ColorList

		private bool SetColorList(Parameter parameter, IList<Color> value)
		{
			return parameter != null && parameter.SetColorList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ColorList型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the ColorList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetColorList(string name, IList<Color> value)
		{
			Parameter parameter = GetParam(name);
			return SetColorList(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ColorList型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the ColorList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetColorList(int id, IList<Color> value)
		{
			Parameter parameter = GetParam(id);
			return SetColorList(parameter, value);
		}

		private bool TryGetColorList(Parameter parameter, out IList<Color> value)
		{
			if (parameter != null)
			{
				return parameter.TryGetColorList(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ColorList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the ColorList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetColorList(string name, out IList<Color> value)
		{
			Parameter parameter = GetParam(name);
			return TryGetColorList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ColorList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the ColorList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetColorList(int id, out IList<Color> value)
		{
			Parameter parameter = GetParam(id);
			return TryGetColorList(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// ColorList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the ColorList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Color> GetColorList(int id)
		{
			IList<Color> value = null;
			if (TryGetColorList(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ColorList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the ColorList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Color> GetColorList(string name)
		{
			IList<Color> value = null;
			if (TryGetColorList(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // ColorList
	}
}