using System;
using System.Collections.Generic;
using System.Linq;
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

	public static string Join<T>(this string delimiter, IEnumerable<T> enumerable)
	{
		return string.Join(delimiter, enumerable);
	}

	public static string ToDelimString<T>(this IEnumerable<T> enumerable)
	{
		return "[ " + string.Join(", ", enumerable) +  " ]";
	}

	public static T Pop<T>(this List<T> list)
	{
		if (list.Count == 0)
			throw new InvalidOperationException($"Tried to pop an empty list");

		T obj = list[0];
		list.RemoveAt(0);
		return obj;
	}

	public static void Replace(this Transform transform, Transform other)
	{
		transform.transform.parent = other.parent;
		transform.transform.localPosition = other.localPosition;
		transform.transform.localRotation = other.localRotation;
		transform.transform.localScale = other.localScale;
		foreach (Transform child in other)
			child.SetParent(transform, true);
		UnityEngine.Object.Destroy(other.gameObject);
	}

	public static IEnumerable<T> Insert<T>(this IEnumerable<T> enumerable, int index, T element)
	{
		return enumerable.Take(index).Append(element).Concat(enumerable.TakeLast(enumerable.Count() - index));
	}

	// From PhysX
	// indexed rotation around axis, with sine and cosine of half-angle
	private static Quaternion IndexedRotation(int axis, float s, float c)
	{
		Vector3 v = Vector3.zero;
		v[axis] = s;
		return new Quaternion(v[0], v[1], v[2], c);
	}

	// From PhysX. I *sure hope* this just works with 4x4 matrices
	public static Vector3 Diagonalize(this Matrix4x4 m, out Quaternion massFrame)
	{
		// jacobi rotation using quaternions (from an idea of Stan Melax, with fix for precision issues)

		const int MAX_ITERS = 24;

		Quaternion q = Quaternion.identity;
		Matrix4x4 d = Matrix4x4.identity;

		for (int i = 0; i < MAX_ITERS; i++)
		{
			Matrix4x4 axes = Matrix4x4.Rotate(q);
			d = axes.transpose * m * axes;

			float d0 = Mathf.Abs(d[1, 2]), d1 = Mathf.Abs(d[0, 2]), d2 = Mathf.Abs(d[0, 1]);
			int a = d0 > d1 && d0 > d2 ? 0 : d1 > d2 ? 1 : 2; // rotation axis index, from largest off-diagonal element

			int a1 = (a + 1) % 3, a2 = (a1 + 1) % 3;
			if (d[a1, a2] == 0.0f || Mathf.Abs(d[a1, a1] - d[a2, a2]) > 2e6f * Mathf.Abs(2.0f * d[a1, a2]))
				break;

			float w = (d[a1, a1] - d[a2, a2]) / (2.0f * d[a1, a2]); // cot(2 * phi), where phi is the rotation angle
			float absw = Mathf.Abs(w);

			Quaternion r;
			if (absw > 1000)
				r = IndexedRotation(a, 1 / (4 * w), 1); // h will be very close to 1, so use small angle approx instead
			else
			{
				float t = 1 / (absw + Mathf.Sqrt(w * w + 1)); // absolute value of tan phi
				float h = 1 / Mathf.Sqrt(t * t + 1);          // absolute value of cos phi

				Debug.Assert(h != 1); // |w|<1000 guarantees this with typical IEEE754 machine eps (approx 6e-8)
				r = IndexedRotation(a, Mathf.Sqrt((1 - h) / 2) * Mathf.Sign(w), Mathf.Sqrt((1 + h) / 2));
			}

			q = (q * r).normalized;
		}

		massFrame = q;
		return new Vector3(d[0, 0], d[1, 1], d[2, 2]);
	}

	public static Matrix4x4 AddedBy(this Matrix4x4 a, Matrix4x4 b)
	{
		Matrix4x4 rtn = Matrix4x4.zero;
		for (int i = 0; i < 16; i++)
			rtn[i] = a[i] + b[i];
		return rtn;
	}

	public static Matrix4x4 SubtractedBy(this Matrix4x4 a, Matrix4x4 b)
	{
		Matrix4x4 rtn = Matrix4x4.zero;
		for (int i = 0; i < 16; i++)
			rtn[i] = a[i] - b[i];
		return rtn;
	}

	public static Matrix4x4 ScaledBy(this Matrix4x4 a, float b)
	{
		Matrix4x4 rtn = Matrix4x4.zero;
		for (int i = 0; i < 16; i++)
			rtn[i] = a[i] * b;
		return rtn;
	}

	public static Matrix4x4 OuterSquared(this Vector3 a) => new Matrix4x4(
			new Vector4(a.x * a.x, a.x * a.y, a.x * a.z, 0),
			new Vector4(a.y * a.x, a.y * a.y, a.y * a.z, 0),
			new Vector4(a.z * a.x, a.z * a.y, a.z * a.z, 0),
			new Vector4(0, 0, 0, 1));
}
