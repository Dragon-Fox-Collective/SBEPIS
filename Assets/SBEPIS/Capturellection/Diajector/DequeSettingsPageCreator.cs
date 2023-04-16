using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class DequeSettingsPageCreator : MonoBehaviour
	{
		public Deque deque;
		public Diajector diajector;
		public DequeSettingsPage settingsPagePrefab;
		
		private List<DequeSettingsPage> settingsPages = new();
		
		public DequeSettingsPage FirstSettingsPage => settingsPages.Count > 0 ? settingsPages[0] : null;
		
		public void Start()
		{
			CreateSettingsPages();
		}
		
		public void CreateSettingsPages()
		{
			List<DequeSettingsPageLayout> layouts = deque.definition.GetNewSettingsPageLayouts().ToList();
			foreach ((int i, DequeSettingsPageLayout layout) in layouts.Enumerate())
			{
				DequeSettingsPage page = Instantiate(settingsPagePrefab, diajector.mainPage.transform.parent);
				layout.transform.SetParent(page.settingsParent, false);
				
				if (i == 0) Destroy(page.prevButton.transform.parent.gameObject);
				if (i == layouts.Count - 1) Destroy(page.nextButton.transform.parent.gameObject);
				
				settingsPages.Add(page);
			}
			for (int i = 0; i < settingsPages.Count; i++)
			{
				DequeSettingsPage page = settingsPages[i];
				page.backButton.onGrab.AddListener(diajector.ChangePageAction(diajector.mainPage));
				if (i > 0) page.prevButton.onGrab.AddListener(diajector.ChangePageAction(settingsPages[i - 1].Page));
				if (i < settingsPages.Count - 1) page.nextButton.onGrab.AddListener(diajector.ChangePageAction(settingsPages[i + 1].Page));
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
