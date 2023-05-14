//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	public partial class ParameterContainerInternal : ParameterContainerBase, ISerializationCallbackReceiver
	{
		#region Component

		private bool SetComponent(Parameter parameter, Component value)
		{
			return parameter != null && parameter.SetComponent(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Component type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetComponent(string name, Component value)
		{
			Parameter parameter = GetParam(name);
			return SetComponent(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Component type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetComponent(int id, Component value)
		{
			Parameter parameter = GetParam(id);
			return SetComponent(parameter, value);
		}

		private bool TryGetComponent(Parameter parameter, out Component value)
		{
			if (parameter != null)
			{
				return parameter.TryGetComponent(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Component type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetComponent(string name, out Component value)
		{
			Parameter parameter = GetParam(name);
			return TryGetComponent(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Component type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetComponent(int id, out Component value)
		{
			Parameter parameter = GetParam(id);
			return TryGetComponent(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Component type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public Component GetComponent(string name, Component defaultValue = null)
		{
			Component value = null;
			if (TryGetComponent(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Component type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public Component GetComponent(int id, Component defaultValue = null)
		{
			Component value = null;
			if (TryGetComponent(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

		private bool SetComponent<TComponent>(Parameter parameter, TComponent value) where TComponent : Component
		{
			return parameter != null && parameter.SetComponent<TComponent>(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値を設定する。
		/// </summary>
		/// <typeparam name="TComponent">設定するComponentの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Component type.
		/// </summary>
		/// <typeparam name="TComponent">Type of Component to set</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetComponent<TComponent>(string name, TComponent value) where TComponent : Component
		{
			Parameter parameter = GetParam(name);
			return SetComponent(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値を設定する。
		/// </summary>
		/// <typeparam name="TComponent">設定するComponentの型</typeparam>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Component type.
		/// </summary>
		/// <typeparam name="TComponent">Type of Component to set</typeparam>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetComponent<TComponent>(int id, TComponent value) where TComponent : Component
		{
			Parameter parameter = GetParam(id);
			return SetComponent(parameter, value);
		}

		private bool TryGetComponent<TComponent>(Parameter parameter, out TComponent value) where TComponent : Component
		{
			if (parameter != null)
			{
				return parameter.TryGetComponent<TComponent>(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値を取得する。
		/// </summary>
		/// <typeparam name="TComponent">取得するComponentの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Component type.
		/// </summary>
		/// <typeparam name="TComponent">Type of Component to get</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetComponent<TComponent>(string name, out TComponent value) where TComponent : Component
		{
			Parameter parameter = GetParam(name);
			return TryGetComponent(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値を取得する。
		/// </summary>
		/// <typeparam name="TComponent">取得するComponentの型</typeparam>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the Component type.
		/// </summary>
		/// <typeparam name="TComponent">Type of Component to get</typeparam>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetComponent<TComponent>(int id, out TComponent value) where TComponent : Component
		{
			Parameter parameter = GetParam(id);
			return TryGetComponent(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値を取得する。
		/// </summary>
		/// <typeparam name="TComponent">取得するComponentの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Component type.
		/// </summary>
		/// <typeparam name="TComponent">Type of Component to get</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public TComponent GetComponent<TComponent>(string name, TComponent defaultValue = null) where TComponent : Component
		{
			TComponent value;
			if (TryGetComponent<TComponent>(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値を取得する。
		/// </summary>
		/// <typeparam name="TComponent">取得するComponentの型</typeparam>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Component type.
		/// </summary>
		/// <typeparam name="TComponent">Type of Component to get</typeparam>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public TComponent GetComponent<TComponent>(int id, TComponent defaultValue = null) where TComponent : Component
		{
			TComponent value;
			if (TryGetComponent(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

		#endregion //Component
	}
}