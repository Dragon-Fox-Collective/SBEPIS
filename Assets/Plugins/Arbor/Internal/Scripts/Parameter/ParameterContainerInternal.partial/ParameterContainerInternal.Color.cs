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
		internal List<Color> _ColorParameters = new List<Color>();

		#region Color

		private bool SetColor(Parameter parameter, Color value)
		{
			return parameter != null && parameter.SetColor(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Color型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Color type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetColor(string name, Color value)
		{
			Parameter parameter = GetParam(name);
			return SetColor(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Color型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Color type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetColor(int id, Color value)
		{
			Parameter parameter = GetParam(id);
			return SetColor(parameter, value);
		}

		private bool TryGetColor(Parameter parameter, out Color value)
		{
			if (parameter != null)
			{
				return parameter.TryGetColor(out value);
			}

			value = Color.white;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Color型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Color type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetColor(string name, out Color value)
		{
			Parameter parameter = GetParam(name);
			return TryGetColor(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Color型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Color type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetColor(int id, out Color value)
		{
			Parameter parameter = GetParam(id);
			return TryGetColor(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Color型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Color type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Color GetColor(string name, Color defaultValue)
		{
			Color value = default(Color);
			if (TryGetColor(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Color型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はColor.whiteを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Color type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns Color.white.</returns>
#endif
		public Color GetColor(string name)
		{
			return GetColor(name, Color.white);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Color型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Color type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Color GetColor(int id, Color defaultValue)
		{
			Color value = default(Color);
			if (TryGetColor(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Color型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はColor.whiteを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Color type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns Color.white.</returns>
#endif
		public Color GetColor(int id)
		{
			return GetColor(id, Color.white);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Color型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Color type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetColor(string, out Color)")]
		public bool GetColor(string name, out Color value)
		{
			return TryGetColor(name, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Color型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Color type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetColor(int, out Color)")]
		public bool GetColor(int id, out Color value)
		{
			return TryGetColor(id, out value);
		}

		#endregion //Color
	}
}