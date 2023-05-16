using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class KeyChar : MonoBehaviour, KeyHandler
	{
		[SerializeField] private string key;
		
		public string Handle(string current) => current + key;
	}
}
