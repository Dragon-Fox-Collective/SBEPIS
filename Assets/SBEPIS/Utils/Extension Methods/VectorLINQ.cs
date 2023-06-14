using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Utils.VectorLinq
{
	public static class VectorLINQ
	{
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
		
		public static Vector3 MaxEach(Vector3 a, Vector3 b) => new(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z));
		public static Vector3 MaxEach(this IEnumerable<Vector3> source) => source.Aggregate(MaxEach);
		
		public static Quaternion Select(this Quaternion quaternion, Func<float, float> func) => new(func(quaternion.x), func(quaternion.y), func(quaternion.z), func(quaternion.w));
		
		public static Vector3x2 Select(this Vector3x2 vector, Func<Vector3, Vector3> func) => new(func(vector.x), func(vector.y));
		public static Vector3x2 SelectIndex(this Vector3x2 vector, Func<int, Vector3, Vector3> func) => new(func(0, vector.x), func(1, vector.y));
		public static Vector2 AggregateIndex(this Vector3x2 vector, Func<int, Vector3, float> func) => new(func(0, vector.x), func(1, vector.y));
		public static Vector3x2 Select(this Vector3x2 vector, Func<float, float> func) => vector.Select(v => v.Select(func));
	}
}
