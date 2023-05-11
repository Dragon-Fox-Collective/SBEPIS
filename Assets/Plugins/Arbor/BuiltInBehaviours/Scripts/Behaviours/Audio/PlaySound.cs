﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// AudioSourceを再生する。
	/// </summary>
#else
	/// <summary>
	/// Play AudioSource.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Audio/PlaySound")]
	[BuiltInBehaviour]
	public sealed class PlaySound : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生するAudioSource。
		/// </summary>
#else
		/// <summary>
		/// AudioSource to play.
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(AudioSource))]
		private FlexibleComponent _AudioSource = new FlexibleComponent(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生時にClipを設定するかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether to set Clip during playback.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _IsSetClip = new FlexibleBool(false);

#if ARBOR_DOC_JA
		/// <summary>
		/// 設定するAudioClip。
		/// </summary>
#else
		/// <summary>
		/// AudioClip to set.
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(AudioClip))]
		private FlexibleAssetObject _Clip = new FlexibleAssetObject((AudioClip)null);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[FormerlySerializedAs("_AudioSource")]
		[SerializeField]
		[HideInInspector]
		private AudioSource _OldAudioSource = null;

		[SerializeField]
		[FormerlySerializedAs("_IsSetClip")]
		[HideInInspector]
		private bool _OldIsSetClip = false;

		[SerializeField]
		[FormerlySerializedAs("_Clip")]
		[HideInInspector]
		private AudioClip _OldClip = null;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 3;

		public AudioSource cachedAudioSource
		{
			get
			{
				return _AudioSource.value as AudioSource;
			}
		}

		void Play()
		{
			AudioSource audioSource = cachedAudioSource;
			if (audioSource != null)
			{
				if (_IsSetClip.value)
				{
					audioSource.clip = _Clip.value as AudioClip;
				}
				audioSource.Play();
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			Play();
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

		void SerializeVer3()
		{
			_IsSetClip = (FlexibleBool)_OldIsSetClip;
			_Clip = new FlexibleAssetObject(_OldClip);
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
