using TMPro;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public abstract class DequeSettingsPageLayout : MonoBehaviour
	{
		public TMP_Text title;
	}
	
	public abstract class DequeSettingsPageLayout<T> : DequeSettingsPageLayout
	{
		public T Object { get; set; }
	}
}
