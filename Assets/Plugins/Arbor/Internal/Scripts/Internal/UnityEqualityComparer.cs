//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.Internal
{
	internal static class UnityEqualityComparer
	{
		static readonly IEqualityComparer<Color32> color32EqualityComparer = new Color32EqualityComparer();
		static readonly IEqualityComparer<Ray> rayEqualityComparer = new RayEqualityComparer();
		static readonly IEqualityComparer<Ray2D> ray2DEqualityComparer = new Ray2DEqualityComparer();
		static readonly IEqualityComparer<RaycastHit> raycastHitEqualityComparer = new RaycastHitEqualityComparer();
		static readonly IEqualityComparer<RaycastHit2D> raycastHit2DEqualityComparer = new Raycast2DHitEqualityComparer();

		public static IEqualityComparer<T> GetComparer<T>()
		{
			return Cache<T>.Comparer;
		}

		static class Cache<T>
		{
			public readonly static IEqualityComparer<T> Comparer;

			static Cache()
			{
				var comparer = GetDefaultComparer(typeof(T));
				if (comparer != null)
				{
					Comparer = (IEqualityComparer<T>)comparer;
				}
				else
				{
					Comparer = EqualityComparer<T>.Default;
				}
			}
		}

		static object GetDefaultComparer(System.Type t)
		{
			if (t == typeof(Color32)) return color32EqualityComparer;
			if (t == typeof(Ray)) return rayEqualityComparer;
			if (t == typeof(Ray2D)) return ray2DEqualityComparer;
			if (t == typeof(RaycastHit)) return raycastHitEqualityComparer;
			if (t == typeof(RaycastHit2D)) return raycastHit2DEqualityComparer;

			return null;
		}

		sealed class Color32EqualityComparer : IEqualityComparer<Color32>
		{
			public bool Equals(Color32 x, Color32 y)
			{
				return x.a.Equals(y.a) && x.r.Equals(y.r) && x.g.Equals(y.g) && x.b.Equals(y.b);
			}

			public int GetHashCode(Color32 obj)
			{
				return obj.a.GetHashCode() ^ obj.r.GetHashCode() << 2 ^ obj.g.GetHashCode() >> 2 ^ obj.b.GetHashCode() >> 1;
			}
		}

		sealed class RayEqualityComparer : IEqualityComparer<Ray>
		{
			public bool Equals(Ray x, Ray y)
			{
				return x.origin.Equals(y.origin) && x.direction.Equals(y.direction);
			}

			public int GetHashCode(Ray obj)
			{
				return (obj.origin.GetHashCode(), obj.direction.GetHashCode()).GetHashCode();
			}
		}

		sealed class Ray2DEqualityComparer : IEqualityComparer<Ray2D>
		{
			public bool Equals(Ray2D x, Ray2D y)
			{
				return x.origin.Equals(y.origin) && x.direction.Equals(y.direction);
			}

			public int GetHashCode(Ray2D obj)
			{
				return (obj.origin.GetHashCode(), obj.direction.GetHashCode()).GetHashCode();
			}
		}

		sealed class RaycastHitEqualityComparer : IEqualityComparer<RaycastHit>
		{
			public bool Equals(RaycastHit x, RaycastHit y)
			{
				return x.barycentricCoordinate.Equals(y.barycentricCoordinate) &&
					x.collider == y.collider &&
					x.distance.Equals(y.distance) &&
					x.normal.Equals(y.normal) &&
					x.point.Equals(y.point) &&
					x.triangleIndex.Equals(y.triangleIndex);
			}

			public int GetHashCode(RaycastHit obj)
			{
				return (obj.barycentricCoordinate.GetHashCode(),
					(obj.collider != null) ? obj.collider.GetHashCode() : 0,
					obj.distance.GetHashCode(),
					obj.normal.GetHashCode(),
					obj.point.GetHashCode(),
					obj.triangleIndex).GetHashCode();
			}
		}

		sealed class Raycast2DHitEqualityComparer : IEqualityComparer<RaycastHit2D>
		{
			public bool Equals(RaycastHit2D x, RaycastHit2D y)
			{
				return x.centroid.Equals(y.centroid) &&
					x.collider == y.collider &&
					x.distance.Equals(y.distance) &&
					x.fraction.Equals(y.fraction) &&
					x.normal.Equals(y.normal) &&
					x.point.Equals(y.point);
			}

			public int GetHashCode(RaycastHit2D obj)
			{
				return (obj.centroid.GetHashCode(),
					(obj.collider != null) ? obj.collider.GetHashCode() : 0,
					obj.distance.GetHashCode(),
					obj.fraction.GetHashCode(),
					obj.normal.GetHashCode(),
					obj.point.GetHashCode()).GetHashCode();
			}
		}
	}
}
