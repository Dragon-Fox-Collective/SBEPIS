using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(DequeOwner))]
	public class DequeSettingsPageCreator : MonoBehaviour
	{
		public DequeSettingsPage settingsPagePrefab;
		
		private List<DequeSettingsPage> settingsPages = new();
		
		public DequeSettingsPage FirstSettingsPage => settingsPages.Count > 0 ? settingsPages[0] : null;

		private DequeOwner dequeOwner;

		private void Awake()
		{
			dequeOwner = GetComponent<DequeOwner>();
		}

		public void CreateSettingsPages()
		{
			List<DequeSettingsPageLayout> layouts = dequeOwner.Deque.definition.GetNewSettingsPageLayouts().ToList();
			foreach ((int i, DequeSettingsPageLayout layout) in layouts.Enumerate())
			{
				DequeSettingsPage page = Instantiate(settingsPagePrefab, dequeOwner.diajector.mainPage.transform.parent);
				page.ManualAwake();
				layout.transform.SetParent(page.settingsParent, false);
				
				if (i == 0) Destroy(page.prevButton.transform.parent.gameObject);
				if (i == layouts.Count - 1) Destroy(page.nextButton.transform.parent.gameObject);
				
				settingsPages.Add(page);
			}
			for (int i = 0; i < settingsPages.Count; i++)
			{
				DequeSettingsPage page = settingsPages[i];
				page.backButton.onGrab.AddListener(dequeOwner.diajector.ChangePageMethod(dequeOwner.diajector.mainPage));
				if (i > 0) page.prevButton.onGrab.AddListener(dequeOwner.diajector.ChangePageMethod(settingsPages[i - 1].page));
				if (i < settingsPages.Count - 1) page.nextButton.onGrab.AddListener(dequeOwner.diajector.ChangePageMethod(settingsPages[i + 1].page));
			}
		}
		
		public void DestroySettingsPages()
		{
			foreach (DequeSettingsPage dequeSettingsPage in settingsPages)
				Destroy(dequeSettingsPage.gameObject);
			settingsPages.Clear();
		}
	}
}
