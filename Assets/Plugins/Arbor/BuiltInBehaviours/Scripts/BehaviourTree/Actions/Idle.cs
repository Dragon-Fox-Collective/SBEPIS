//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor.BehaviourTree.Actions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 何も行いません。<br/>
	/// このアクションから抜けるにはAbortFlags.SelfのDecoratorが失敗を返すか、高優先度のノードに割り込ませてください。
	/// </summary>
#else
	/// <summary>
	/// Do nothing.<br/>
	/// To exit this action, the Decorator in AbortFlags.Self should either return a failure or interrupt the high priority node.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Idle")]
	[BuiltInBehaviour]
	public sealed class Idle : ActionBehaviour
	{
	}
}