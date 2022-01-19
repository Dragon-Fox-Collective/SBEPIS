using SBEPIS.Captchalogue;
using UnityEngine;

namespace SBEPIS
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager instance { get; private set; }

		public Captcharoid captcharoid;

		private void Awake()
		{
			instance = this;
		}
	}
}