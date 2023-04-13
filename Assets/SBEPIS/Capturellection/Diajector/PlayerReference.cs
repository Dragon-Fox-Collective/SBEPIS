using System;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class PlayerReference : MonoBehaviour
	{
		public Transform reference;

		public T GetReferencedComponent<T>()
		{
			if (!reference.TryGetComponent(out T component))
				throw new NullReferenceException($"{this} doesn't have a {typeof(T).Name}");
			return component;
		}
	}
}
