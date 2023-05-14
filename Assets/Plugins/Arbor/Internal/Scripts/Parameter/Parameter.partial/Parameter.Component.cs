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
		/// コンポーネントの型
		/// </summary>
#else
		/// <summary>
		/// Component type
		/// </summary>
#endif
		[System.Obsolete("use referenceType")]
		public ClassTypeReference componentType
		{
			get
			{
				return referenceType;
			}
			set
			{
				referenceType = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Component type.
		/// </summary>
#endif
		public Component componentValue
		{
			get
			{
				Component value;
				if (TryGetComponent(out value))
				{
					return value;
				}
				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetComponent(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Component

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Component type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetComponent(Component value)
		{
			switch (type)
			{
				case Type.Component:
					if (value != null)
					{
						System.Type componentType = value.GetType();
						if (referenceType != componentType)
						{
							referenceType = componentType;
						}
					}
					Internal_SetObject(value);
					return true;
				case Type.Transform:
					if (value == null || TypeUtility.IsAssignableFrom(typeof(Transform), value.GetType()))
					{
						Internal_SetObject(value);
						return true;
					}
					break;
				case Type.RectTransform:
					if (value == null || TypeUtility.IsAssignableFrom(typeof(RectTransform), value.GetType()))
					{
						Internal_SetObject(value);
						return true;
					}
					break;
				case Type.Rigidbody:
					if (value == null || TypeUtility.IsAssignableFrom(typeof(Rigidbody), value.GetType()))
					{
						Internal_SetObject(value);
						return true;
					}
					break;
				case Type.Rigidbody2D:
					if (value == null || TypeUtility.IsAssignableFrom(typeof(Rigidbody2D), value.GetType()))
					{
						Internal_SetObject(value);
						return true;
					}
					break;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Component type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetComponent(out Component value)
		{
			if (type == Type.Transform || type == Type.RectTransform || type == Type.Rigidbody || type == Type.Rigidbody2D || type == Type.Component)
			{
				value = Internal_GetObject() as Component;
				return true;
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Component type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Component GetComponent(Component defaultValue = null)
		{
			Component value;
			if (TryGetComponent(out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Componentの値を設定
		/// </summary>
		/// <typeparam name="TComponent">設定するComponentの型</typeparam>
		/// <param name="value">値</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Set Component value
		/// </summary>
		/// <typeparam name="TComponent">Type of Component to set</typeparam>
		/// <param name="value">Value</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetComponent<TComponent>(TComponent value) where TComponent : Component
		{
			switch (type)
			{
				case Type.Component:
					System.Type componentType = typeof(TComponent);
					if (referenceType != componentType)
					{
						referenceType = componentType;
					}
					Internal_SetObject(value);
					return true;
				case Type.Transform:
					if (TypeUtility.IsAssignableFrom(typeof(Transform), typeof(TComponent)))
					{
						Internal_SetObject(value);
						return true;
					}
					break;
				case Type.RectTransform:
					if (TypeUtility.IsAssignableFrom(typeof(RectTransform), typeof(TComponent)))
					{
						Internal_SetObject(value);
						return true;
					}
					break;
				case Type.Rigidbody:
					if (TypeUtility.IsAssignableFrom(typeof(Rigidbody), typeof(TComponent)))
					{
						Internal_SetObject(value);
						return true;
					}
					break;
				case Type.Rigidbody2D:
					if (TypeUtility.IsAssignableFrom(typeof(Rigidbody2D), typeof(TComponent)))
					{
						Internal_SetObject(value);
						return true;
					}
					break;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値を取得する。
		/// </summary>
		/// <typeparam name="TComponent">取得するComponentの型</typeparam>
		/// <param name="value">取得する値。</param>
		/// <returns>成功した場合はtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Component type.
		/// </summary>
		/// <typeparam name="TComponent">Type of Component to get</typeparam>
		/// <param name="value">The value you get.</param>
		/// <returns>Returns true if it succeeds.</returns>
#endif
		public bool TryGetComponent<TComponent>(out TComponent value) where TComponent : Component
		{
			switch (type)
			{
				case Type.Component:
					value = Internal_GetObject() as TComponent;
					return true;
				case Type.Transform:
					if (TypeUtility.IsAssignableFrom(typeof(TComponent), typeof(Transform)))
					{
						value = Internal_GetObject() as TComponent;
						return true;
					}
					break;
				case Type.RectTransform:
					if (TypeUtility.IsAssignableFrom(typeof(TComponent), typeof(RectTransform)))
					{
						value = Internal_GetObject() as TComponent;
						return true;
					}
					break;
				case Type.Rigidbody:
					if (TypeUtility.IsAssignableFrom(typeof(TComponent), typeof(Rigidbody)))
					{
						value = Internal_GetObject() as TComponent;
						return true;
					}
					break;
				case Type.Rigidbody2D:
					if (TypeUtility.IsAssignableFrom(typeof(TComponent), typeof(Rigidbody2D)))
					{
						value = Internal_GetObject() as TComponent;
						return true;
					}
					break;
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Component型の値を取得する。
		/// </summary>
		/// <typeparam name="TComponent">取得するComponentの型</typeparam>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Component type.
		/// </summary>
		/// <typeparam name="TComponent">Type of Component to get</typeparam>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public TComponent GetComponent<TComponent>(TComponent defaultValue = null) where TComponent : Component
		{
			TComponent value;
			if (TryGetComponent(out value))
			{
				return value;
			}
			return defaultValue;
		}

		#endregion // Component
	}
}