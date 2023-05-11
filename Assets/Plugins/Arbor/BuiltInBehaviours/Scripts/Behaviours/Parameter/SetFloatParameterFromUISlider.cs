//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ARBOR_SUPPORT_UGUI
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Sliderの値をParameterに設定する。
	/// </summary>
#else
	/// <summary>
	/// Set the value of the Slider to Parameter.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Parameter/SetFloatParameterFromUISlider")]
	[BuiltInBehaviour]
	public sealed class SetFloatParameterFromUISlider : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 設定するパラメータ
		/// </summary>
#else
		/// <summary>
		/// Parameters to be set
		/// </summary>
#endif
		[SerializeField] private FloatParameterReference _Parameter = new FloatParameterReference();

#if ARBOR_DOC_JA
		/// <summary>
		/// 参照するSlider
		/// </summary>
#else
		/// <summary>
		/// See to Slider
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Slider))]
		private FlexibleComponent _Slider = new FlexibleComponent((Slider)null);

#if ARBOR_DOC_JA
		/// <summary>
		/// 変更時に更新するかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether to update at the time of the change.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _ChangeTimingUpdate = new FlexibleBool(false);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_Slider")]
		[HideInInspector]
		private Slider _OldSlider = null;

		[SerializeField]
		[FormerlySerializedAs("_ChangeTimingUpdate")]
		[HideInInspector]
		private bool _OldChangeTimingUpdate = false;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		private Slider _CacheSlider = null;
		private bool _CacheChangeTimingUpdate = false;

		void UpdateParameter(float value)
		{
			_Parameter.value = value;
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			_CacheSlider = _Slider.value as Slider;
			if (_CacheSlider != null)
			{
				UpdateParameter(_CacheSlider.value);

				_CacheChangeTimingUpdate = _ChangeTimingUpdate.value;
				if (_CacheChangeTimingUpdate)
				{
					_CacheSlider.onValueChanged.AddListener(UpdateParameter);
				}
			}
		}

		public override void OnStateEnd()
		{
			if (_CacheChangeTimingUpdate)
			{
				_CacheSlider.onValueChanged.RemoveListener(UpdateParameter);
			}
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Slider = (FlexibleComponent)_OldSlider;
			_ChangeTimingUpdate = (FlexibleBool)_OldChangeTimingUpdate;
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
#endif
