//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if !(UNITY_ANDROID || UNITY_IOS || UNITY_WSA || UNITY_TVOS)
#define ARBOR_ENABLE_ONMOUSE_EVENT
#endif
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
	public abstract class OnMouseBehaviourBase : StateBehaviour
	{
	}
}