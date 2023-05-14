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
	/// GameObjectパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference GameObject parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.GameObject)]
	public sealed class GameObjectParameterReference : ParameterReference, IValueContainer<GameObject>
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
		public GameObject value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetGameObject();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetGameObject(value);
				}
			}
		}

		GameObject IValueGetter<GameObject>.GetValue()
		{
			return value;
		}

		void IValueSetter<GameObject>.SetValue(GameObject value)
		{
			this.value = value;
		}
	}
}
