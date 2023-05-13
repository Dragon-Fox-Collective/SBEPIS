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
		internal List<GameObjectListParameter> _GameObjectListParameters = new List<GameObjectListParameter>();

		#region GameObjectList

		private bool SetGameObjectList(Parameter parameter, IList<GameObject> value)
		{
			return parameter != null && parameter.SetGameObjectList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectList型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the GameObjectList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetGameObjectList(string name, IList<GameObject> value)
		{
			Parameter parameter = GetParam(name);
			return SetGameObjectList(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectList型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the GameObjectList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetGameObjectList(int id, IList<GameObject> value)
		{
			Parameter parameter = GetParam(id);
			return SetGameObjectList(parameter, value);
		}

		private bool TryGetGameObjectList(Parameter parameter, out IList<GameObject> value)
		{
			if (parameter != null)
			{
				return parameter.TryGetGameObjectList(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the GameObjectList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetGameObjectList(string name, out IList<GameObject> value)
		{
			Parameter parameter = GetParam(name);
			return TryGetGameObjectList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the GameObjectList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetGameObjectList(int id, out IList<GameObject> value)
		{
			Parameter parameter = GetParam(id);
			return TryGetGameObjectList(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectList型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the GameObjectList type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<GameObject> GetGameObjectList(int id)
		{
			IList<GameObject> value = null;
			if (TryGetGameObjectList(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectList型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the GameObjectList type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<GameObject> GetGameObjectList(string name)
		{
			IList<GameObject> value = null;
			if (TryGetGameObjectList(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // GameObjectList
	}
}