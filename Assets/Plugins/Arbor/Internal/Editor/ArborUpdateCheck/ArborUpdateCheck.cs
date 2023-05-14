//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using System;

namespace ArborEditor.UpdateCheck
{
	[System.Serializable]
	internal sealed class ArborUpdateCheck : Arbor.ScriptableSingleton<ArborUpdateCheck>, IUpdateCallback
	{
		private const string kUpdateSkipVersionKey = "ArborEditor.UpdateSkipVersionString";

		private static string s_SkipVersion = null;
		public static string skipVersion
		{
			get
			{
				if (s_SkipVersion == null && EditorPrefs.HasKey(kUpdateSkipVersionKey))
				{
					s_SkipVersion = EditorPrefs.GetString(kUpdateSkipVersionKey, "");
				}
				return s_SkipVersion;
			}
			set
			{
				if (value != null && s_SkipVersion != value)
				{
					s_SkipVersion = value;
					EditorPrefs.SetString(kUpdateSkipVersionKey, value);
				}
			}
		}

#if ARBOR_DLL
		static readonly System.Reflection.PropertyInfo s_ResultPropertyInfo;
		static readonly System.Reflection.PropertyInfo s_IsNetworkErrorPropertyInfo;
		static readonly System.Reflection.PropertyInfo s_IsHttpErrorPropertyInfo;

		static ArborUpdateCheck()
		{
			System.Type unityWebRequestType = typeof(UnityWebRequest);

			s_ResultPropertyInfo = unityWebRequestType.GetProperty("result", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

			s_IsNetworkErrorPropertyInfo = unityWebRequestType.GetProperty("isNetworkError", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			if (s_IsNetworkErrorPropertyInfo == null)
			{
				s_IsNetworkErrorPropertyInfo = unityWebRequestType.GetProperty("isError", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			}

			s_IsHttpErrorPropertyInfo = unityWebRequestType.GetProperty("isHttpError", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
		}
#endif

		private UnityWebRequest _Request;

		public event Action onDone;

		private bool _IsDone;
		private UpdateInfo _UpdateInfo;

		public bool isDone
		{
			get
			{
				return _IsDone;
			}
		}

		public UpdateInfo updateInfo
		{
			get
			{
				return _UpdateInfo;
			}
		}

		public string latestVersion
		{
			get
			{
				string currentVersion = ArborVersion.version;

				if (!isDone || _UpdateInfo == null)
				{
					return currentVersion;
				}

				switch (ArborVersion.buildType)
				{
					case VersionInfo.BuildType.Release:
						if (_UpdateInfo.Release.Version != currentVersion)
						{
							return _UpdateInfo.Release.Version;
						}
						else if (_UpdateInfo.Patch.BaseVersion == currentVersion)
						{
							return _UpdateInfo.Patch.Version;
						}
						break;
					case VersionInfo.BuildType.Patch:
						if (_UpdateInfo.Release.Version != ArborVersion.baseVersion)
						{
							return _UpdateInfo.Release.Version;
						}
						if (_UpdateInfo.Patch.Version != currentVersion)
						{
							return _UpdateInfo.Patch.Version;
						}
						break;
				}

				return currentVersion;
			}
		}

		public bool isUpdated
		{
			get
			{
				if (!isDone || _UpdateInfo == null)
				{
					return false;
				}

				string currentVersion = ArborVersion.version;

				string latestVersion = this.latestVersion;

				return (currentVersion != latestVersion && skipVersion != latestVersion);
			}
		}

		public bool isRelease
		{
			get
			{
				if (!isDone || _UpdateInfo == null)
				{
					return false;
				}

				return (ArborVersion.version != _UpdateInfo.Release.Version || _UpdateInfo.Release.Version != _UpdateInfo.Patch.BaseVersion) && skipVersion != _UpdateInfo.Release.Version;
			}
		}

		public bool isUpgrade
		{
			get
			{
				if (!isDone || _UpdateInfo == null || !_UpdateInfo.Upgrade.IsValid())
				{
					return false;
				}

				return _UpdateInfo.Upgrade.IsValid();
			}
		}

		public void CheckStart(bool force = false)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable || _Request != null || (_IsDone && !force))
			{
				return;
			}

			_Request = UnityWebRequest.Get(ArborVersion.updateCheckURL);
			_Request.SendWebRequest();
			_IsDone = false;
			_UpdateInfo = null;

			EditorCallbackUtility.RegisterUpdateCallback(this);
		}

		void IUpdateCallback.OnUpdate()
		{
			if (!_Request.isDone)
			{
				return;
			}

			_IsDone = true;

			bool isError = false;
#if ARBOR_DLL
			if(s_ResultPropertyInfo != null)
			{
				int result = (int)s_ResultPropertyInfo.GetValue(_Request, null);
				isError = isError || result != 1;
			}
			else
			{
				if (s_IsNetworkErrorPropertyInfo != null)
				{
					isError = isError || (bool)s_IsNetworkErrorPropertyInfo.GetValue(_Request, null);
				}
				if (s_IsHttpErrorPropertyInfo != null)
				{
					isError = isError || (bool)s_IsHttpErrorPropertyInfo.GetValue(_Request, null);
				}
			}
#elif UNITY_2020_2_OR_NEWER
			UnityWebRequest.Result result = _Request.result;
			isError = result != UnityWebRequest.Result.Success;
#else
			isError = _Request.isNetworkError || _Request.isHttpError;
#endif

			if (!isError)
			{
				string json = _Request.downloadHandler.text;
				try
				{
					_UpdateInfo = JsonUtility.FromJson<UpdateInfo>(json);
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
			}
			else
			{
				_UpdateInfo = null;
			}
			_Request.Dispose();
			_Request = null;

			EditorCallbackUtility.UnregisterUpdateCallback(this);

			onDone?.Invoke();
			onDone = null;
		}
	}
}