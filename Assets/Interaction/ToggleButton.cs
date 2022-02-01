using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(Collider))]
	public class ToggleButton : Button
	{
		public UnityEvent onActivated;
		public UnityEvent onDeactivated;

		public bool activated;

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
			if (activated)
				onActivated.Invoke();
		}

		private void Toggle(ItemHolder itemHolder)
		{
			activated = !activated;
			if (activated)
				onActivated.Invoke();
			else
				onDeactivated.Invoke();
		}
	}
}