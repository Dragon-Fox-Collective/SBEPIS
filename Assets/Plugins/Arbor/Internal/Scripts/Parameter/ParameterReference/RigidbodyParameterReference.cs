//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Rigidbodyパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Rigidbody parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Rigidbody)]
	public sealed class RigidbodyParameterReference : ComponentParameterReference<Rigidbody>
	{
	}
}
