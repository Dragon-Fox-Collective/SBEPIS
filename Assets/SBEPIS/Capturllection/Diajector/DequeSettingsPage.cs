using System;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(DequePage))]
	public class DequeSettingsPage : MonoBehaviour
	{
		public DequePage page { get; private set; }

		private void Awake()
		{
			page = GetComponent<DequePage>();
		}

		public void AddSettingsToPage()
		{
			
		}
	}
}
