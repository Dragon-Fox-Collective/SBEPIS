//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// AudioSourceを停止する。
	/// </summary>
#else
	/// <summary>
	/// Stop AudioSource.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Audio/StopSound")]
	[BuiltInBehaviour]
	public sealed class StopSound : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 停止するAudioSource。
		/// </summary>
#else
		/// <summary>
		/// AudioSource to stop.
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(AudioSource))]
		private FlexibleComponent _AudioSource = new FlexibleComponent(FlexibleHierarchyType.Self);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion;

		#region old

		[FormerlySerializedAs("_AudioSource")]
		[SerializeField]
		[HideInInspector]
		private AudioSource _OldAudioSource = null;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 2;

		public AudioSource cachedAudioSource
		{
			get
			{
				return _AudioSource.value as AudioSource;
			}
		}

		void Stop()
		{
			AudioSource audioSource = cachedAudioSource;
			if (audioSource != null)
			{
				audioSource.Stop();
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			Stop();
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_AudioSource = (FlexibleComponent)_OldAudioSource;
		}

		void SerializeVer2()
		{
			_AudioSource.SetHierarchyIfConstantNull();
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
