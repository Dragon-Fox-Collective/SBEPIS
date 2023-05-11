//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

namespace Arbor.BehaviourTree.Decorators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Parameterの条件によるループ。
	/// </summary>
#else
	/// <summary>
	/// Loop by condition of Parameter.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("ParameterConditionalLoop")]
	[BuiltInBehaviour]
	public sealed class ParameterConditionalLoop : Decorator, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

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
		[Internal.DocumentType(typeof(ParameterCondition))]
		private ParameterConditionList _ConditionList = new ParameterConditionList();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_Conditions")]
		private List<ParameterConditionLegacy> _OldConditions = new List<ParameterConditionLegacy>();

		#endregion // old

		#endregion // Serialize Fields

		private const int kCurrentSerializeVersion = 1;

		public override bool HasConditionCheck()
		{
			return false;
		}

		bool CheckCondition()
		{
			return _ConditionList.CheckCondition();
		}

		protected override bool OnRepeatCheck()
		{
			return CheckCondition();
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_ConditionList.ImportLegacy(_OldConditions);
			_OldConditions.Clear();
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
					default:
						_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

		protected override void OnPreDestroy()
		{
			if (_OldConditions != null)
			{
				for (int i = 0; i < _OldConditions.Count; i++)
				{
					_OldConditions[i].Destroy();
				}
			}
			_ConditionList.Destroy();
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