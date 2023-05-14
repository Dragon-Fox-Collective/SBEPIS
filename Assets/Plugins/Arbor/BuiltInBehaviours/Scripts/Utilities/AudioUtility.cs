//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Audio;

namespace Arbor.Utilities
{
#if ARBOR_DOC_JA
	/// <summary>
	/// オーディオのユーティリティクラス。
	/// </summary>
#else
	/// <summary>
	/// Utility class of audio.
	/// </summary>
#endif
	public static class AudioUtility
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 再生地点を指定してオーディオ再生。
		/// </summary>
		/// <param name="clip">AudioClip</param>
		/// <param name="position">再生地点</param>
		/// <param name="volume">音量</param>
		/// <param name="outputAudioMixerGroup">出力先のAudioMixerGroup。</param>
		/// <param name="spatialBlend">空間ブレンド</param>
#else
		/// <summary>
		/// Specify playback point and play audio.
		/// </summary>
		/// <param name="clip">AudioClip</param>
		/// <param name="position">Playback point</param>
		/// <param name="volume">Volume</param>
		/// <param name="outputAudioMixerGroup">Output AudioMixerGroup.</param>
		/// <param name="spatialBlend">Spatial blend</param>
#endif
		public static void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume, AudioMixerGroup outputAudioMixerGroup, float spatialBlend)
		{
			if (clip == null)
			{
				Debug.LogError("Clip cannot be null.");
				return;
			}

			GameObject gameObject = new GameObject("One shot audio");
			gameObject.transform.position = position;
			AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
			audioSource.clip = clip;
			audioSource.volume = volume;
			audioSource.outputAudioMixerGroup = outputAudioMixerGroup;
			audioSource.spatialBlend = spatialBlend;
			audioSource.Play();
			Object.Destroy(gameObject, clip.length * Time.timeScale);
		}
	}

}
