using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Discord
{
	public class DiscordController : MonoBehaviour
	{
		private static readonly int startEpoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

		private Discord discord;

		private void Start()
		{
#if UNITY_EDITOR
			Destroy(this);
#else
			try
			{
				discord = new Discord(948761142950514738, (UInt64)CreateFlags.NoRequireDiscord);
				//InvokeRepeating(nameof(UpdatePresence), 0, 60);
				UpdatePresence();
			}
			catch
			{
				print("No Discord! Destroying");
				Destroy(this);
			}
#endif
		}

		private void Update()
		{
			discord.RunCallbacks();
		}

		private void OnApplicationQuit()
		{
			discord.GetActivityManager().ClearActivity(result => print(result));
		}

		private void UpdatePresence()
		{
			print("Updating presence");
			discord.GetActivityManager().UpdateActivity(new Activity
			{
				Details = "Exploring " + SceneManager.GetActiveScene().name,
				State = "discord.gg/qHREQu7Zxm",
				Timestamps =
				{
					Start = startEpoch
				},
				Assets =
				{
					LargeImage = "sbepis",
					LargeText = "SBEPIS",
					SmallImage = "bitflower",
					SmallText = "sbepis",
				},
				Instance = true,
			}, result => print(result));
		}
	}
}