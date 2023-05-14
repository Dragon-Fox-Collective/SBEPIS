//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
	using Arbor.ValueFlow;
	
#if ARBOR_DOC_JA
	/// <summary>
	/// GameObjectListパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference GameObjectList parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.GameObjectList)]
	public sealed class GameObjectListParameterReference : ParameterReference, IValueContainer<IList<GameObject>>
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
		public IList<GameObject> value
		{
			get
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					return parameter.GetGameObjectList();
				}

				return null;
			}
			set
			{
				Parameter parameter = this.parameter;
				if (parameter != null)
				{
					parameter.SetGameObjectList(value);
				}
			}
		}

		IList<GameObject> IValueGetter<IList<GameObject>>.GetValue()
		{
			return value;
		}

		void IValueSetter<IList<GameObject>>.SetValue(IList<GameObject> value)
		{
			this.value = value;
		}
	}
}