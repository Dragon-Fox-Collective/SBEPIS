using SBEPIS.Utils.VectorLinq;
using UnityEngine;

namespace SBEPIS.Utils
{
	public static class Math
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
		
		public static Vector3 MultiplyTerms(Vector3 a, Vector3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);
		
		public static Bounds Containing(this Bounds a, Bounds b)
		{
			Bounds bounds = new(a.center, a.size);
			bounds.Encapsulate(b);
			return bounds;
		}
		
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
}
