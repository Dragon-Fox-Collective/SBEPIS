//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Audio;

namespace Arbor.StateMachine.StateBehaviours
{
	using Arbor.Utilities;

#if ARBOR_DOC_JA
	/// <summary>
	/// 指定した地点でサウンドを再生する基本クラス。
	/// </summary>
#else
	/// <summary>
	/// A base class that plays a sound at a specified point.
	/// </summary>
#endif
	public abstract class PlaySoundAtPointBase : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生するAudioClip。
		/// </summary>
#else
		/// <summary>
		/// AudioClip to play.
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(AudioClip))]
		private FlexibleAssetObject _Clip = new FlexibleAssetObject((AudioClip)null);

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生するボリューム
		/// </summary>
#else
		/// <summary>
		/// Volume to play
		/// </summary>
#endif
		[SerializeField, ConstantRange(0, 1)]
		private FlexibleFloat _Volume = new FlexibleFloat(1.0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// 出力先のAudioMixerGroup
		/// </summary>
#else
		/// <summary>
		/// Output AudioMixerGroup
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(AudioMixerGroup))]
		private FlexibleAssetObject _OutputAudioMixerGroup = new FlexibleAssetObject((AudioMixerGroup)null);

#if ARBOR_DOC_JA
		/// <summary>
		/// 立体音響の影響をどれくらい受けるかを設定する。<br/>
		/// 0.0は音を完全に2Dにし、1.0は完全に3Dとなる。
		/// </summary>
#else
		/// <summary>
		/// Set how much the influence of stereophonic sound will be received.<br />
		/// 0.0 makes the sound completely 2D and 1.0 is completely 3D.
		/// </summary>
#endif
		[SerializeField, ConstantRange(0, 1)]
		private FlexibleFloat _SpatialBlend = new FlexibleFloat(0f);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersionBase = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_Clip")]
		[HideInInspector]
		private AudioClip _OldClip = null;

		[SerializeField]
		[FormerlySerializedAs("_Volume")]
		[HideInInspector]
		private float _OldVolume = 1.0f;

		[SerializeField]
		[FormerlySerializedAs("_OutputAudioMixerGroup")]
		[HideInInspector]
		private AudioMixerGroup _OldOutputAudioMixerGroup = null;

		[SerializeField]
		[FormerlySerializedAs("_SpatialBlend")]
		[HideInInspector]
		private float _OldSpatialBlend = 0f;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		protected virtual void Reset()
		{
			_SerializeVersionBase = kCurrentSerializeVersion;
		}

		protected void PlayClipAtPoint(Vector3 position)
		{
			AudioUtility.PlayClipAtPoint(_Clip.value as AudioClip, position, _Volume.value, _OutputAudioMixerGroup.value as AudioMixerGroup, _SpatialBlend.value);
		}

		void SerializeVer1()
		{
			_Clip = (FlexibleAssetObject)_OldClip;
			_Volume = (FlexibleFloat)_OldVolume;
			_OutputAudioMixerGroup = (FlexibleAssetObject)_OldOutputAudioMixerGroup;
			_SpatialBlend = (FlexibleFloat)_OldSpatialBlend;
		}

		void Serialize()
		{
			while (_SerializeVersionBase != kCurrentSerializeVersion)
			{
				switch (_SerializeVersionBase)
				{
					case 0:
						SerializeVer1();
						_SerializeVersionBase++;
						break;
					default:
						_SerializeVersionBase = kCurrentSerializeVersion;
						break;
				}
			}
		}

		public virtual void OnBeforeSerialize()
		{
			Serialize();
		}

		public virtual void OnAfterDeserialize()
		{
			Serialize();
		}
	}
}
