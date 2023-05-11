//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;
using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	public class TweenBaseInspector : Editor
	{
		TweenBase _Target;

		private SerializedProperty _UpdateMethodProperty;
		private FlexibleEnumProperty<TweenBase.Type> _TypeProperty;
		private SerializedProperty _DurationProperty;
		private SerializedProperty _CurveProperty;
		private SerializedProperty _UseRealtimeProperty;
		private FlexibleNumericProperty _RepeatUntilTransitionProperty;

		protected virtual void OnEnable()
		{
			_Target = target as TweenBase;

			_UpdateMethodProperty = serializedObject.FindProperty("_UpdateMethod");
			_TypeProperty = new FlexibleEnumProperty<TweenBase.Type>(serializedObject.FindProperty("_Type"));
			_DurationProperty = serializedObject.FindProperty("_Duration");
			_CurveProperty = serializedObject.FindProperty("_Curve");
			_UseRealtimeProperty = serializedObject.FindProperty("_UseRealtime");
			_RepeatUntilTransitionProperty = new FlexibleNumericProperty(serializedObject.FindProperty("_RepeatUntilTransition"));
		}

		protected void DrawBase()
		{
			FlexibleType typeFlexibleType = _TypeProperty.type;
			TweenBase.Type tweenType = _TypeProperty.value;

			EditorGUILayout.PropertyField(_UpdateMethodProperty);

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(_TypeProperty.property);
			if (EditorGUI.EndChangeCheck())
			{
				FlexibleType newTypeFlexibleType = _TypeProperty.type;
				TweenBase.Type newTweenType = _TypeProperty.value;
				if (typeFlexibleType != newTypeFlexibleType || tweenType != newTweenType)
				{
					if (newTypeFlexibleType == FlexibleType.Constant && newTweenType == TweenBase.Type.Once)
					{
						_RepeatUntilTransitionProperty.Disconnect();

						serializedObject.ApplyModifiedProperties();

						GUIUtility.ExitGUI();
					}
				}
				typeFlexibleType = newTypeFlexibleType;
				tweenType = newTweenType;
			}

			EditorGUILayout.PropertyField(_DurationProperty);
			EditorGUILayout.PropertyField(_CurveProperty);
			if (!_Target.fixedUpdate && !_Target.forceRealtime)
			{
				EditorGUILayout.PropertyField(_UseRealtimeProperty);
			}

			if (typeFlexibleType != FlexibleType.Constant || tweenType != TweenBase.Type.Once)
			{
				EditorGUILayout.PropertyField(_RepeatUntilTransitionProperty.property);

				FlexiblePrimitiveType primitiveType = _RepeatUntilTransitionProperty.type;
				switch (primitiveType)
				{
					case FlexiblePrimitiveType.Constant:
						{
							SerializedProperty valueProperty = _RepeatUntilTransitionProperty.valueProperty;
							valueProperty.intValue = Mathf.Max(valueProperty.intValue, 1);
						}
						break;
					case FlexiblePrimitiveType.Random:
						{
							SerializedProperty minProperty = _RepeatUntilTransitionProperty.minRangeProperty;
							minProperty.intValue = Mathf.Max(minProperty.intValue, 1);

							SerializedProperty maxProperty = _RepeatUntilTransitionProperty.maxRangeProperty;
							maxProperty.intValue = Mathf.Max(maxProperty.intValue, 1);
						}
						break;
				}

				if (Application.isPlaying && _Target.stateMachine.currentState == _Target.state)
				{
					Rect r = EditorGUILayout.BeginVertical();
					EditorGUI.ProgressBar(r, (float)_Target.repeatCount / (float)_Target.repeatUntilTransition, _Target.repeatCount.ToString());
					GUILayout.Space(16);
					EditorGUILayout.EndVertical();
				}
			}
		}

		public override bool RequiresConstantRepaint()
		{
			return Application.isPlaying && _Target.stateMachine.currentState == _Target.state;
		}
	}
}
