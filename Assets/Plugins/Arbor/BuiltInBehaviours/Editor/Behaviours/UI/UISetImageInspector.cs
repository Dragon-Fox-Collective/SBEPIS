//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ARBOR_SUPPORT_UGUI
using UnityEditor;

using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using ArborEditor.Inspectors;

	[CustomEditor(typeof(UISetImage))]
	internal sealed class UISetImageInspector : InspectorBase
	{
		protected override void OnRegisterElements()
		{
			RegisterProperty("_Image");
			RegisterProperty("_Sprite");
		}
	}
}
#endif
