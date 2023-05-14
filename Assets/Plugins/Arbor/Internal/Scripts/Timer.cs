//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 経過時間の計測を行うクラス。
	/// </summary>
#else
	/// <summary>
	/// A class that measures elapsed time.
	/// </summary>
#endif
	public sealed class Timer
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 再生状態
		/// </summary>
#else
		/// <summary>
		/// Playback state
		/// </summary>
#endif
		public enum PlayState
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// 停止中
			/// </summary>
#else
			/// <summary>
			/// Stopping
			/// </summary>
#endif
			Stopping,

#if ARBOR_DOC_JA
			/// <summary>
			/// 再生中
			/// </summary>
#else
			/// <summary>
			/// Playing
			/// </summary>
#endif
			Playing,

#if ARBOR_DOC_JA
			/// <summary>
			/// 一時停止中
			/// </summary>
#else
			/// <summary>
			/// Pausing
			/// </summary>
#endif
			Pausing,
		}

		private TimeType _TimeType;

		private float _BeginTime;
		private float _AlreadyElapsedTime;

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生状態を取得する。
		/// </summary>
#else
		/// <summary>
		/// Get the playback state.
		/// </summary>
#endif
		public PlayState playState
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 時間タイプ。
		/// </summary>
#else
		/// <summary>
		/// Time type.
		/// </summary>
#endif
		public TimeType timeType
		{
			get
			{
				return _TimeType;
			}
			set
			{
				if (_TimeType == value)
				{
					return;
				}

				float currentElapsedTime = elapsedTime;

				_TimeType = value;

				if (playState == PlayState.Playing)
				{
					_AlreadyElapsedTime = currentElapsedTime;
					SetBeginTime();
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 経過時間を取得する。
		/// </summary>
#else
		/// <summary>
		/// Get the elapsed time.
		/// </summary>
#endif
		public float elapsedTime
		{
			get
			{
				switch (playState)
				{
					case PlayState.Stopping:
						return 0f;
					case PlayState.Playing:
						return GetCurrentTime() - _BeginTime + _AlreadyElapsedTime;
					case PlayState.Pausing:
						return _AlreadyElapsedTime;
				}

				return 0f;
			}
		}

		private float GetCurrentTime()
		{
			return TimeUtility.CurrentTime(_TimeType);
		}

		private void SetBeginTime()
		{
			_BeginTime = GetCurrentTime();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 経過時間の計測を開始する。
		/// </summary>
#else
		/// <summary>
		/// Start measuring the elapsed time.
		/// </summary>
#endif
		public void Start()
		{
			SetBeginTime();
			_AlreadyElapsedTime = 0f;

			playState = PlayState.Playing;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 経過時間の計測を停止する。
		/// </summary>
#else
		/// <summary>
		/// Stop measuring the elapsed time.
		/// </summary>
#endif
		public void Stop()
		{
			playState = PlayState.Stopping;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 経過時間の計測を一時停止する。
		/// </summary>
#else
		/// <summary>
		/// Pause measuring the elapsed time.
		/// </summary>
#endif
		public void Pause()
		{
			if (playState != PlayState.Playing)
			{
				return;
			}

			_AlreadyElapsedTime = elapsedTime;

			playState = PlayState.Pausing;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 経過時間の計測を再開する。
		/// </summary>
#else
		/// <summary>
		/// Resume measuring the elapsed time.
		/// </summary>
#endif
		public void Resume()
		{
			if (playState != PlayState.Pausing)
			{
				return;
			}

			SetBeginTime();

			playState = PlayState.Playing;
		}
	}
}