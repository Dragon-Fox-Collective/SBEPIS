//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	public sealed partial class Parameter
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// AssetObject型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Component type.
		/// </summary>
#endif
		public Object assetObjectValue
		{
			get
			{
				Object value;
				if (TryGetAssetObject(out value))
				{
					return value;
				}
				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetAssetObject(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region AssetObject

#if ARBOR_DOC_JA
		/// <summary>
		/// Object型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Object type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetAssetObject(Object value)
		{
			if (type == Type.AssetObject)
			{
				if (value != null)
				{
					System.Type valueType = value.GetType();
					if (referenceType != valueType)
					{
						referenceType = valueType;
					}
				}
				Internal_SetObject(value);
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Object型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Object type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetAssetObject(out Object value)
		{
			if (type == Type.AssetObject)
			{
				value = Internal_GetObject();

				return true;
			}

			value = null;
			return false;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// Object型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Object type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Object GetAssetObject(Object defaultValue = null)
		{
			Object value;
			if (TryGetAssetObject(out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Objectの値を設定
		/// </summary>
		/// <typeparam name="TObject">設定するAssetObjectの型</typeparam>
		/// <param name="value">値</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Set Object value
		/// </summary>
		/// <typeparam name="TObject">Type of AssetObject to set</typeparam>
		/// <param name="value">Value</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetAssetObject<TObject>(TObject value) where TObject : Object
		{
			if (type == Type.AssetObject)
			{
				System.Type objectType = typeof(TObject);
				if (referenceType != objectType)
				{
					referenceType = objectType;
				}
				Internal_SetObject(value);
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値を取得する。
		/// </summary>
		/// <typeparam name="TObject">取得するAssetObjectの型</typeparam>
		/// <param name="value">取得する値。</param>
		/// <returns>成功した場合はtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Component type.
		/// </summary>
		/// <typeparam name="TObject">Type of AssetObject to get</typeparam>
		/// <param name="value">The value you get.</param>
		/// <returns>Returns true if it succeeds.</returns>
#endif
		public bool TryGetAssetObject<TObject>(out TObject value) where TObject : Object
		{
			if (type == Type.AssetObject)
			{
				value = Internal_GetObject() as TObject;
				return true;
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Object型の値を取得する。
		/// </summary>
		/// <typeparam name="TObject">取得するAssetObjectの型</typeparam>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Object type.
		/// </summary>
		/// <typeparam name="TObject">Type of AssetObject to get</typeparam>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public TObject GetAssetObject<TObject>(TObject defaultValue = null) where TObject : Object
		{
			TObject value;
			if (TryGetAssetObject(out value))
			{
				return value;
			}
			return defaultValue;
		}


		#endregion //AssetObject
	}
}