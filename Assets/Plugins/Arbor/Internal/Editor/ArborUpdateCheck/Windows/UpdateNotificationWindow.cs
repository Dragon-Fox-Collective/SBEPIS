//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.UpdateCheck
{
	internal sealed class UpdateNotificationWindow : EditorWindow
	{
		private static GUIContent s_LogoContent = null;

		static GUIContent logoContent
		{
			get
			{
				if (s_LogoContent == null)
				{
					s_LogoContent = new GUIContent(Icons.logoIconLarge);
				}
				return s_LogoContent;
			}
		}
		internal static void Open()
		{
			EditorWindow.GetWindowWithRect(typeof(UpdateNotificationWindow), new Rect(100, 100, 570, 400), true, "Arbor Update Check");
		}

		private enum TabType
		{
			Upgrade,
			Release,
		}
		private TabType m_TabType;
		private Vector2 m_ScrollPos;
		private bool m_IsUpdatable = false;

		private void OnEnable()
		{
			ArborUpdateCheck updateCheck = ArborUpdateCheck.instance;

			m_IsUpdatable = false;
			if (updateCheck.isUpgrade)
			{
				m_TabType = TabType.Upgrade;
				m_IsUpdatable = true;
			}
			else if (updateCheck.isUpdated)
			{
				m_TabType = TabType.Release;
				m_IsUpdatable = true;
			}
		}

		private void OnGUI()
		{
			ArborUpdateCheck updateCheck = ArborUpdateCheck.instance;
			UpdateInfo updateInfo = updateCheck.updateInfo;

			using (new GUILayout.VerticalScope())
			{
				if (updateCheck.isUpdated && updateCheck.isUpgrade)
				{
					using (new GUILayout.HorizontalScope())
					{
						GUIContent[] contents = new GUIContent[2];
						contents[0] = EditorContents.upgrade;
						if (updateCheck.isRelease)
						{
							contents[1] = EditorContents.release;
						}
						else
						{
							contents[1] = EditorContents.patch;
						}
						m_TabType = (TabType)GUILayout.Toolbar((int)m_TabType, contents);
					}
				}
				else
				{
					GUILayout.Space(10);
				}

				using (new GUILayout.HorizontalScope())
				{
					GUILayout.Space(5);

					Vector2 tempIconSize = EditorGUIUtility.GetIconSize();
					EditorGUIUtility.SetIconSize(new Vector2(64, 64));

					GUILayout.Label(logoContent, GUIStyle.none);

					EditorGUIUtility.SetIconSize(tempIconSize);

					GUILayout.Space(5);

					using (new GUILayout.VerticalScope())
					{
						if (m_IsUpdatable)
						{
							switch (m_TabType)
							{
								case TabType.Upgrade:
									{
										GUILayout.Label("UPGRADE", "HeaderLabel");

										string updateMessage = string.Format(Localization.GetWord("ArborUpdateNotification.UpgradeMessage"), ArborVersion.version, updateInfo.Upgrade.Version);
										GUILayout.Label(updateMessage, "WordWrappedLabel", GUILayout.Width(405));

										GUILayout.Space(20);
										m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(405), GUILayout.Height(200));
										GUILayout.Label(updateInfo.Upgrade.Message.GetText(), "WordWrappedLabel");
										EditorGUILayout.EndScrollView();

										GUILayout.Space(20);
										using (new GUILayout.HorizontalScope())
										{
											ReleaseInfo releaseInfo = updateInfo.Upgrade;
											if (GUILayout.Button(EditorContents.assetStore, GUILayout.Width(200)))
											{
												releaseInfo.OpenAssetStore();
											}
											if (GUILayout.Button(EditorContents.releaseNotes, GUILayout.Width(200)))
											{
												releaseInfo.OpenReleaseNote();
											}
										}
									}
									break;
								case TabType.Release:
									{
										GUILayout.Label("NEW VERSION", "HeaderLabel");

										string latestVersion = string.Empty;
										string message = string.Empty;
										if (updateCheck.isRelease)
										{
											latestVersion = updateInfo.Release.Version;
											message = updateInfo.Release.Message.GetText();
										}
										else
										{
											latestVersion = updateInfo.Patch.Version + " (Patch)";
											message = updateInfo.Patch.Message.GetText();
										}

										string updateMessage = string.Format(Localization.GetWord("ArborUpdateNotification.UpdateMessage"), ArborVersion.version, latestVersion);
										GUILayout.Label(updateMessage, "WordWrappedLabel", GUILayout.Width(405));

										GUILayout.Space(20);
										m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(405), GUILayout.Height(200));
										GUILayout.Label(message, "WordWrappedLabel");
										EditorGUILayout.EndScrollView();

										GUILayout.Space(20);
										using (new GUILayout.HorizontalScope())
										{
											if (updateCheck.isRelease)
											{
												ReleaseInfo releaseInfo = updateInfo.Release;
												if (GUILayout.Button(EditorContents.assetStore, GUILayout.Width(200)))
												{
													releaseInfo.OpenAssetStore();
												}
												if (GUILayout.Button(EditorContents.releaseNotes, GUILayout.Width(200)))
												{
													releaseInfo.OpenReleaseNote();
												}
											}
											else
											{
												if (GUILayout.Button(EditorContents.downloadPage, GUILayout.Width(200)))
												{
													updateInfo.Patch.OpenDownalodPage();
												}
											}
										}
									}
									break;
							}
						}
						else
						{
							string upToDateMessage = string.Format(Localization.GetWord("ArborUpdateNotification.UpToDateMessage"), ArborVersion.version);
							GUILayout.Label(upToDateMessage, "WordWrappedLabel", GUILayout.Width(300));
						}
					}
				}
			}

#if ARBOR_DEBUG
			GUILayout.FlexibleSpace();

			if (GUILayout.Button("Check for Update"))
			{
				updateCheck.CheckStart(Open,true);
				Close();
			}
#endif
		}
	}
}