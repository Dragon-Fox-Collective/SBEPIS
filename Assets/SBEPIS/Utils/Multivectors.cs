using System;
using UnityEngine;

namespace SBEPIS.Utils
{
	[Serializable]
	public struct Vector3X2
	{
		public Vector3 x, y;
		public Vector2 Magnitude => new(x.magnitude, y.magnitude);
		
		public Vector3X2(Vector3 x, Vector3 y)
		{
			this.x = x;
			this.y = y;
		}
		
		public static Vector3X2 operator *(Vector2 scalar, Vector3X2 vector) => new(scalar.x * vector.x, scalar.y * vector.y);
		
		public static Vector3X2 Project(Vector3X2 vector, Vector3X2 onNormal) => new(Vector3.Project(vector.x, onNormal.x), Vector3.Project(vector.y, onNormal.y));
		public static Vector3X2 Project(Vector3 vector, Vector3X2 onNormal) => new(Vector3.Project(vector, onNormal.x), Vector3.Project(vector, onNormal.y));

		public Vector3 Sum() => x + y;
	}
	
	[Serializable]
	public struct Vector3X3
	{
		public Vector3 x, y, z;
		
		public Vector3X3(Vector3 x, Vector3 y, Vector3 z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}

	public static class MultivectorExtensions
	{
		public static Vector3X2 Select(this Vector3X2 vector, Func<Vector3, Vector3> func) => new(func(vector.x), func(vector.y));
		public static Vector3X2 SelectIndex(this Vector3X2 vector, Func<int, Vector3, Vector3> func) => new(func(0, vector.x), func(1, vector.y));
		public static Vector2 AggregateIndex(this Vector3X2 vector, Func<int, Vector3, float> func) => new(func(0, vector.x), func(1, vector.y));
		public static Vector3X2 Select(this Vector3X2 vector, Func<float, float> func) => vector.Select(v => v.Select(func));
	}
}