using ArborEditor.UpdateCheck;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArborEditor.UIElements
{
	internal class NoGraphElement : VisualElement
	{
		private readonly ArborEditorWindow _Window;

		public NoGraphElement(ArborEditorWindow window)
		{
			_Window = window;

			style.alignItems = Align.Center;
			style.justifyContent = Justify.Center;
			style.overflow = Overflow.Hidden;

			this.StretchToParentSize();

			var label = new Label()
			{
				style =
				{
					marginBottom = 8f,
				}
			};
			label.AddManipulator(new LocalizationManipulator("NoGraphSelected.Message", LocalizationManipulator.TargetText.Text));
			Add(label);

			DropdownButton createButton = null;
			createButton = new DropdownButton(() =>
			{
				_Window.OpenCreateMenu(createButton.worldBound);
			})
			{
				style =
				{
					paddingTop = 3f,
					paddingBottom = 3f,
					marginBottom = 8f,
				}
			};
			createButton.AddManipulator(new LocalizationManipulator("Create", LocalizationManipulator.TargetText.Text));
			Add(createButton);

			float buttonWidth = 130;

			VisualElement buttons = new VisualElement()
			{
				style =
				{
					flexDirection = FlexDirection.Row,
					flexWrap = Wrap.Wrap,
					width = buttonWidth * 3,
				}
			};

			Add(buttons);

			Button assetStore = new Button(() =>
			{
				ArborVersion.OpenAssetStore();
			})
			{
				style =
				{
					width = buttonWidth,
				}
			};
			assetStore.AddToClassList("button-left");
			assetStore.AddManipulator(new LocalizationManipulator("Asset Store", LocalizationManipulator.TargetText.Text));
			buttons.Add(assetStore);

			Button officialSite = new Button(() =>
			{
				Help.BrowseURL(Localization.GetWord("SiteURL"));
			})
			{
				style =
				{
					width = buttonWidth,
				}
			};
			officialSite.AddToClassList("button-middle");
			officialSite.AddManipulator(new LocalizationManipulator("Official Site", LocalizationManipulator.TargetText.Text));
			buttons.Add(officialSite);

			Button releaseNodes = new Button(() =>
			{
				Help.BrowseURL(Localization.GetWord("ReleaseNotesURL"));
			})
			{
				style =
				{
					width = buttonWidth,
				}
			};
			releaseNodes.AddToClassList("button-right");
			releaseNodes.AddManipulator(new LocalizationManipulator("Release Notes", LocalizationManipulator.TargetText.Text));
			buttons.Add(releaseNodes);

			var manual = new Button(() =>
			{
				Help.BrowseURL(Localization.GetWord("ManualURL"));
			})
			{
				style =
				{
					width = buttonWidth,
				}
			};
			manual.AddToClassList("button-left");
			manual.AddManipulator(new LocalizationManipulator("Manual", LocalizationManipulator.TargetText.Text));
			buttons.Add(manual);

			var inspectorReference = new Button(() =>
			{
				Help.BrowseURL(Localization.GetWord("InspectorReferenceURL"));
			})
			{
				style =
				{
					width = buttonWidth,
				}
			};
			inspectorReference.AddToClassList("button-middle");
			inspectorReference.AddManipulator(new LocalizationManipulator("Inspector Reference", LocalizationManipulator.TargetText.Text));
			buttons.Add(inspectorReference);

			var scriptReference = new Button(() =>
			{
				Help.BrowseURL(Localization.GetWord("ScriptReferenceURL"));
			})
			{
				style =
				{
					width = buttonWidth,
				}
			};
			scriptReference.AddToClassList("button-right");
			scriptReference.AddManipulator(new LocalizationManipulator("Script Reference", LocalizationManipulator.TargetText.Text));
			buttons.Add(scriptReference);
		}
	}
}