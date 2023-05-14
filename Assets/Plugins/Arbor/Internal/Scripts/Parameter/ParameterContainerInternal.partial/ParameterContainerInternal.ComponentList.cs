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
		internal List<ComponentListParameter> _ComponentListParameters = new List<ComponentListParameter>();

		#region ComponentList

		private bool SetComponentList<TComponent>(Parameter parameter, IList<TComponent> value) where TComponent : Component
		{
			return parameter != null && parameter.SetComponentList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ComponentList型の値を設定する。
		/// </summary>
		/// <typeparam name="TComponent">設定するComponentの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the ComponentList type.
		/// </summary>
		/// <typeparam name="TComponent">Type of Component to set</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetComponentList<TComponent>(string name, IList<TComponent> value) where TComponent : Component
		{
			Parameter parameter = GetParam(name);
			return SetComponentList(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ComponentList型の値を設定する。
		/// </summary>
		/// <typeparam name="TComponent">設定するComponentの型</typeparam>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the ComponentList type.
		/// </summary>
		/// <typeparam name="TComponent">Type of Component to set</typeparam>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetComponentList<TComponent>(int id, IList<TComponent> value) where TComponent : Component
		{
			Parameter parameter = GetParam(id);
			return SetComponentList(parameter, value);
		}

		private bool TryGetComponentList<TComponent>(Parameter parameter, out IList<TComponent> value) where TComponent : Component
		{
			if (parameter != null)
			{
				return parameter.TryGetComponentList(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ComponentList型の値を取得する。
		/// </summary>
		/// <typeparam name="TComponent">取得するComponentの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the ComponentList type.
		/// </summary>
		/// <typeparam name="TComponent">Type of Component to get</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetComponentList<TComponent>(string name, out IList<TComponent> value) where TComponent : Component
		{
			Parameter parameter = GetParam(name);
			return TryGetComponentList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ComponentList型の値を取得する。
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
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetComponentList<TComponent>(int id, out IList<TComponent> value) where TComponent : Component
		{
			Parameter parameter = GetParam(id);
			return TryGetComponentList(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// ComponentList型の値を取得する。
		/// </summary>
		/// <typeparam name="TComponent">取得するComponentの型</typeparam>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the ComponentList type.
		/// </summary>
		/// <typeparam name="TComponent">Type of Component to get</typeparam>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<TComponent> GetComponentList<TComponent>(int id) where TComponent : Component
		{
			IList<TComponent> value = null;
			if (TryGetComponentList(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ComponentList型の値を取得する。
		/// </summary>
		/// <typeparam name="TComponent">取得するComponentの型</typeparam>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the ComponentList type.
		/// </summary>
		/// <typeparam name="TComponent">Type of Component to get</typeparam>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<TComponent> GetComponentList<TComponent>(string name) where TComponent : Component
		{
			IList<TComponent> value = null;
			if (TryGetComponentList(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // ComponentList
	}
}