//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// RectTransformパラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference RectTransform parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.ParameterType(Parameter.Type.RectTransform)]
	public sealed class RectTransformParameterReference : ComponentParameterReference<RectTransform>
	{
	}
}
