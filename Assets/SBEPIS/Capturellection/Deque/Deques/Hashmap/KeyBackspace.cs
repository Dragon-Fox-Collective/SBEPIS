using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class KeyBackspace : MonoBehaviour, KeyHandler
	{
		public string Handle(string current) => current[..^1];
	}
}
