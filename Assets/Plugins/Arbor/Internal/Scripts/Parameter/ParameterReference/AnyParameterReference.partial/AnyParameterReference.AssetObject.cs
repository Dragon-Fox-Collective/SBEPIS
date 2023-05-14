//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	public sealed partial class AnyParameterReference
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObject型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of AssetObject type.
		/// </summary>
#endif
		public Object assetObjectValue
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetAssetObject();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetAssetObject(value);
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObjectの値を設定
		/// </summary>
		/// <typeparam name="TAssetObject">設定するAssetObjectの型</typeparam>
		/// <param name="value">値</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Set AssetObject value
		/// </summary>
		/// <typeparam name="TAssetObject">Type of AssetObject to set</typeparam>
		/// <param name="value">Value</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetAssetObject<TAssetObject>(TAssetObject value) where TAssetObject : Object
		{
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.SetAssetObject<TAssetObject>(value);
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObject型の値を取得する。
		/// </summary>
		/// <typeparam name="TAssetObject">取得するAssetObjectの型</typeparam>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the AssetObject type.
		/// </summary>
		/// <typeparam name="TAssetObject">Type of AssetObject to get</typeparam>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public TAssetObject GetAssetObject<TAssetObject>(TAssetObject defaultValue = null) where TAssetObject : Object
		{
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.GetAssetObject<TAssetObject>(defaultValue);
			}

			return defaultValue;
		}
	}
}