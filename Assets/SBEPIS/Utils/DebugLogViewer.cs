using UnityEngine;

public class DebugLogViewer : MonoBehaviour
{
	private string log = "";

	private void OnEnable()
	{
		Application.logMessageReceived += Log;
	}

	void OnDisable()
	{
		Application.logMessageReceived -= Log;
	}

	public void Log(string logString, string stackTrace, LogType type)
	{
		if (type != LogType.Log)
			log = stackTrace + "\n" + log;
		log = logString + "\n" + log;
		if (log.Length > 5000)
			log = log[..4000];
	}

	void OnGUI()
	{
		log = GUI.TextArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20), log);
	}
}