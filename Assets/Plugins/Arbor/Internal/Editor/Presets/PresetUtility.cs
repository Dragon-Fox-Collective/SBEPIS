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
	using Arbor.DynamicReflection;

	public static class PresetUtility
	{
		static readonly DynamicField s_ReferencesField;
		static readonly DynamicField s_ReferenceField;

		static PresetUtility()
		{
			var unityEditorAssembly = System.Reflection.Assembly.Load("UnityEditor.dll");
			var presetEditorType = unityEditorAssembly.GetType("UnityEditor.Presets.PresetEditor");
			if (presetEditorType != null)
			{
				var referencesField = presetEditorType.GetField("s_References", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
				s_ReferencesField = DynamicField.GetField(referencesField);
			}

			var referenceCountType = presetEditorType.GetNestedType("ReferenceCount", System.Reflection.BindingFlags.NonPublic);
			if (referenceCountType != null)
			{
				var referenceField = referenceCountType.GetField("reference", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
				s_ReferenceField = DynamicField.GetField(referenceField);
			}
		}

		public static void ApplyPresetBehaviour(Preset preset, NodeBehaviour target)
		{
			if (target == null)
			{
				return;
			}

			NodeGraph nodeGraph = target.nodeGraph;
			int nodeID = target.nodeID;
			bool expanded = target.expanded;
			
			Clipboard.DestroyChildGraphs(target);

			if (nodeGraph != null)
			{
				nodeGraph.DisconnectDataBranch(target);
			}

			preset.ApplyTo(target);

			target.Initialize(nodeGraph, nodeID);
			target.expanded = expanded;

			Clipboard.CopyChildGraphs(target);
		}

		private static Dictionary<System.Type, PresetProcessor> s_Processors = new Dictionary<System.Type, PresetProcessor>();

		private static PresetProcessor GetProcessor(System.Type classType)
		{
			if (classType == null)
			{
				return null;
			}

			PresetProcessor processor = null;
			if (!s_Processors.TryGetValue(classType, out processor))
			{
				System.Type processorType = CustomAttributes<CustomPresetProcessor, PresetProcessor>.FindEditorType(classType);
				if (processorType != null)
				{
					processor = (PresetProcessor)System.Activator.CreateInstance(processorType);
				}
				else
				{
					processor = null;
				}
				s_Processors.Add(classType, processor);
			}

			return processor;
		}

		public static void ApplyPreset(Preset preset, Object target)
		{
			NodeBehaviour nodeBehaviour = target as NodeBehaviour;
			if (nodeBehaviour != null)
			{
				var processor = GetProcessor(nodeBehaviour.GetType());
				if (processor != null)
				{
					processor.ApplyPreset(preset, nodeBehaviour);
				}
				else
				{
					ApplyPresetBehaviour(preset, nodeBehaviour);
				}
			}
			else
			{
				preset.ApplyTo(target);
			}
		}

		public static bool enableApplyDefaultPreset = true;

		public static bool IsObjectExcludedFromPresets(Object target)
		{
			return !(target != null && new PresetType(target).IsValid());
		}

		public static Preset GetDefaultPreset(Object target)
		{
			if (!enableApplyDefaultPreset || IsObjectExcludedFromPresets(target))
			{
				return null;
			}

			var defaults = Preset.GetDefaultPresetsForObject(target);
			return defaults.Length > 0 ? defaults[0] : null;
		}

		public static bool IsPreset(Object target)
		{
			if (s_ReferencesField == null || s_ReferenceField == null)
			{
				return false;
			}
			var referencesObj = s_ReferencesField.GetValue(null);

			IDictionary dicReferences = referencesObj as IDictionary;
			if (dicReferences == null)
			{
				return false;
			}

			foreach (var value in dicReferences.Values)
			{
				var reference = (Object)s_ReferenceField.GetValue(value);
				if (reference == target)
				{
					return true;
				}
			}

			return false;
		}
	}
}