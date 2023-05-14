//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(PlaySound))]
	internal sealed class PlaySoundInspector : InspectorBase
	{
		protected override void OnRegisterElements()
		{
			RegisterProperty("_AudioSource");
			RegisterProperty("_IsSetClip");
			RegisterProperty("_Clip");
		}
	}
}