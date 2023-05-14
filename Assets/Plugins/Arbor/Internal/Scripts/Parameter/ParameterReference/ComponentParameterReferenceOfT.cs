//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Componentパラメータの参照(ジェネリック)。
	/// </summary>
	/// <typeparam name="T">参照するコンポーネントの型</typeparam>
#else
	/// <summary>
	/// Reference Component parameters(Generic).
	/// </summary>
	/// <typeparam name="T">Type of component to reference</typeparam>
#endif
	public class ComponentParameterReference<T> : ParameterReference, IValueContainer<T> where T : Component
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの値。
		/// </summary>
#else
		/// <summary>
		/// Value of the parameter
		/// </summary>
#endif
		public T value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetComponent<T>();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetComponent<T>(value);
				}
			}
		}

		T IValueGetter<T>.GetValue()
		{
			return value;
		}

		void IValueSetter<T>.SetValue(T value)
		{
			this.value = value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="ComponentParameterReference{T}"/>を<see cref="ComponentParameterReference"/>にキャスト。
		/// </summary>
		/// <param name="parameterReference"><see cref="ComponentParameterReference{T}"/></param>
		/// <returns>ComponentParameterReferenceにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast <see cref="ComponentParameterReference{T}"/> to <see cref="ComponentParameterReference"/>.
		/// </summary>
		/// <param name="parameterReference"><see cref="ComponentParameterReference{T}"/></param>
		/// <returns>Returns the result of casting to ComponentParameterReference.</returns>
#endif
		public static explicit operator ComponentParameterReference(ComponentParameterReference<T> parameterReference)
		{
			ComponentParameterReference componentParameterReference = new ComponentParameterReference();
			componentParameterReference.Copy(parameterReference);
			return componentParameterReference;
		}
	}
}