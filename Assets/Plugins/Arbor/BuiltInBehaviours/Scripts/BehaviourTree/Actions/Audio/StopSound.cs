//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
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
	public sealed class StopSound : ActionBehaviour, INodeBehaviourSerializationCallbackReceiver
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
		private int _SerializeVersion = 0;

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

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

		protected override void OnExecute()
		{
			Stop();
			FinishExecute(true);
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
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