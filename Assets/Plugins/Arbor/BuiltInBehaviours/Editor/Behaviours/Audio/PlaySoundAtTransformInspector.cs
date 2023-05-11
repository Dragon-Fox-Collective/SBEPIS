//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(PlaySoundAtTransform))]
	internal sealed class PlaySoundAtTransformInspector : PlaySoundAtPointBaseInspector
	{
		protected override void OnRegisterElements()
		{
			base.OnRegisterElements();

			RegisterProperty("_Target");
		}
	}
}
