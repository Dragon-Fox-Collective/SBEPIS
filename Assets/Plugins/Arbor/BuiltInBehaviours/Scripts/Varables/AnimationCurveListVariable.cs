//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// List&lt;AnimationCurve&gt;型のVariable
	/// </summary>
#else
	/// <summary>
	/// List&lt;AnimationCurve&gt; type Variable
	/// </summary>
#endif
	[AddVariableMenu("UnityEngine/AnimationCurveList")]
	[BehaviourTitle("AnimationCurveList")]
	[AddComponentMenu("")]
	public sealed class AnimationCurveListVariable : VariableList<AnimationCurve>
	{
	}
}