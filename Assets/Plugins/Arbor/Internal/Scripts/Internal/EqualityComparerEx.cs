//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
using System.Collections.Generic;
using UnityObject = UnityEngine.Object;

namespace Arbor.Internal
{
	internal sealed class EqualityComparerEx<T> : IEqualityComparer<T>
	{
		private static readonly Func<T, T, bool> s_Equals;

		private static IEqualityComparer<T> s_EqualityComparer;

		private static IEqualityComparer<T> s_Default = null;

		public static IEqualityComparer<T> Default
		{
			get
			{
				if (s_Default == null)
				{
					s_Default = new EqualityComparerEx<T>();
				}
				return s_Default;
			}
		}

		static EqualityComparerEx()
		{
			if (!TypeUtility.IsAssignableFrom(typeof(IEquatable<T>), typeof(T)))
			{
				if (TypeUtility.IsAssignableFrom(typeof(UnityObject), typeof(T)))
				{
					s_Equals = EqualsUnityObject;
				}
				else
				{
					var method = MemberCache.GetMethodInfo(typeof(T), "op_Equality", new Type[] { typeof(T), typeof(T) });
					if (method != null)
					{
						s_Equals = (Func<T, T, bool>)method.CreateDelegate(typeof(Func<T, T, bool>));
					}
				}
			}

			s_EqualityComparer = UnityEqualityComparer.GetComparer<T>();
		}

		static bool EqualsUnityObject(T x, T y)
		{
			UnityObject objX = x as UnityObject;
			UnityObject objY = y as UnityObject;

			return objX == objY;
		}

		public bool Equals(T x, T y)
		{
			if (s_Equals != null)
			{
				return s_Equals(x, y);
			}

			if (s_EqualityComparer != null)
			{
				return s_EqualityComparer.Equals(x, y);
			}

			return object.Equals(x, y);
		}

		public int GetHashCode(T obj)
		{
			return s_EqualityComparer.GetHashCode(obj);
		}
	}
}