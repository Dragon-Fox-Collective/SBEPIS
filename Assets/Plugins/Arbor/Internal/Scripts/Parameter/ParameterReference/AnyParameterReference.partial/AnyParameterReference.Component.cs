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
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetComponent();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetComponent(value);
				}
			}
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
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.SetComponent<TComponent>(value);
			}

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
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.GetComponent<TComponent>(defaultValue);
			}

			return defaultValue;
		}
	}
}