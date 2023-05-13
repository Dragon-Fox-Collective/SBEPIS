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
		/// ComponentList型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of ComponentList type.
		/// </summary>
#endif
		public IList<Component> componentListValue
		{
			get
			{
				IList<Component> value;
				if (TryGetComponentList(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetComponentList(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region ComponentList

		object Internal_GetComponentList()
		{
			if (type == Type.ComponentList)
			{
				return container._ComponentListParameters[_ParameterIndex].listObject;
			}

			return null;
		}

		void Internal_SetComponentList(IList value, bool changeType)
		{
			if (changeType)
			{
				if (value == null)
				{
					return;
				}

				System.Type valueType = value.GetType();
				System.Type elementType = ListUtility.GetElementType(valueType);

				Internal_ComponentListUpdateTypeIfNecessary(elementType);
			}

			if (container._ComponentListParameters[_ParameterIndex].SetList(value))
			{
				DoChanged();
			}
		}

		void Internal_ComponentListUpdateTypeIfNecessary(System.Type elementType)
		{
			if (elementType == null || !TypeUtility.IsAssignableFrom(typeof(Component), elementType))
			{
				return;
			}

			var componentType = referenceType.type;
			if (elementType == componentType)
			{
				return;
			}

			var parameters = container._ComponentListParameters[_ParameterIndex];

			parameters.OnBeforeSerialize();

			referenceType.type = elementType;

			parameters.OnAfterDeserialize(elementType);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ComponentList型の値を設定する。
		/// </summary>
		/// <typeparam name="TComponent">設定するComponentの型</typeparam>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the ComponentList type.
		/// </summary>
		/// <typeparam name="TComponent">Type of Component to set</typeparam>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetComponentList<TComponent>(IList<TComponent> value) where TComponent : Component
		{
			if (type == Type.ComponentList)
			{
				Internal_ComponentListUpdateTypeIfNecessary(typeof(TComponent));
				Internal_SetComponentList((IList)value, false);
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ComponentList型の値を取得する。
		/// </summary>
		/// <typeparam name="TComponent">取得するComponentの型</typeparam>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the ComponentList type.
		/// </summary>
		/// <typeparam name="TComponent">Type of Component to get</typeparam>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetComponentList<TComponent>(out IList<TComponent> value) where TComponent : Component
		{
			var componentType = referenceType.type ?? typeof(Component);

			if (type == Type.ComponentList)
			{
				if (typeof(TComponent) == typeof(Component))
				{
					value = (IList<TComponent>)container._ComponentListParameters[_ParameterIndex].list;
					return true;
				}
				else if (typeof(TComponent) == componentType)
				{
					value = (IList<TComponent>)container._ComponentListParameters[_ParameterIndex].listObject;
					return true;
				}
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ComponentList型の値を取得する。
		/// </summary>
		/// <typeparam name="TComponent">取得するComponentの型</typeparam>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the ComponentList type.
		/// </summary>
		/// <typeparam name="TComponent">Type of Component to get</typeparam>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<TComponent> GetComponentList<TComponent>() where TComponent : Component
		{
			IList<TComponent> value;
			if (TryGetComponentList(out value))
			{
				return value;
			}
			return null;
		}

		#endregion //ComponentList
	}
}