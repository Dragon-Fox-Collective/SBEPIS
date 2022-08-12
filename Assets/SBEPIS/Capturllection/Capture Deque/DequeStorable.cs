using SBEPIS.Controller;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Grabbable))]
	public class DequeStorable : MonoBehaviour
	{
		public bool isStored { get; set; }
		public Grabbable grabbable { get; private set; }

		private void Awake()
		{
			grabbable = GetComponent<Grabbable>();
		}
	}
}
