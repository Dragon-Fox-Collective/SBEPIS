//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	public partial class ParameterContainerInternal : ParameterContainerBase, ISerializationCallbackReceiver
	{
		#region AssetObject

		private bool SetAssetObject(Parameter parameter, Object value)
		{
			return parameter != null && parameter.SetAssetObject(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObject型の値を設定する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the AssetObject type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetAssetObject(string name, Object value)
		{
			Parameter parameter = GetParam(name);
			return SetAssetObject(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObject型の値を設定する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the AssetObject type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetAssetObject(int id, Object value)
		{
			Parameter parameter = GetParam(id);
			return SetAssetObject(parameter, value);
		}

		private bool TryGetAssetObject(Parameter parameter, out Object value)
		{
			if (parameter != null)
			{
				return parameter.TryGetAssetObject(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObject型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the AssetObject type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetAssetObject(string name, out Object value)
		{
			Parameter parameter = GetParam(name);
			return TryGetAssetObject(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObject型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the AssetObject type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetAssetObject(int id, out Object value)
		{
			Parameter parameter = GetParam(id);
			return TryGetAssetObject(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObject型の値を取得する。
		/// </summary>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the AssetObject type.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public Object GetAssetObject(string name, Object defaultValue = null)
		{
			Object value = null;
			if (TryGetAssetObject(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObject型の値を取得する。
		/// </summary>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the AssetObject type.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public Object GetAssetObject(int id, Object defaultValue = null)
		{
			Object value = null;
			if (TryGetAssetObject(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

		private bool SetAssetObject<TAssetObject>(Parameter parameter, TAssetObject value) where TAssetObject : Object
		{
			return parameter != null && parameter.SetAssetObject<TAssetObject>(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObject型の値を設定する。
		/// </summary>
		/// <typeparam name="TAssetObject">設定するAssetObjectの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the AssetObject type.
		/// </summary>
		/// <typeparam name="TAssetObject">Type of AssetObject to set</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetAssetObject<TAssetObject>(string name, TAssetObject value) where TAssetObject : Object
		{
			Parameter parameter = GetParam(name);
			return SetAssetObject(parameter, value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObject型の値を設定する。
		/// </summary>
		/// <typeparam name="TAssetObject">設定するAssetObjectの型</typeparam>
		/// <param name="id">ID。</param>
		/// <param name="value">値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// It wants to set the value of the AssetObject type.
		/// </summary>
		/// <typeparam name="TAssetObject">Type of AssetObject to set</typeparam>
		/// <param name="id">ID.</param>
		/// <param name="value">Value.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool SetAssetObject<TAssetObject>(int id, TAssetObject value) where TAssetObject : Object
		{
			Parameter parameter = GetParam(id);
			return SetAssetObject(parameter, value);
		}

		private bool TryGetAssetObject<TAssetObject>(Parameter parameter, out TAssetObject value) where TAssetObject : Object
		{
			if (parameter != null)
			{
				return parameter.TryGetAssetObject<TAssetObject>(out value);
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObject型の値を取得する。
		/// </summary>
		/// <typeparam name="TAssetObject">取得するAssetObjectの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="value">取得する値。</param>
		/// <returns>指定した名前のパラメータがあった場合にtrue。</returns>
#else
		/// <summary>
		/// Get the value of the AssetObject type.
		/// </summary>
		/// <typeparam name="TAssetObject">Type of AssetObject to get</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetAssetObject<TAssetObject>(string name, out TAssetObject value) where TAssetObject : Object
		{
			Parameter parameter = GetParam(name);
			return TryGetAssetObject(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObject型の値を取得する。
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
		/// <param name="value">The value you get.</param>
		/// <returns>The true when there parameters of the specified name.</returns>
#endif
		public bool TryGetAssetObject<TAssetObject>(int id, out TAssetObject value) where TAssetObject : Object
		{
			Parameter parameter = GetParam(id);
			return TryGetAssetObject(parameter, out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObject型の値を取得する。
		/// </summary>
		/// <typeparam name="TAssetObject">取得するAssetObjectの型</typeparam>
		/// <param name="name">名前。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the AssetObject type.
		/// </summary>
		/// <typeparam name="TAssetObject">Type of AssetObject to get</typeparam>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public TAssetObject GetAssetObject<TAssetObject>(string name, TAssetObject defaultValue = null) where TAssetObject : Object
		{
			TAssetObject value;
			if (TryGetAssetObject<TAssetObject>(name, out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObject型の値を取得する。
		/// </summary>
		/// <typeparam name="TAssetObject">取得するAssetObjectの型</typeparam>
		/// <param name="id">ID。</param>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the AssetObject type.
		/// </summary>
		/// <typeparam name="TAssetObject">Type of AssetObject to get</typeparam>
		/// <param name="id">ID.</param>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public TAssetObject GetAssetObject<TAssetObject>(int id, TAssetObject defaultValue = null) where TAssetObject : Object
		{
			TAssetObject value;
			if (TryGetAssetObject(id, out value))
			{
				return value;
			}
			return defaultValue;
		}

		#endregion //AssetObject
	}
}