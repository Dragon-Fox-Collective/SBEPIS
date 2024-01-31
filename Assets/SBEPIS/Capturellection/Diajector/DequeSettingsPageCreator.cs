using System.Collections.Generic;
using System.Linq;
using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class DequeSettingsPageCreator : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere] private Deque deque;
		[SerializeField, Anywhere] private Diajector diajector;
		[SerializeField, Anywhere] private DiajectorPage backPage;
		[SerializeField, Anywhere] private DequeSettingsPage settingsPagePrefab;
		
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
				CreatePage(layout, i == 0, i == layouts.Count - 1);
			
			for (int i = 0; i < settingsPages.Count; i++)
				SetupPageButtons(
					i > 0 ? settingsPages[i - 1] : null,
					settingsPages[i],
					i < settingsPages.Count - 1 ? settingsPages[i + 1] : null);
		}
		
		private void CreatePage(DequeSettingsPageLayout layout, bool isFirst, bool isLast)
		{
			DequeSettingsPage page = Instantiate(settingsPagePrefab, backPage.transform.parent);
			SceneRefAttributeValidator.Validate(page.Page, true);
			layout.transform.SetParent(page.settingsParent, false);
			
			if (isFirst) Destroy(page.prevButton.transform.parent.gameObject);
			if (isLast) Destroy(page.nextButton.transform.parent.gameObject);
			
			settingsPages.Add(page);
		}
		
		private void SetupPageButtons(DequeSettingsPage prevPage, DequeSettingsPage page, DequeSettingsPage nextPage)
		{
			page.backButton.OnGrab.AddListener(() => diajector.ChangePage(backPage));
			if (prevPage) page.prevButton.OnGrab.AddListener(() => diajector.ChangePage(prevPage.Page));
			if (nextPage) page.nextButton.OnGrab.AddListener(() => diajector.ChangePage(nextPage.Page));
		}
		
		public void DestroySettingsPages()
		{
			foreach (DequeSettingsPage dequeSettingsPage in settingsPages)
				Destroy(dequeSettingsPage.gameObject);
			settingsPages.Clear();
		}
	}
}
