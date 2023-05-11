//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// List&lt;Gradient&gt;型のVariable
	/// </summary>
#else
	/// <summary>
	/// List&lt;Gradient&gt; type Variable
	/// </summary>
#endif
	[AddVariableMenu("UnityEngine/GradientList")]
	[BehaviourTitle("GradientList")]
	[AddComponentMenu("")]
	public sealed class GradientListVariable : VariableList<Gradient>
	{
	}
}