using UnityEngine;

public static class ExtensionMethods
{
	public static float Map(this float value, float inputFrom, float inputTo, float outputFrom, float outputTo)
	{
		return (value - inputFrom) / (inputTo - inputFrom) * (outputTo - outputFrom) + outputFrom;
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
		rigidbody.interpolation = RigidbodyInterpolation.None;
	}

	public static void Enable(this Rigidbody rigidbody)
	{
		rigidbody.isKinematic = false;
		rigidbody.detectCollisions = true;
		rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
	}
}
