using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ExtensionMethods
{
	public static float Map(this float value, float inputFrom, float inputTo, float outputFrom, float outputTo)
	{
		return (value - inputFrom) / (inputTo - inputFrom) * (outputTo - outputFrom) + outputFrom;
	}
	
	/// <summary>
	/// Returns positive mod of value
	/// </summary>
	public static float Mod(this float value, float mod)
	{
		float res = value % mod;
		if (res < 0) res += mod;
		return res;
	}
	
	/// <summary>
	/// Returns positive mod of value
	/// </summary>
	public static int Mod(this int value, int mod)
	{
		int res = value % mod;
		if (res < 0) res += mod;
		return res;
	}
	
	/// <summary>
	/// Returns mod of value between -mod/2 and mod/2
	/// </summary>
	public static float ModAround(this float value, float mod)
	{
		float res = value.Mod(mod);
		if (res > mod / 2)
			res -= mod;
		return res;
	}
	
	public static int Add(int a, int b) => a + b;
	public static float Add(float a, float b) => a + b;
	public static Vector3 Add(Vector3 a, Vector3 b) => a + b;
	public static Vector3 Max(Vector3 a, Vector3 b) => new(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z));
	public static Vector3 Multiply(Vector3 a, Vector3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);
	
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
	
	public static string Join<T>(this string delimiter, IEnumerable<T> source) => string.Join(delimiter, source);
	public static string ToDelimString<T>(this IEnumerable<T> source) => "[" + ", ".Join(source) +  "]";
	
	public static T Pop<T>(this IList<T> list)
	{
		T obj = list[0];
		list.RemoveAt(0);
		return obj;
	}
	
	public static IEnumerable<(int index, T item)> Enumerate<T>(this IEnumerable<T> source)
	{
		int i = 0;
		foreach (T item in source)
			yield return (i++, item);
	}
	
	public static bool All(this IEnumerable<bool> source) => source.All(b => b);
	public static bool Any(this IEnumerable<bool> source) => source.Any(b => b);
	
	public static TResult InvokeWith<T1, T2, TResult>(this Func<T1, T2, TResult> func, (T1, T2) args) => func(args.Item1, args.Item2);
	public static void InvokeWith<T1, T2>(this Action<T1, T2> func, (T1, T2) args) => func(args.Item1, args.Item2);
	
	public static async UniTask<TAccumulate> Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, UniTask<TAccumulate>> func)
	{
		TAccumulate value = seed;
		// ReSharper disable once LoopCanBeConvertedToQuery
		foreach (TSource item in source)
			value = await func(value, item);
		return value;
	}
	
	public static async UniTask ForEach<T>(this IEnumerable<T> source, Func<T, UniTask> func)
	{
		foreach (T t in source)
			await func(t);
	}
	
	public static async UniTask ForEach<T1, T2>(this IEnumerable<(T1, T2)> source, Func<T1, T2, UniTask> func)
	{
		foreach ((T1 t1, T2 t2) in source)
			await func(t1, t2);
	}
	
	public static async UniTask<IEnumerable<TResult>> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, UniTask<TResult>> action)
	{
		IEnumerable<TResult> result = Enumerable.Empty<TResult>();
		// ReSharper disable once LoopCanBeConvertedToQuery
		foreach (TSource item in source)
			result = result.Append(await action(item));
		return result;
	}
	
	public static UniTask<IEnumerable<TResult>> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, UniTask<IEnumerable<TResult>>> action) => source.Select(action).ContinueWith(result => result.Flatten());
	
	public static UniTask<IEnumerable<T>> Where<T>(this UniTask<IEnumerable<T>> source, Func<T, bool> predicate) => source.ContinueWith(result => result.Where(predicate));
	
	public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source) => source.SelectMany(item => item);
	
	public static IEnumerable<T> EnumerableOf<T>(T item) { yield return item; }
	public static IEnumerable<T> EnumerableOf<T>(params T[] items) => items;
	
	public static IEnumerable<T> Process<T>(this IEnumerable<T> source, Action<T> action)
	{
		foreach (T item in source)
		{
			action(item);
			yield return item;
		}
	}
	
	public static IEnumerable<T> Pivot<T>(this IEnumerable<T> source, int index)
	{
		IEnumerable<T> first = Enumerable.Empty<T>();
		int i = 0;
		foreach (T item in source)
		{
			if (i++ < index)
				first = first.Append(item);
			else
				yield return item;
		}
		foreach (T item in first)
			yield return item;
	}
	
	public static (IEnumerable<T>, IEnumerable<T>) Split<T>(this IEnumerable<T> source, int index)
	{
		IEnumerable<T> first = Enumerable.Empty<T>();
		IEnumerable<T> last = Enumerable.Empty<T>();
		int i = 0;
		foreach (T item in source)
		{
			if (i >= index)
				first = first.Append(item);
			else
				last = last.Append(item);
			i++;
		}
		return (first, last);
	}
	
	public static TValue GetEnsured<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> newValueFactory)
	{
		if (!dictionary.ContainsKey(key))
			dictionary.Add(key, newValueFactory());
		return dictionary[key];
	}
	
	public static TValue GetEnsured<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) where TValue : new() => GetEnsured(dictionary, key, () => new TValue());

	public static T[] Fill<T>(this T[] array, T item)
	{
		T[] newArray = new T[array.Length];
		Array.Fill(newArray, item);
		return newArray;
	}

	public static Bounds Containing(this Bounds a, Bounds b)
	{
		Bounds bounds = new(a.center, a.size);
		bounds.Encapsulate(b);
		return bounds;
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
	
	public static T RandomElement<T>(this IList<T> source) => source[Random.Range(0, source.Count)];
	public static void AddRange<T>(this IList<T> source, int count, Func<T> factory) => Enumerable.Range(0, count).Select(_ => factory()).ForEach(source.Add);
	
	// Note that lhs * rhs means rotating by lhs and then by rhs
	public static Quaternion TransformRotation(this Transform from, Quaternion delta) => from.rotation * delta; // from * delta = to
	public static Quaternion InverseTransformRotation(this Transform from, Quaternion to) => from.rotation.Inverse() * to; // delta = from-1 * to

	public static Vector3 CenterOfRotation(Vector3 pos1, Vector3 up1, Vector3 pos2, Vector3 up2)
	{
		float theta = Vector3.Angle(up1, up2) * Mathf.Deg2Rad;
		float height = Mathf.Sqrt((pos2 - pos1).sqrMagnitude / (1 - Mathf.Cos(theta)) / 2f); // cosine rule frickery
		bool centerIsDown = ((pos1 + up1) - (pos2 + up2)).sqrMagnitude > (pos2 - pos1).sqrMagnitude;
		if (centerIsDown)
			height *= -1;
		return pos2 + height * up2; // biased toward pos2 but idc
	}

	public static Quaternion Inverse(this Quaternion quaternion) => Quaternion.Inverse(quaternion);

	public static Vector3 ToEulersAngleAxis(this Quaternion quaternion)
	{
		if (quaternion.w < 0) quaternion = quaternion.Select(x => -x);
		quaternion.ToAngleAxis(out float angle, out Vector3 axis);
		return angle * Mathf.Deg2Rad * axis;
	}
	
	public static IEnumerable<T> Insert<T>(this IEnumerable<T> enumerable, int index, T newItem)
	{
		int i = 0;
		foreach (T item in enumerable)
		{
			if (i++ == index)
				yield return newItem;
			yield return item;
		}
	}
	
	public static T ElementAtOrLast<T>(this IEnumerable<T> enumerable, int index)
	{
		T last = default;
		int i = 0;
		foreach (T item in enumerable)
		{
			last = item;
			if (i == index)
				return last;
			i++;
		}
		return last;
	}
	
	public static IEnumerable<(T, TSecond)> Zip<T, TSecond>(this IEnumerable<T> first, IEnumerable<TSecond> second)
	{
		return first.Zip(second, (firstItem, secondItem) => (firstItem, secondItem));
	}
	
	public static void Shuffle<T>(this IList<T> list)  
	{
		for (int n = list.Count - 1; n > 1; n--)
			list.Swap(n, Random.Range(0, n + 1));
	}
	
	public static void Swap<T>(this IList<T> list, int i1, int i2) => (list[i1], list[i2]) = (list[i2], list[i1]);
	
	public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
	{
		foreach (T t in source)
			action(t);
	}
	
	public static void ForEach<T1, T2>(this IEnumerable<(T1, T2)> source, Action<T1, T2> action)
	{
		foreach ((T1 item1, T2 item2) in source)
			action(item1, item2);
	}
	
	public static IEnumerable<IEnumerable<T>> Divide<T>(this IList<T> source, int count)
	{
		for (int i = 0; i * count < source.Count; i++)
			yield return source.Skip(i * count).Take(count);
	}
	
	public static IEnumerable<float> AsEnumerable(this Vector3 vector)
	{
		yield return vector.x;
		yield return vector.y;
		yield return vector.z;
	}
	
	public static Vector3 ToVector3(this IEnumerable<float> enumerable)
	{
		Vector3 vector = new();
		int i = 0;
		foreach (float x in enumerable)
		{
			vector[i++] = x;
			if (i == 3)
				break;
		}
		return vector;
	}
	
	public static Vector3 Select(this Vector3 vector, Func<float, float> func) => new(func(vector.x), func(vector.y), func(vector.z));
	public static Vector3 SelectIndex(this Vector3 vector, Func<int, float, float> func) => new(func(0, vector.x), func(1, vector.y), func(2, vector.z));
	public static Vector3 SelectVectorIndex(Func<int, float> func) => new(func(0), func(1), func(2));
	
	public static float Aggregate(this Vector3 vector, Func<float, float, float> func) => vector.Aggregate(0, func);
	public static float Aggregate(this Vector3 vector, float seed, Func<float, float, float> func) => func(func(func(seed, vector.x), vector.y), vector.z);
	
	public static bool Any(this Vector3 vector, Func<float, bool> func) => vector.AsEnumerable().Any(func);
	public static bool All(this Vector3 vector, Func<float, bool> func) => vector.AsEnumerable().All(func);
	
	public static Vector3 Sum<T>(this IEnumerable<T> source, Func<T, Vector3> func) => source.Aggregate(Vector3.zero, (sum, x) => sum + func(x));
	public static Vector3 Sum(this IEnumerable<Vector3> source) => source.Aggregate(Vector3.zero, (sum, x) => sum + x);
	
	public static Quaternion Select(this Quaternion quaternion, Func<float, float> func) => new(func(quaternion.x), func(quaternion.y), func(quaternion.z), func(quaternion.w));
	
	public static Vector3x2 Select(this Vector3x2 vector, Func<Vector3, Vector3> func) => new(func(vector.x), func(vector.y));
	public static Vector3x2 SelectIndex(this Vector3x2 vector, Func<int, Vector3, Vector3> func) => new(func(0, vector.x), func(1, vector.y));
	public static Vector2 AggregateIndex(this Vector3x2 vector, Func<int, Vector3, float> func) => new(func(0, vector.x), func(1, vector.y));
	public static Vector3x2 Select(this Vector3x2 vector, Func<float, float> func) => vector.Select(v => v.Select(func));
	
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

		const int maxIters = 24;

		Quaternion q = Quaternion.identity;
		Matrix4x4 d = Matrix4x4.identity;

		for (int i = 0; i < maxIters; i++)
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

				// ReSharper disable once CompareOfFloatsByEqualityOperator
				Debug.Assert(h != 1); // |w|<1000 guarantees this with typical IEEE754 machine eps (approx 6e-8)
				r = IndexedRotation(a, Mathf.Sqrt((1 - h) / 2) * Mathf.Sign(w), Mathf.Sqrt((1 + h) / 2));
			}

			q = (q * r).normalized;
		}

		massFrame = q;
		return new Vector3(d[0, 0], d[1, 1], d[2, 2]);
	}

	public static Matrix4x4 Plus(this Matrix4x4 a, Matrix4x4 b)
	{
		Matrix4x4 rtn = Matrix4x4.zero;
		for (int i = 0; i < 16; i++)
			rtn[i] = a[i] + b[i];
		return rtn;
	}

	public static Matrix4x4 Minus(this Matrix4x4 a, Matrix4x4 b)
	{
		Matrix4x4 rtn = Matrix4x4.zero;
		for (int i = 0; i < 16; i++)
			rtn[i] = a[i] - b[i];
		return rtn;
	}

	public static Matrix4x4 Times(this Matrix4x4 a, float b)
	{
		Matrix4x4 rtn = Matrix4x4.zero;
		for (int i = 0; i < 16; i++)
			rtn[i] = a[i] * b;
		return rtn;
	}

	public static Matrix4x4 OuterSquared(this Vector3 a) => new(
			new Vector4(a.x * a.x, a.x * a.y, a.x * a.z, 0),
			new Vector4(a.y * a.x, a.y * a.y, a.y * a.z, 0),
			new Vector4(a.z * a.x, a.z * a.y, a.z * a.z, 0),
			new Vector4(0, 0, 0, 1));

	public static float InnerSquared(this Vector3 a) => Vector3.Dot(a, a);
}
