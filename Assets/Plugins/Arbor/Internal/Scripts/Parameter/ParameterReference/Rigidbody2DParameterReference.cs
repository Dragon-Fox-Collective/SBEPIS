//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Rigidbody2Dパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Rigidbody2D parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Rigidbody2D)]
	public sealed class Rigidbody2DParameterReference : ComponentParameterReference<Rigidbody2D>
	{
	}
}
