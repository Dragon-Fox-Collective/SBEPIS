using System.Linq;
using KBCore.Refs;
using TMPro;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class DequeSettingsPageLayout : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private DequeSettingsPageModule[] modules;
		
		public TMP_Text title;
		
		private object settings;
		public object Settings
		{
			get => settings;
			set
			{
				settings = value;
				modules.ForEach(module => module.Settings = settings);
			}
		}
	}
	
	public abstract class DequeSettingsPageModule : MonoBehaviour
	{
		public object Settings { get; set; }
	}
	
	public abstract class DequeSettingsPageModule<TSettings> : DequeSettingsPageModule
	{
		protected new TSettings Settings => (TSettings)base.Settings;
	}
}
