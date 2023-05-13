//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArborEditor
{
	using Arbor;
	using ArborEditor.Presets;
	using UnityEditor.Presets;

	[CustomPresetProcessor(typeof(StateBehaviour))]
	public class StateBehaviourPresetProcessor : PresetProcessor
	{
		public override void ApplyPreset(Preset preset, NodeBehaviour target)
		{
			StateBehaviour stateBehaviour = target as StateBehaviour;
			if (stateBehaviour == null)
			{
				return;
			}

			PresetUtility.ApplyPresetBehaviour(preset, stateBehaviour);

			for (int i = 0, count = stateBehaviour.stateLinkCount; i < count; ++i)
			{
				StateLink s = stateBehaviour.GetStateLink(i);
				s.stateID = 0;
			}
		}
	}
}