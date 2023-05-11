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
	/// Sliderの値を設定します。
	/// </summary>
#else
	/// <summary>
	/// Set the value of Slider.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("UI/UISetSlider")]
	[BuiltInBehaviour]
	public sealed class UISetSlider : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象となるSlider。
		/// </summary>
#else
		/// <summary>
		/// Slider of interest.
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Slider))]
		private FlexibleComponent _Slider = new FlexibleComponent(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 設定する値。
		/// </summary>
#else
		/// <summary>
		/// The value to be set.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Value = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータが変更したときに更新するかどうか(ValueがParameterの時のみ)。
		/// </summary>
#else
		/// <summary>
		/// Whether to update when parameters change(Only when Value is Parameter).
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
		[FormerlySerializedAs("_Value")]
		[HideInInspector]
		private float _OldValue = 0;

		[SerializeField]
		[FormerlySerializedAs("_ChangeTimingUpdate")]
		[HideInInspector]
		private bool _OldChangeTimingUpdate = false;

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

		void UpdateSlider()
		{
			Slider slider = cachedSlider;
			if (slider != null)
			{
				slider.value = _Value.value;
			}
		}

		private Parameter _CachedParameter;
		private Parameter cachedParameter
		{
			get
			{
				if (_CachedParameter == null && _Value.type == FlexiblePrimitiveType.Parameter)
				{
					_CachedParameter = _Value.parameter;
				}
				return _CachedParameter;
			}
		}

		private bool _IsSettedOnChanged;

		void SetOnChanged()
		{
			if (_IsSettedOnChanged)
			{
				ReleaseOnChanged();
				_CachedParameter = null;
			}

			Parameter parameter = cachedParameter;
			if (parameter != null && _ChangeTimingUpdate.value)
			{
				parameter.onChanged += OnChangedParameter;
				_IsSettedOnChanged = true;
			}
		}

		void ReleaseOnChanged()
		{
			Parameter parameter = cachedParameter;
			if (parameter != null && _IsSettedOnChanged)
			{
				parameter.onChanged -= OnChangedParameter;
				_IsSettedOnChanged = false;
			}
		}

		void OnEnable()
		{
			SetOnChanged();
		}

		void OnDisable()
		{
			ReleaseOnChanged();
		}

		protected override void OnValidate()
		{
			base.OnValidate();

			if (Application.isPlaying && isActiveAndEnabled && _IsSettedOnChanged)
			{
				SetOnChanged();
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			UpdateSlider();
		}

		void OnChangedParameter(Parameter parameter)
		{
			UpdateSlider();
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Slider = (FlexibleComponent)_OldSlider;
			_Value = (FlexibleFloat)_OldValue;
		}

		void SerializeVer2()
		{
			_Slider.SetHierarchyIfConstantNull();
		}

		void SerializeVer3()
		{
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
