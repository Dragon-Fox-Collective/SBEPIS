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
		internal List<Rect> _RectParameters = new List<Rect>();

		#region Rect

		private bool SetRect(Parameter parameter, Rect value)
		{
			return parameter != null && parameter.SetRect(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rect型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Rect type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetRect(string name, Rect value)
		{
			Parameter parameter = GetParam(name);
			return SetRect(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rect型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Rect type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetRect(int id, Rect value)
		{
			Parameter parameter = GetParam(id);
			return SetRect(parameter, value);
		}

		private bool TryGetRect(Parameter parameter, out Rect value)
		{
			if (parameter != null)
			{
				return parameter.TryGetRect(out value);
			}

			value = new Rect();
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rect型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Rect type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetRect(string name, out Rect value)
		{
			Parameter parameter = GetParam(name);
			return TryGetRect(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rect型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Rect type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetRect(int id, out Rect value)
		{
			Parameter parameter = GetParam(id);
			return TryGetRect(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rect型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Rect type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Rect GetRect(string name, Rect defaultValue)
		{
			Rect value = new Rect();
			if (TryGetRect(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rect型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はRect(0, 0, 0, 0)を返す。</returns>
#else
		/// <summary>
		/// Get the value of the Rect type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns Rect(0, 0, 0, 0).</returns>
#endif
		public Rect GetRect(string name)
		{
			return GetRect(name, new Rect(0, 0, 0, 0));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rect型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Rect type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Rect GetRect(int id, Rect defaultValue)
		{
			Rect value = new Rect();
			if (TryGetRect(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rect型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はRect(0, 0, 0, 0)を返す。</returns>
#else
		/// <summary>
		/// Get the value of the Rect type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns Rect(0, 0, 0, 0).</returns>
#endif
		public Rect GetRect(int id)
		{
			return GetRect(id, new Rect(0, 0, 0, 0));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rect型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Rect type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetRect(string, out Rect)")]
		public bool GetRect(string name, out Rect value)
		{
			return TryGetRect(name, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rect型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Rect type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		[System.Obsolete("use TryGetRect(int, out Rect)")]
		public bool GetRect(int id, out Rect value)
		{
			return TryGetRect(id, out value);
		}

		#endregion //Rect
	}
}