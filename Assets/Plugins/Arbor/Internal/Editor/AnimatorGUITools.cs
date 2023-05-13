//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace ArborEditor
{
	using Arbor;

	public static class AnimatorGUITools
	{
		private static string StringPopupInternal(Rect position, GUIContent label, string selectedValue, GUIContent[] displayedOptions, string[] optionValues)
		{
			int selectedIndex = -1;
			if (optionValues != null)
			{
				selectedIndex = 0;
				while (selectedIndex < optionValues.Length && selectedValue != optionValues[selectedIndex])
					++selectedIndex;
			}
			int index = EditorGUI.Popup(position, label, selectedIndex, displayedOptions);
			if (optionValues == null)
				return string.Empty;
			if (index < 0 || index >= optionValues.Length)
				return selectedValue;
			return optionValues[index];
		}

		static void StringPopup(Rect position, SerializedProperty property, GUIContent[] displayedOptions, string[] optionValues, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, property);
			EditorGUI.BeginChangeCheck();
			string value = StringPopupInternal(position, label, property.stringValue, displayedOptions, optionValues);
			if (EditorGUI.EndChangeCheck())
			{
				property.stringValue = value;
			}
			EditorGUI.EndProperty();
		}

		public static AnimatorController GetAnimatorController(Animator animator)
		{
			if (animator == null)
			{
				return null;
			}

			var runtimeAnimatorController = animator.runtimeAnimatorController;
			if (runtimeAnimatorController is AnimatorOverrideController overrideController)
			{
				runtimeAnimatorController = overrideController.runtimeAnimatorController;
			}

			return runtimeAnimatorController as AnimatorController;
		}

		static int GetLayerIndex(AnimatorController animatorController, string layerName)
		{
			if (animatorController == null)
			{
				return -1;
			}
			AnimatorControllerLayer[] layers = animatorController.layers;

			int layerCount = layers.Length;

			for (int i = 0; i < layerCount; i++)
			{
				AnimatorControllerLayer layer = layers[i];

				if (layer.name == layerName)
				{
					return i;
				}
			}

			return -1;
		}

		static AnimatorControllerLayer GetAnimatorLayer(AnimatorController animatorController, string layerName)
		{
			if (animatorController == null)
			{
				return null;
			}
			AnimatorControllerLayer[] layers = animatorController.layers;

			int layerIndex = GetLayerIndex(animatorController, layerName);

			return (layerIndex >= 0) ? layers[layerIndex] : null;
		}

		static AnimatorControllerLayer AnimatorLayerField(Rect position, AnimatorController animatorController, SerializedProperty layerNameProperty, GUIContent label)
		{
			if (animatorController == null)
			{
				layerNameProperty.stringValue = string.Empty;
				return null;
			}

			AnimatorControllerLayer[] layers = animatorController.layers;

			int layerCount = layers.Length;

			string[] layerNames = new string[layerCount];
			GUIContent[] layerDisplayed = new GUIContent[layerCount];

			for (int i = 0; i < layerCount; i++)
			{
				AnimatorControllerLayer layer = layers[i];

				string name = layer.name;

				layerNames[i] = name;
				layerDisplayed[i] = new GUIContent(name);
			}

			StringPopup(position, layerNameProperty, layerDisplayed, layerNames, label);

			int layerIndex = GetLayerIndex(animatorController, layerNameProperty.stringValue);

			AnimatorControllerLayer selectedLayer = (layerIndex >= 0) ? layers[layerIndex] : null;

			if (selectedLayer != null)
			{
				layerNameProperty.stringValue = selectedLayer.name;
			}
			else
			{
				layerNameProperty.stringValue = string.Empty;
			}

			return selectedLayer;
		}

		public static void AnimatorLayerField(AnimatorController animatorController, FlexibleFieldProperty layerNameProperty)
		{
			layerNameProperty.property.SetStateData<OnFlexibleConstantGUI>((fieldPosition, valueProperty, label) =>
			{
				if (animatorController == null)
				{
					return false;
				}
				AnimatorLayerField(fieldPosition, animatorController, valueProperty, label);
				return true;
			});
			EditorGUILayout.PropertyField(layerNameProperty.property);

			layerNameProperty.property.RemoveStateData<OnFlexibleConstantGUI>();
		}

		static void AnimatorStateField(Rect position, AnimatorControllerLayer layer, SerializedProperty stateNameProperty, GUIContent label)
		{
			if (layer == null)
			{
				return;
			}

			ChildAnimatorState[] states = layer.stateMachine.states;

			int stateCount = states.Length;

			string[] stateNames = new string[stateCount];
			GUIContent[] stateDisplayed = new GUIContent[stateCount];

			for (int i = 0; i < stateCount; i++)
			{
				AnimatorState state = states[i].state;

				string stateName = state.name;

				stateNames[i] = stateName;
				stateDisplayed[i] = new GUIContent(stateName);
			}

			StringPopup(position, stateNameProperty, stateDisplayed, stateNames, label);
		}

		public static void AnimatorStateField(AnimatorController animatorController, FlexibleFieldProperty layerNameProperty, FlexibleFieldProperty stateNameProperty)
		{
			AnimatorLayerField(animatorController, layerNameProperty);

			if (layerNameProperty.type == FlexibleType.Constant)
			{
				AnimatorControllerLayer layer = GetAnimatorLayer(animatorController, layerNameProperty.valueProperty.stringValue);

				stateNameProperty.property.SetStateData<OnFlexibleConstantGUI>((fieldPosition, valueProperty, label) =>
				{
					if (layer == null)
					{
						return false;
					}
					AnimatorStateField(fieldPosition, layer, valueProperty, label);
					return true;
				});
			}
			EditorGUILayout.PropertyField(stateNameProperty.property);

			stateNameProperty.property.RemoveStateData<OnFlexibleConstantGUI>();
		}

		private sealed class AnimatorParameters
		{
			public GUIContent[] displayNames;
			public string[] names;
			public AnimatorControllerParameterType[] types;
			public int selected;

			public Dictionary<AnimatorControllerParameterType, AnimatorParameters> parametersByType = new Dictionary<AnimatorControllerParameterType, AnimatorParameters>();

			public AnimatorParameters GetTypeParameter(AnimatorControllerParameterType type)
			{
				AnimatorParameters results = null;
				if (parametersByType.TryGetValue(type, out results))
				{
					return results;
				}
				return null;
			}

			public bool Update(AnimatorController animatorController, string name)
			{
				AnimatorControllerParameter[] animParames = animatorController.parameters;

				int parameterCount = (animParames != null) ? animParames.Length + 1 : 0;
				if (parameterCount > 0)
				{
					selected = -1;

					if (names == null || names.Length != parameterCount)
					{
						names = new string[parameterCount];
					}
					if (displayNames == null || displayNames.Length != parameterCount)
					{
						displayNames = new GUIContent[parameterCount];
					}
					if (this.types == null || this.types.Length != parameterCount)
					{
						this.types = new AnimatorControllerParameterType[parameterCount];
					}

					names[0] = string.Empty;
					displayNames[0] = GUIContentCaches.Get("[None]");
					this.types[0] = AnimatorControllerParameterType.Float;

					Dictionary<AnimatorControllerParameterType, List<int>> typeIndexes = new Dictionary<AnimatorControllerParameterType, List<int>>();

					for (int paramIndex = 1; paramIndex < parameterCount; paramIndex++)
					{
						AnimatorControllerParameter parameter = animParames[paramIndex - 1];

						string parameterName = parameter.name;

						displayNames[paramIndex] = new GUIContent(parameterName);
						names[paramIndex] = parameterName;
						types[paramIndex] = parameter.type;

						List<int> indexes = null;
						if (!typeIndexes.TryGetValue(parameter.type, out indexes))
						{
							indexes = new List<int>();
							typeIndexes.Add(parameter.type, indexes);
						}
						indexes.Add(paramIndex);

						if (parameterName == name)
						{
							selected = paramIndex;
						}
					}

					parametersByType.Clear();

					foreach (KeyValuePair<AnimatorControllerParameterType, List<int>> pair in typeIndexes)
					{
						AnimatorParameters p = new AnimatorParameters();
						List<int> indexes = pair.Value;

						p.selected = 0;

						p.displayNames = new GUIContent[indexes.Count + 1];
						p.names = new string[indexes.Count + 1];
						p.types = new AnimatorControllerParameterType[indexes.Count + 1];

						p.displayNames[0] = GUIContentCaches.Get("[None]");
						p.names[0] = string.Empty;
						p.types[0] = AnimatorControllerParameterType.Float;

						int count = 1;
						for (int i = 0; i < indexes.Count; i++)
						{
							int index = indexes[i];
							p.displayNames[count] = displayNames[index];
							p.names[count] = names[index];
							p.types[count] = types[index];
							if (index == selected)
							{
								p.selected = count;
							}
							count++;
						}

						parametersByType.Add(pair.Key, p);
					}

					return true;
				}

				return false;
			}

			public void Popup(Rect position, SerializedProperty nameProperty, SerializedProperty typeProperty, GUIContent label)
			{
				label = EditorGUI.BeginProperty(position, label, nameProperty);
				EditorGUI.BeginChangeCheck();
				selected = EditorGUI.Popup(position, label, selected, displayNames);
				if (EditorGUI.EndChangeCheck())
				{
					if (selected >= 0)
					{
						nameProperty.stringValue = names[selected];
						if (typeProperty != null)
						{
							typeProperty.enumValueIndex = EnumUtility.GetIndexFromValue(types[selected]);
						}
					}
				}
				EditorGUI.EndProperty();
			}
		}
		private static readonly Dictionary<AnimatorController, AnimatorParameters> _Parameters = new Dictionary<AnimatorController, AnimatorParameters>();

		private static AnimatorParameters GetAnimatorParameters(AnimatorController animatorController)
		{
			AnimatorParameters parameters;
			if (!_Parameters.TryGetValue(animatorController, out parameters))
			{
				parameters = new AnimatorParameters();
				_Parameters.Add(animatorController, parameters);
			}

			return parameters;
		}

		public static void AnimatorParameterField(Rect position, AnimatorController animatorController, SerializedProperty nameProperty, SerializedProperty typeProperty, GUIContent label, bool hasType = false, AnimatorControllerParameterType parameterType = AnimatorControllerParameterType.Bool)
		{
			if (animatorController != null)
			{
				string name = nameProperty.stringValue;

				AnimatorParameters parameters = GetAnimatorParameters(animatorController);

				if (parameters.Update(animatorController, name))
				{
					if (hasType)
					{
						parameters = parameters.GetTypeParameter(parameterType);
					}
				}
				else
				{
					parameters = null;
				}

				if (parameters != null)
				{
					parameters.Popup(position, nameProperty, typeProperty, label);
				}
				else
				{
					EditorGUI.BeginDisabledGroup(true);

					label = EditorGUI.BeginProperty(position, label, nameProperty);

					EditorGUI.Popup(position, label, -1, new GUIContent[] { GUIContent.none });

					EditorGUI.EndProperty();

					EditorGUI.EndDisabledGroup();
				}
			}
			else
			{
				position.height = EditorGUI.GetPropertyHeight(nameProperty);
				EditorGUI.PropertyField(position, nameProperty, true);
				position.y += position.height;

				if (typeProperty != null)
				{
					if (hasType)
					{
						typeProperty.enumValueIndex = EnumUtility.GetIndexFromValue(parameterType);
					}
					else
					{
						position.y += EditorGUIUtility.standardVerticalSpacing;
						position.height = EditorGUI.GetPropertyHeight(typeProperty);
						EditorGUI.PropertyField(position, typeProperty);
						position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
					}
				}
			}
		}

		public static float GetAnimatorParameterFieldHeight(AnimatorController animatorController, SerializedProperty nameProperty, SerializedProperty typeProperty, bool hasType = false)
		{
			float height = 0.0f;

			if (animatorController != null)
			{
				height = EditorGUIUtility.singleLineHeight;
			}
			else
			{
				height += EditorGUI.GetPropertyHeight(nameProperty);

				if (typeProperty != null)
				{
					if (!hasType)
					{
						height += EditorGUIUtility.standardVerticalSpacing;

						height += EditorGUI.GetPropertyHeight(typeProperty);
					}
				}
			}

			return height;
		}
	}
}