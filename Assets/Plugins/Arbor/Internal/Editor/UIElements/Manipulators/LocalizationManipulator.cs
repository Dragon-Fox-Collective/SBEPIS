//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArborEditor.UIElements
{
	internal sealed class LocalizationManipulator : Manipulator
	{
		public enum TargetText
		{
			Text,
			Tooltip,
		}

		private string _Word;
		public string word
		{
			get
			{
				return _Word;
			}
			set
			{
				if (_Word != value)
				{
					_Word = value;

					OnChangedLanguage();
				}
			}
		}

		private TargetText _TargetText;
		public TargetText targetText
		{
			get
			{
				return _TargetText;
			}
			set
			{
				if (_TargetText != value)
				{
					_TargetText = value;

					OnChangedLanguage();
				}
			}
		}

		public LocalizationManipulator(string word, TargetText targetText)
		{
			_Word = word;
			_TargetText = targetText;
		}

		protected override void RegisterCallbacksOnTarget()
		{
			target.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);

			if (target.parent != null)
			{
				RegisterCallbackOnLocalization();
			}
			OnChangedLanguage();
		}

		void RegisterCallbackOnLocalization()
		{
			LanguageManager.onRebuild += OnChangedLanguage;
			ArborSettings.onChangedLanguage += OnChangedLanguage;
		}

		void UnregisterCallbackFromLocalization()
		{
			LanguageManager.onRebuild -= OnChangedLanguage;
			ArborSettings.onChangedLanguage -= OnChangedLanguage;
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			target.UnregisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

			UnregisterCallbackFromLocalization();
		}

		void OnAttachToPanel(AttachToPanelEvent e)
		{
			RegisterCallbackOnLocalization();
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			UnregisterCallbackFromLocalization();
		}

		void OnChangedLanguage()
		{
			switch (_TargetText)
			{
				case TargetText.Text:
					if (target is TextElement textElement)
					{
						textElement.text = Localization.GetWord(_Word);
					}
					else if (target is Toggle toggle)
					{
						toggle.text = Localization.GetWord(_Word);
					}
					break;
				case TargetText.Tooltip:
					target.tooltip = Localization.GetWord(_Word);
					break;
			}
		}
	}
}