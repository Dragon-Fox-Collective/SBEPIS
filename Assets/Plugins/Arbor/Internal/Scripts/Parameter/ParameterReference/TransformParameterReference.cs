//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Transformパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference Transform parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.Transform)]
	public sealed class TransformParameterReference : ComponentParameterReference<Transform>
	{
	}
}
