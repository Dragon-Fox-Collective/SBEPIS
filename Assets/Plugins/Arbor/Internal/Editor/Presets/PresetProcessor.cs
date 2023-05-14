//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Presets;

namespace ArborEditor.Presets
{
	using Arbor;

	public abstract class PresetProcessor
	{
		public abstract void ApplyPreset(Preset preset, NodeBehaviour target);
	}
}