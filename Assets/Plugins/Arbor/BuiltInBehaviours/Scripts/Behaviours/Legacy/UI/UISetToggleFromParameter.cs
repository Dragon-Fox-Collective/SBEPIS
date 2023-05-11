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
	/// ToggleをParameterから設定します。
	/// </summary>
#else
	/// <summary>
	/// Set the Toggle from Parameter.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Legacy/UI/UISetToggleFromParameter")]
	[BuiltInBehaviour]
	public sealed class UISetToggleFromParameter : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象となるToggle。<br/>
		/// </summary>
#else
		/// <summary>
		/// Toggle of interest.<br/>
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Toggle))]
		private FlexibleComponent _Toggle = new FlexibleComponent(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 参照するParameter
		/// </summary>
#else
		/// <summary>
		/// Reference Parameter
		/// </summary>
#endif
		[SerializeField] private BoolParameterReference _Parameter = new BoolParameterReference();

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータが変更したときに更新するかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether to update when parameters change.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _ChangeTimingUpdate = new FlexibleBool(false);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_Toggle")]
		[HideInInspector]
		private Toggle _OldToggle = null;

		[SerializeField]
		[FormerlySerializedAs("_ChangeTimingUpdate")]
		[HideInInspector]
		private bool _OldChangeTimingUpdate = false;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		void UpdateToggle(Parameter parameter)
		{
			Toggle toggle = _Toggle.value as Toggle;
			if (toggle != null && parameter != null)
			{
				toggle.isOn = parameter.boolValue;
			}
		}

		void OnChangedParameter(Parameter parameter)
		{
			UpdateToggle(parameter);
		}

		private Parameter _CachedParameter;
		private Parameter cachedParameter
		{
			get
			{
				if (_CachedParameter == null)
				{
					_CachedParameter = _Parameter.parameter;
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
			UpdateToggle(cachedParameter);
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Toggle = (FlexibleComponent)_OldToggle;
			_Toggle.SetHierarchyIfConstantNull();

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
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
		}
	}
}
#endif
