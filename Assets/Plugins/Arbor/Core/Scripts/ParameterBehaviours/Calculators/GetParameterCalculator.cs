//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.ParameterBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Parameterから値を取得する。
	/// </summary>
#else
	/// <summary>
	/// Get a value from Parameter.
	/// </summary>
#endif
	[BehaviourTitle("GetParameter")]
	[BuiltInBehaviour]
	[AddComponentMenu("")]
	[AddBehaviourMenu("Parameter/GetParameter")]
	public sealed class GetParameterCalculator : GetParameterCalculatorInternal
	{
	}
}