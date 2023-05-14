//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Presets;

namespace ArborEditor.Presets
{
	using Arbor;

	internal sealed class PresetContextMenu
#if UNITY_2023_1_OR_NEWER
		: ScriptableObject
#else
		: PresetSelectorReceiver
#endif
	{
		private static class Style
		{
			public static readonly GUIStyle bottomBarBg = "ProjectBrowserBottomBarBg";
			public static readonly GUIStyle toolbarBack = "ObjectPickerToolbar";
			public static readonly GUIContent presetIcon = EditorGUIUtility.IconContent("Preset.Context");
		}

		Object _Target;
		Preset _InitialValue;

		System.Action _OnChanged;

		private bool _Applied = false;

		internal static bool HasPresetButton(Object target)
		{
			if (PresetUtility.IsObjectExcludedFromPresets(target)
				|| (target.hideFlags & HideFlags.NotEditable) != 0)
			{
				return false;
			}

			return true;
		}

#if ARBOR_DLL
		delegate void DelegateShowSelector(Object[] targets, Preset currentSelection, bool createNewAllowed, System.Action<Preset> onSelectionChanged, System.Action<Preset, bool> onSelectionClosed);
		private static DelegateShowSelector s_ShowSelector;

		static PresetContextMenu()
		{
			var showSelectorMethod = typeof(PresetSelector).GetMethod("ShowSelector",
				System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, null,
				new System.Type[] { typeof(Object[]), typeof(Preset), typeof(bool), typeof(System.Action<Preset>), typeof(System.Action<Preset, bool>) }, null);
			if (showSelectorMethod != null)
			{
				s_ShowSelector = (DelegateShowSelector)System.Delegate.CreateDelegate(typeof(DelegateShowSelector), showSelectorMethod, false);
			}
		}
#endif

		internal static void CreateAndShow(Object target,System.Action onChanged)
		{
			var instance = CreateInstance<PresetContextMenu>();
			instance.Init(target, onChanged);

#if ARBOR_DLL
			if (s_ShowSelector != null)
			{
				s_ShowSelector.Invoke(new Object[] { target }, null, true, instance.OnSelectionChanged, 
					(preset, canceled) => instance.OnSelectionClosed(preset));
			}
			else
			{
				PresetSelector.ShowSelector(target, null, true, instance);
			}
#elif UNITY_2023_1_OR_NEWER
			PresetSelector.ShowSelector(new Object[] { target }, null, true, instance.OnSelectionChanged, instance.OnSelectionClosed);
#else
			PresetSelector.ShowSelector(target, null, true, instance);
#endif
		}

		internal void Init(Object target,System.Action onChanged)
		{
			_Target = target;
			_InitialValue = new Preset(target);
			_OnChanged = onChanged;
			_Applied = false;
		}

#if UNITY_2023_1_OR_NEWER
		void OnSelectionChanged(Preset selection)
#else
		public override void OnSelectionChanged(Preset selection)
#endif
		{
			if (selection != null)
			{
				Undo.RecordObject(_Target, "Apply Preset " + selection.name);

				PresetUtility.ApplyPreset(selection, _Target);

				_Applied = true;
			}
			else
			{
				if (_Applied)
				{
					Undo.RecordObject(_Target, "Cancel Preset");

					PresetUtility.ApplyPreset(_InitialValue, _Target);
				}
			}

			_OnChanged?.Invoke();
		}

#if UNITY_2023_1_OR_NEWER
		void OnSelectionClosed(Preset selection, bool canceled)
#else
		public override void OnSelectionClosed(Preset selection)
#endif
		{
			OnSelectionChanged(selection);
			DestroyImmediate(this);
		}
	}
}