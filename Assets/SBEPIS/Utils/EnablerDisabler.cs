using System;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Utils
{
	public class EnablerDisabler : MonoBehaviour
	{
		public List<Component> thingsToEnable;

		public void Enable()
		{
			foreach (Component component in thingsToEnable)
				if (component is Behaviour behaviour)
					behaviour.enabled = true;
				else if (component is Rigidbody rigidbody)
					rigidbody.Enable();
				else if (component is Renderer renderer)
					renderer.enabled = true;
				else
					throw new ArgumentException($"{component.GetType()} is not a supported type to enable");
		}

		public void Disable()
		{
			foreach (Component component in thingsToEnable)
				if (component is Behaviour behaviour)
					behaviour.enabled = false;
				else if (component is Rigidbody rigidbody)
					rigidbody.Disable();
				else if (component is Renderer renderer)
					renderer.enabled = false;
				else
					throw new ArgumentException($"{component.GetType()} is not a supported type to disable");
		}
	}
}
