using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(DequePage))]
	public class DequeSettingsPage : MonoBehaviour
	{
		public TMP_Text title;
		public RectTransform settingsParent;
		public DequePageSwitcher backButton;
		public DequePageSwitcher nextButton;
		public DequePageSwitcher prevButton;

		public DequePage page { get; private set; }
		
		private void Awake()
		{
			page = GetComponent<DequePage>();
		}
		
		public void Setup()
		{
			
		}
	}
}
