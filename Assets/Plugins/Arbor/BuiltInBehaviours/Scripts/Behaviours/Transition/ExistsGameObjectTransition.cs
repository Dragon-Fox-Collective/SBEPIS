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
	/// GameObjectが存在しているかどうかで遷移する。
	/// </summary>
#else
	/// <summary>
	/// GameObject is I will transition on whether exists.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/ExistsGameObjectTransition")]
	[BuiltInBehaviour]
	public sealed class ExistsGameObjectTransition : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 存在しているかチェックする対象。
		/// </summary>
#else
		/// <summary>
		/// Subject to check that you are there.
		/// </summary>
#endif
		[SerializeField]
		private List<FlexibleGameObject> _Targets = new List<FlexibleGameObject>();

#if ARBOR_DOC_JA
		/// <summary>
		/// 存在しているかチェックする対象(リストによる入力)
		/// </summary>
#else
		/// <summary>
		/// Subject to check that you are there(Input by list).
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(IList<GameObject>))]
		private InputSlotAny _InputTargets = new InputSlotAny();

#if ARBOR_DOC_JA
		/// <summary>
		/// アクティブかどうかを判定するフラグ
		/// </summary>
#else
		/// <summary>
		/// Flag to determine whether it is active
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _CheckActive = new FlexibleBool(false);

#if ARBOR_DOC_JA
		/// <summary>
		/// Update時にチェックするかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether Update at check.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _CheckUpdate = new FlexibleBool(false);

#if ARBOR_DOC_JA
		/// <summary>
		/// すべて存在した場合の遷移先。<br />
		/// 遷移メソッド : OnStateBegin, OnStateUpdate
		/// </summary>
#else
		/// <summary>
		/// Transition destination when all are present.<br />
		/// Transition Method : OnStateBegin, OnStateUpdate
		/// </summary>
#endif
		[SerializeField] private StateLink _AllExistsState = new StateLink();

#if ARBOR_DOC_JA
		/// <summary>
		/// すべて存在しなかった場合の遷移先。<br />
		/// 遷移メソッド : OnStateBegin, OnStateUpdate
		/// </summary>
#else
		/// <summary>
		/// Transition destination when all it did not exist.<br />
		/// Transition Method : OnStateBegin, OnStateUpdate
		/// </summary>
#endif
		[SerializeField] private StateLink _AllNothingState = new StateLink();

#if ARBOR_DOC_JA
		/// <summary>
		/// ひとつでも存在している場合の遷移先。<br />
		/// 遷移メソッド : OnStateBegin, OnStateUpdate
		/// </summary>
#else
		/// <summary>
		/// Transition destination when at least one exists.<br />
		/// Transition Method : OnStateBegin, OnStateUpdate
		/// /// </summary>
#endif
		[SerializeField]
		private StateLink _SomeExistsState = new StateLink();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_Targets")]
		[HideInInspector]
		private GameObject[] _OldTargets = null;

		[SerializeField]
		[FormerlySerializedAs("_CheckUpdate")]
		[HideInInspector]
		private bool _OldCheckUpdate = false;

		[SerializeField]

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		void CheckTransition()
		{
			int targetCount = _Targets.Count;

			IList<GameObject> inputTargets = null;
			if (_InputTargets.TryGetValue<IList<GameObject>>(out inputTargets) && inputTargets != null)
			{
				targetCount += inputTargets.Count;
			}

			if (targetCount == 0)
			{
				return;
			}

			int existsCount = 0;
			int nothingCount = 0;

			bool checkActive = _CheckActive.value;

			for (int targetIndex = 0; targetIndex < _Targets.Count; targetIndex++)
			{
				var target = _Targets[targetIndex];
				GameObject value = target.value;
				if (value != null && (!checkActive || value.activeInHierarchy))
				{
					existsCount++;
				}
				else
				{
					nothingCount++;
				}
			}

			if (inputTargets != null)
			{
				for (int targetIndex = 0; targetIndex < inputTargets.Count; targetIndex++)
				{
					var target = inputTargets[targetIndex];
					if (target != null && (!checkActive || target.activeInHierarchy))
					{
						existsCount++;
					}
					else
					{
						nothingCount++;
					}
				}
			}

			if (existsCount == targetCount)
			{
				if (Transition(_AllExistsState))
				{
					return;
				}
			}

			if (existsCount > 0)
			{
				if (Transition(_SomeExistsState))
				{
					return;
				}
			}

			if (nothingCount == targetCount)
			{
				if (Transition(_AllNothingState))
				{
					return;
				}
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			CheckTransition();
		}

		// Update is called once per frame
		public override void OnStateUpdate()
		{
			if (_CheckUpdate.value)
			{
				CheckTransition();
			}
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Targets.Clear();
			for (int targetIndex = 0; targetIndex < _OldTargets.Length; targetIndex++)
			{
				var target = _OldTargets[targetIndex];
				_Targets.Add(new FlexibleGameObject(target));
			}

			_CheckUpdate = (FlexibleBool)_OldCheckUpdate;
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

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			Serialize();
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
			Serialize();
		}
	}
}
