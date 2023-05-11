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
	/// スライダーの値によって遷移する。
	/// </summary>
#else
	/// <summary>
	/// It will transition by the value of the slider.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/UI/UISliderTransition")]
	[BuiltInBehaviour]
	public sealed class UISliderTransition : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 判定をするスライダー
		/// </summary>
#else
		/// <summary>
		/// Slider to the judgment
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Slider))]
		private FlexibleComponent _Slider = new FlexibleComponent(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// スライダーが変更されたタイミングで遷移するかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether slider transitions with the changed timing.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _ChangeTimingTransition = new FlexibleBool(false);

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移をする閾値。
		/// </summary>
#else
		/// <summary>
		/// Threshold for the transition.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Threshold = new FlexibleFloat(0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// Thresholdよりも低かった場合の遷移先。<br />
		/// 遷移メソッド : OnStateBegin, Slider.onValueChanged
		/// </summary>
#else
		/// <summary>
		/// Transition destination when it was lower than Threshold.<br />
		/// Transition Method : OnStateBegin, Slider.onValueChanged
		/// </summary>
#endif
		[SerializeField] private StateLink _LessState = new StateLink();

#if ARBOR_DOC_JA
		/// <summary>
		/// Thresholdよりも高かった場合の遷移先。<br />
		/// 遷移メソッド : OnStateBegin, Slider.onValueChanged
		/// </summary>
#else
		/// <summary>
		/// Transition destination when it is higher than Threshold.<br />
		/// Transition Method : OnStateBegin, Slider.onValueChanged
		/// </summary>
#endif
		[SerializeField] private StateLink _GreaterState = new StateLink();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[FormerlySerializedAs("_Slider")]
		[SerializeField]
		[HideInInspector]
		private Slider _OldSlider = null;

		[SerializeField]
		[FormerlySerializedAs("_ChangeTimingTransition")]
		[HideInInspector]
		private bool _OldChangeTimingTransition = false;

		[SerializeField]
		[FormerlySerializedAs("_Threshold")]
		[HideInInspector]
		private float _OldThreshold = 0f;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 3;

		public Slider cachedSlider
		{
			get
			{
				return _Slider.value as Slider;
			}
		}

		void Transition(float value)
		{
			if (value <= _Threshold.value)
			{
				Transition(_LessState);
			}
			else
			{
				Transition(_GreaterState);
			}
		}

		private bool _CacheChangeTimingTransition = false;

		// Use this for enter state
		public override void OnStateBegin()
		{
			Slider slider = cachedSlider;
			if (slider != null)
			{
				_CacheChangeTimingTransition = _ChangeTimingTransition.value;
				if (!_CacheChangeTimingTransition)
				{
					Transition(slider.value);
				}
				else
				{
					slider.onValueChanged.AddListener(Transition);
				}
			}
		}

		// Use this for exit state
		public override void OnStateEnd()
		{
			Slider slider = cachedSlider;
			if (slider != null)
			{
				if (_CacheChangeTimingTransition)
				{
					slider.onValueChanged.RemoveListener(Transition);
				}
			}
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Slider = (FlexibleComponent)_OldSlider;
		}

		void SerializeVer2()
		{
			_Slider.SetHierarchyIfConstantNull();
		}

		void SerializeVer3()
		{
			_ChangeTimingTransition = (FlexibleBool)_OldChangeTimingTransition;
			_Threshold = (FlexibleFloat)_OldThreshold;
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
