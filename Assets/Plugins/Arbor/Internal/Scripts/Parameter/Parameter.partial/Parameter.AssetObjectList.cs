//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor
{
	public sealed partial class Parameter
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObjectList型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of ComponentList type.
		/// </summary>
#endif
		public IList<Object> assetObjectListValue
		{
			get
			{
				IList<Object> value;
				if (TryGetAssetObjectList(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetAssetObjectList(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region AssetObjectList

		object Internal_GetAssetObjectList()
		{
			if (type == Type.AssetObjectList)
			{
				return container._AssetObjectListParameters[_ParameterIndex].listObject;
			}

			return null;
		}

		void Internal_SetAssetObjectList(IList value, bool changeType)
		{
			if (changeType)
			{
				if (value == null)
				{
					return;
				}

				System.Type valueType = value.GetType();
				System.Type elementType = ListUtility.GetElementType(valueType);

				Internal_AssetObjectUpdateTypeIfNecessary(elementType);
			}

			if (container._AssetObjectListParameters[_ParameterIndex].SetList(value))
			{
				DoChanged();
			}
		}

		void Internal_AssetObjectUpdateTypeIfNecessary(System.Type elementType)
		{
			if (elementType == null || !TypeUtility.IsAssignableFrom(typeof(Object), elementType))
			{
				return;
			}

			var objectType = referenceType.type ?? typeof(Object);
			if (elementType == objectType)
			{
				return;
			}

			var parameters = container._AssetObjectListParameters[_ParameterIndex];

			parameters.OnBeforeSerialize();

			referenceType.type = elementType;

			parameters.OnAfterDeserialize(elementType);
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
			if (type == Type.AssetObjectList)
			{
				Internal_AssetObjectUpdateTypeIfNecessary(typeof(TAssetObject));
				Internal_SetAssetObjectList((IList)value, false);
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObjectList型の値を取得する。
		/// </summary>
		/// <typeparam name="TAssetObject">取得するAssetObjectの型</typeparam>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the AssetObjectList type.
		/// </summary>
		/// <typeparam name="TAssetObject">Type of AssetObject to get</typeparam>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetAssetObjectList<TAssetObject>(out IList<TAssetObject> value) where TAssetObject : Object
		{
			var componentType = referenceType.type ?? typeof(Object);

			if (type == Type.AssetObjectList)
			{
				if (typeof(TAssetObject) == typeof(Object))
				{
					value = (IList<TAssetObject>)container._AssetObjectListParameters[_ParameterIndex].list;
					return true;
				}
				else if (typeof(TAssetObject) == componentType)
				{
					value = (IList<TAssetObject>)container._AssetObjectListParameters[_ParameterIndex].listObject;
					return true;
				}
			}

			value = null;
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
			IList<TAssetObject> value;
			if (TryGetAssetObjectList(out value))
			{
				return value;
			}
			return null;
		}

		#endregion //AssetObjectList
	}
}