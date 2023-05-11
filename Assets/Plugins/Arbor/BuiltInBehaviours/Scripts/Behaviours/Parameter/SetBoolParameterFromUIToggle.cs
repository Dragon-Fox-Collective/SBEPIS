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
	/// Toggleの値をParameterに設定する。
	/// </summary>
#else
	/// <summary>
	/// The value of the Toggle set to Parameter.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Parameter/SetBoolParameterFromUIToggle")]
	[BuiltInBehaviour]
	public sealed class SetBoolParameterFromUIToggle : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
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
		[SerializeField] private BoolParameterReference _Parameter = new BoolParameterReference();

#if ARBOR_DOC_JA
		/// <summary>
		/// 参照するToggle
		/// </summary>
#else
		/// <summary>
		/// See to Toggle
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Toggle))]
		private FlexibleComponent _Toggle = new FlexibleComponent(FlexibleHierarchyType.Self);

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

		[FormerlySerializedAs("_Toggle")]
		[SerializeField]
		[HideInInspector]
		private Toggle _OldToggle = null;

		[FormerlySerializedAs("_ChangeTimingUpdate")]
		[SerializeField]
		[HideInInspector]
		private bool _OldChangeTimingUpdate = false;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void UpdateParameter(bool value)
		{
			_Parameter.value = value;
		}

		private Toggle _CacheToggle = null;
		private bool _CacheChangeTimingUpdate = false;

		// Use this for enter state
		public override void OnStateBegin()
		{
			_CacheToggle = _Toggle.value as Toggle;
			_CacheChangeTimingUpdate = _ChangeTimingUpdate.value;

			if (_CacheToggle != null)
			{
				UpdateParameter(_CacheToggle.isOn);

				if (_CacheChangeTimingUpdate)
				{
					_CacheToggle.onValueChanged.AddListener(UpdateParameter);
				}
			}
		}

		public override void OnStateEnd()
		{
			if (_CacheToggle != null && _CacheChangeTimingUpdate)
			{
				_CacheToggle.onValueChanged.RemoveListener(UpdateParameter);
			}
		}

		void SerializeVer1()
		{
			_Toggle = (FlexibleComponent)_OldToggle;
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
