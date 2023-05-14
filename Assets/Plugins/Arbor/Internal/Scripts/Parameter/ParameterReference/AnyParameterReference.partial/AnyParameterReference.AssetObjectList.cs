//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
	public sealed partial class AnyParameterReference
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObjectList型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of AssetObjectList type.
		/// </summary>
#endif
		public IList<Object> assetObjectListValue
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetAssetObjectList<Object>();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetAssetObjectList(value);
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObjectList型の値を設定する。
		/// </summary>
		/// <typeparam name="TAssetObject">設定するAssetObjectの型</typeparam>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the AssetObjectList type.
		/// </summary>
		/// <typeparam name="TAssetObject">Type of AssetObject to set</typeparam>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetAssetObjectList<TAssetObject>(IList<TAssetObject> value) where TAssetObject : Object
		{
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.SetAssetObjectList<TAssetObject>(value);
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObjectList型の値を取得する。
		/// </summary>
		/// <typeparam name="TAssetObject">取得するAssetObjectの型</typeparam>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the AssetObjectList type.
		/// </summary>
		/// <typeparam name="TAssetObject">Type of AssetObject to get</typeparam>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<TAssetObject> GetAssetObjectList<TAssetObject>() where TAssetObject : Object
		{
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.GetAssetObjectList<TAssetObject>();
			}

			return null;
		}
	}
}