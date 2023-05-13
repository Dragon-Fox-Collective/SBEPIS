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
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetComponentList<Component>();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetComponentList(value);
				}
			}
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
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.SetComponentList<TComponent>(value);
			}

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
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.GetComponentList<TComponent>();
			}

			return null;
		}
	}
}