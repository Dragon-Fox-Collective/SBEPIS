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
		internal List<AssetObjectListParameter> _AssetObjectListParameters = new List<AssetObjectListParameter>();

		#region AssetObjectList

		private bool SetAssetObjectList<TAssetObject>(Parameter parameter, IList<TAssetObject> value) where TAssetObject : Object
		{
			return parameter != null && parameter.SetAssetObjectList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObjectList型の値を設定する。
		/// </summary>
		/// <typeparam name="TAssetObject">設定するAssetObjectの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the AssetObjectList type.
		/// </summary>
		/// <typeparam name="TAssetObject">Type of AssetObject to set</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetAssetObjectList<TAssetObject>(string name, IList<TAssetObject> value) where TAssetObject : Object
		{
			Parameter parameter = GetParam(name);
			return SetAssetObjectList(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObjectList型の値を設定する。
		/// </summary>
		/// <typeparam name="TAssetObject">設定するAssetObjectの型</typeparam>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the AssetObjectList type.
		/// </summary>
		/// <typeparam name="TAssetObject">Type of AssetObject to set</typeparam>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetAssetObjectList<TAssetObject>(int id, IList<TAssetObject> value) where TAssetObject : Object
		{
			Parameter parameter = GetParam(id);
			return SetAssetObjectList(parameter, value);
		}

		private bool TryGetAssetObjectList<TAssetObject>(Parameter parameter, out IList<TAssetObject> value) where TAssetObject : Object
		{
			if (parameter != null)
			{
				return parameter.TryGetAssetObjectList(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObjectList型の値を取得する。
		/// </summary>
		/// <typeparam name="TAssetObject">取得するAssetObjectの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the AssetObjectList type.
		/// </summary>
		/// <typeparam name="TAssetObject">Type of AssetObject to get</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetAssetObjectList<TAssetObject>(string name, out IList<TAssetObject> value) where TAssetObject : Object
		{
			Parameter parameter = GetParam(name);
			return TryGetAssetObjectList(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObjectList型の値を取得する。
		/// </summary>
		/// <typeparam name="TAssetObject">取得するAssetObjectの型</typeparam>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the AssetObject type.
		/// </summary>
		/// <typeparam name="TAssetObject">Type of AssetObject to get</typeparam>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetAssetObjectList<TAssetObject>(int id, out IList<TAssetObject> value) where TAssetObject : Object
		{
			Parameter parameter = GetParam(id);
			return TryGetAssetObjectList(parameter, out value);
		}
#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObjectList型の値を取得する。
		/// </summary>
		/// <typeparam name="TAssetObject">取得するAssetObjectの型</typeparam>
		/// <param name="id">ID。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the AssetObjectList type.
		/// </summary>
		/// <typeparam name="TAssetObject">Type of AssetObject to get</typeparam>
		/// <param name="id">ID.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<TAssetObject> GetAssetObjectList<TAssetObject>(int id) where TAssetObject : Object
		{
			IList<TAssetObject> value = null;
			if (TryGetAssetObjectList(id, out value))
			{
				return value;
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObjectList型の値を取得する。
		/// </summary>
		/// <typeparam name="TAssetObject">取得するAssetObjectの型</typeparam>
		/// <param name="name">名前。</param>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the AssetObjectList type.
		/// </summary>
		/// <typeparam name="TAssetObject">Type of AssetObject to get</typeparam>
		/// <param name="name">Name.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<TAssetObject> GetAssetObjectList<TAssetObject>(string name) where TAssetObject : Object
		{
			IList<TAssetObject> value = null;
			if (TryGetAssetObjectList(name, out value))
			{
				return value;
			}
			return null;
		}

		#endregion // AssetObjectList
	}
}