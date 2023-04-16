using System;
using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class DequeSettingsPageCreator : MonoBehaviour
	{
		[SerializeField, Anywhere] private Deque deque;
		[SerializeField, Anywhere] private Diajector diajector;
		[SerializeField, Anywhere] private DiajectorPage backPage;
		[SerializeField, Anywhere] private DequeSettingsPage settingsPagePrefab;
		
		private void OnValidate() => this.ValidateRefs();
		
		private List<DequeSettingsPage> settingsPages = new();
		
		public DequeSettingsPage FirstSettingsPage => settingsPages.Count > 0 ? settingsPages[0] : null;
		
		public void Start()
		{
			CreateSettingsPages();
		}
		
		public void CreateSettingsPages()
		{
			List<DequeSettingsPageLayout> layouts = deque.Definition.GetNewSettingsPageLayouts().ToList();
			foreach ((int i, DequeSettingsPageLayout layout) in layouts.Enumerate())
			{
				DequeSettingsPage page = Instantiate(settingsPagePrefab, backPage.transform.parent);
				layout.transform.SetParent(page.settingsParent, false);
				
				if (i == 0) Destroy(page.prevButton.transform.parent.gameObject);
				if (i == layouts.Count - 1) Destroy(page.nextButton.transform.parent.gameObject);
				
				settingsPages.Add(page);
			}
			for (int i = 0; i < settingsPages.Count; i++)
			{
				DequeSettingsPage page = settingsPages[i];
				page.backButton.onGrab.AddListener(diajector.ChangePageAction(backPage));
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
