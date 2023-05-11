//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 演算結果によって遷移する。
	/// </summary>
#else
	/// <summary>
	/// It will transition by the calculation result
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/CalculatorTransition")]
	[BuiltInBehaviour]
	public sealed class CalculatorTransition : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 判定するコールバックを指定する。
		/// </summary>
#else
		/// <summary>
		/// Specify the callback to judge.
		/// </summary>
#endif
		[SerializeField]
		[StateTriggerMask(StateTriggerFlags.OnStateBegin | StateTriggerFlags.OnStateUpdate | StateTriggerFlags.OnStateLateUpdate)]
		[Internal.DocumentType(typeof(StateTriggerFlags))]
		private FlexibleStateTriggerFlags _TriggerFlags = new FlexibleStateTriggerFlags(StateTriggerFlags.OnStateBegin);

#if ARBOR_DOC_JA
		/// <summary>
		/// 判定条件を設定する。
		/// <list type="bullet">
		/// <item><term>＋ボタン</term><description>条件を追加。</description></item>
		/// </list>
		/// </summary>
#else
		/// <summary>
		/// Set the judgment condition.
		/// <list type="bullet">
		/// <item><term>+ Button</term><description>Add condition.</description></item>
		/// </list>
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(CalculatorCondition))]
		private CalculatorConditionList _ConditionList = new CalculatorConditionList();

#if ARBOR_DOC_JA
		/// <summary>
		/// 比較結果がTrueであれば遷移する。<br />
		/// 遷移メソッド : OnStateBegin
		/// </summary>
#else
		/// <summary>
		/// If the comparison result is True, the transition occurs.<br />
		/// Transition Method : OnStateBegin
		/// </summary>
#endif
		[SerializeField] private StateLink _NextState = new StateLink();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_Condisions")]
		[FormerlySerializedAs("_Conditions")]
		private List<CalculatorConditionLegacy> _OldConditions = new List<CalculatorConditionLegacy>();

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_TriggerFlags")]
		private StateTriggerFlags _OldTriggerFlags = StateTriggerFlags.OnStateBegin;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 3;

		// Use this for enter state
		public override void OnStateBegin()
		{
			CheckTransition(StateTriggerFlags.OnStateBegin);
		}

		public override void OnStateUpdate()
		{
			CheckTransition(StateTriggerFlags.OnStateUpdate);
		}

		public override void OnStateLateUpdate()
		{
			CheckTransition(StateTriggerFlags.OnStateLateUpdate);
		}

		void CheckTransition(StateTriggerFlags flags)
		{
			if ((_TriggerFlags.value & flags) == 0)
			{
				return;
			}

			if (_ConditionList.CheckCondition())
			{
				Transition(_NextState);
			}
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			int conditionCount = _OldConditions.Count;
			for (int conditionIndex = 0; conditionIndex < conditionCount; conditionIndex++)
			{
				CalculatorConditionLegacy condition = _OldConditions[conditionIndex];
				condition.CalculatorTransitionSerializeVer1();
			}
		}

		void SerializeVer2()
		{
			_ConditionList.ImportLegacy(_OldConditions);
			_OldConditions.Clear();
		}

		void SerializeVer3()
		{
			_TriggerFlags = (FlexibleStateTriggerFlags)_OldTriggerFlags;
		}

		void Serialize()
		{
			while (_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_SerializeVersion++;
						break;
					case 1:
						SerializeVer2();
						_SerializeVersion++;
						break;
					case 2:
						SerializeVer3();
						_SerializeVersion++;
						break;
					default:
						_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
			Serialize();
		}

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			Serialize();
		}
	}
}

