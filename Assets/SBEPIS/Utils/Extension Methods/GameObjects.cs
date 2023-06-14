using System;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Utils
{
	public static class GameObjects
	{
		public static void SetPositionAndRotation(this Transform transform, Transform other) => transform.SetPositionAndRotation(other.position, other.rotation);
		public static void SetLocalPositionAndRotation(this Transform transform, Transform other) => transform.SetLocalPositionAndRotation(other.localPosition, other.localRotation);
		public static void SetLocalTransforms(this Transform transform, Transform other)
		{
			transform.transform.localPosition = other.localPosition;
			transform.transform.localRotation = other.localRotation;
			transform.transform.localScale = other.localScale;
		}
		
		public static void Replace(this Transform transform, Transform other)
		{
			transform.SetParent(other.parent, false);
			transform.SetLocalTransforms(other);
			foreach (Transform child in other)
				child.SetParent(transform, true);
			UnityEngine.Object.Destroy(other.gameObject);
		}
		
		public static bool IsOnLayer(this GameObject gameObject, int layerMask)
		{
			return ((1 << gameObject.layer) & layerMask) != 0;
		}
		
		public static void SetLayerRecursively(this GameObject gameObject, int layer)
		{
			gameObject.layer = layer;
			foreach (Transform child in gameObject.transform)
				child.gameObject.SetLayerRecursively(layer);
		}
		
		public static void Disable(this Rigidbody rigidbody)
		{
			rigidbody.isKinematic = true;
			rigidbody.detectCollisions = false;
		}
		
		public static void Enable(this Rigidbody rigidbody)
		{
			rigidbody.isKinematic = false;
			rigidbody.detectCollisions = true;
		}
		
		public static bool TryGetComponentInChildren<T>(this Component thisComponent, out T component) where T : Component => component = thisComponent.GetComponentInChildren<T>();

		public static bool TryGetAttachedComponent<T>(this Collider collider, out T component)
		{
			if (!collider || !collider.attachedRigidbody)
			{
				component = default;
				return false;
			}
			
			return collider.attachedRigidbody.TryGetComponent<T>(out component);
		}
		public static T GetAttachedComponent<T>(this Collider collider) => collider ? collider.attachedRigidbody ? collider.attachedRigidbody.GetComponent<T>() : default : default;
		
		public static void PerformOnMaterial(this IEnumerable<Renderer> renderers, Material material, Action<Material> action)
		{
			foreach (Renderer renderer in renderers)
				for (int i = 0; i < renderer.materials.Length; i++)
				{
					string materialName = renderer.materials[i].name;
					if (materialName.EndsWith(" (Instance)") && materialName[..^11] == material.name)
						action.Invoke(renderer.materials[i]);
				}
		}
	}
}
