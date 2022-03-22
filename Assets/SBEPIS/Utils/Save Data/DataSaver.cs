using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SBEPIS.Utils
{
	public static class DataSaver
	{
		public static void SaveData(SaveData data)
		{
			using (FileStream file = File.Create(GetFilePath(data)))
				new BinaryFormatter().Serialize(file, data);
			Debug.Log("Saved to " + GetFilePath(data));
		}

		public static void LoadData<T>(ref T data) where T : SaveData
		{
			if (File.Exists(GetFilePath(data)))
			{
				using (FileStream file = File.Open(GetFilePath(data), FileMode.Open))
					data = (T)new BinaryFormatter().Deserialize(file);
				Debug.Log("Loaded");
			}
			else
				Debug.LogWarning("No save data to load");
		}

		public static void ResetData<T>(ref T data) where T : SaveData
		{
			if (File.Exists(GetFilePath(data)))
			{
				File.Delete(GetFilePath(data));
				Debug.Log("Reset");
			}
			else
				Debug.LogWarning("No save data to reset");
		}

		public static string GetFilePath(SaveData data) => $"{Application.persistentDataPath}/{data.filename}.bin";
	}
}
