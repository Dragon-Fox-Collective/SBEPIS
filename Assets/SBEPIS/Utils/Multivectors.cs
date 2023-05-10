using System;
using UnityEngine;

namespace SBEPIS.Utils
{
	[Serializable]
	public struct Vector3x2
	{
		public Vector3 x, y;
		public Vector2 magnitude => new(x.magnitude, y.magnitude);
		
		public Vector3x2(Vector3 x, Vector3 y)
		{
			this.x = x;
			this.y = y;
		}
		
		public static Vector3x2 operator *(Vector2 scalar, Vector3x2 vector) => new(scalar.x * vector.x, scalar.y * vector.y);
		
		public static Vector3x2 Project(Vector3x2 vector, Vector3x2 onNormal) => new(Vector3.Project(vector.x, onNormal.x), Vector3.Project(vector.y, onNormal.y));
		public static Vector3x2 Project(Vector3 vector, Vector3x2 onNormal) => new(Vector3.Project(vector, onNormal.x), Vector3.Project(vector, onNormal.y));

		public Vector3 Sum() => x + y;
	}
	
	[Serializable]
	public struct Vector3x3
	{
		public Vector3 x, y, z;
		
		public Vector3x3(Vector3 x, Vector3 y, Vector3 z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}
}