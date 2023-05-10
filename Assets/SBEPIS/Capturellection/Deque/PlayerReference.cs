using System;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class PlayerReference : MonoBehaviour
	{
		[SerializeField, Anywhere] private Transform[] references;
		
		private Dictionary<Type, Component> components = new();
		
		public T GetReferencedComponent<T>() where T : Component
		{
			if (components.ContainsKey(typeof(T)))
				return (T)components[typeof(T)];
			
			foreach (Transform reference in references)
				if (reference.TryGetComponent(out T component))
				{
					components.Add(typeof(T), component);
					return component;
				}
			
			throw new NullReferenceException($"{this} doesn't have reference to a {typeof(T).Name}");
		}
	}
}
