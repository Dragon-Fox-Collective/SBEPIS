using SBEPIS.Interaction.Flatscreen;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(Collider))]
	public class ToggleButtonFlatscreen : ButtonFlatscreen
	{
		public UnityEvent onActivated;
		public UnityEvent onDeactivated;

		public bool isToggled;

		private void OnEnable()
		{
			onPressed.AddListener(Toggle);
		}

		private void OnDisable()
		{
			onPressed.RemoveListener(Toggle);
		}

		private void Start()
		{
			if (isToggled)
				onActivated.Invoke();
		}

		private void Toggle(ItemHolder itemHolder)
		{
			isToggled = !isToggled;
			if (isToggled)
				onActivated.Invoke();
			else
				onDeactivated.Invoke();
		}
	}
}