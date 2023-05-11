//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ARBOR_SUPPORT_UGUI
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// トグルの状態によって遷移する。
	/// </summary>
#else
	/// <summary>
	/// It will transition by the toggle.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/UI/UIToggleTransition")]
	[BuiltInBehaviour]
	public sealed class UIToggleTransition : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 判定をするトグル
		/// </summary>
#else
		/// <summary>
		/// Toggle to the judgment
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Toggle))]
		private FlexibleComponent _Toggle = new FlexibleComponent(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// トグルが変更されたタイミングで遷移するかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether toggle transitions with the changed timing.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _ChangeTimingTransition = new FlexibleBool(false);

#if ARBOR_DOC_JA
		/// <summary>
		/// ToggleがOnの場合の遷移先。<br />
		/// 遷移メソッド : OnStateBegin, Toggle.onValueChanged
		/// </summary>
#else
		/// <summary>
		/// Transition destination when Toggle is On.<br />
		/// Transition Method : OnStateBegin, Toggle.onValueChanged
		/// </summary>
#endif
		[SerializeField] private StateLink _OnState = new StateLink();

#if ARBOR_DOC_JA
		/// <summary>
		/// ToggleがOffの場合の遷移先。<br />
		/// 遷移メソッド : OnStateBegin, Toggle.onValueChanged
		/// </summary>
#else
		/// <summary>
		/// Transition destination when Toggle is Off.<br />
		/// Transition Method : OnStateBegin, Toggle.onValueChanged
		/// </summary>
#endif
		[SerializeField] private StateLink _OffState = new StateLink();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_Toggle")]
		[HideInInspector]
		private Toggle _OldToggle = null;

		[SerializeField]
		[FormerlySerializedAs("_ChangeTimingTransition")]
		[HideInInspector]
		private bool _OldChangeTimingTransition = false;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 3;

		private bool _CacheChangeTimingTransition = false;

		public Toggle cachedToggle
		{
			get
			{
				return _Toggle.value as Toggle;
			}
		}

		void Transition(bool on)
		{
			if (on)
			{
				Transition(_OnState);
			}
			else
			{
				Transition(_OffState);
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			Toggle toggle = cachedToggle;
			if (toggle != null)
			{
				_CacheChangeTimingTransition = _ChangeTimingTransition.value;
				if (!_CacheChangeTimingTransition)
				{
					Transition(toggle.isOn);
				}
				else
				{
					toggle.onValueChanged.AddListener(Transition);
				}
			}
		}

		// Use this for exit state
		public override void OnStateEnd()
		{
			Toggle toggle = cachedToggle;
			if (toggle != null)
			{
				if (_CacheChangeTimingTransition)
				{
					toggle.onValueChanged.RemoveListener(Transition);
				}
			}
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Toggle = (FlexibleComponent)_OldToggle;
		}

		void SerializeVer2()
		{
			_Toggle.SetHierarchyIfConstantNull();
		}

		void SerializeVer3()
		{
			_ChangeTimingTransition = (FlexibleBool)_OldChangeTimingTransition;
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
#endif
