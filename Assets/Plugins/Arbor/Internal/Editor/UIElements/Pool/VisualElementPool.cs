//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArborEditor.UIElements.Pool
{
	using Arbor.Pool;

	public class VisualElementPool<T> where T : VisualElement, new()
	{
		internal static readonly ObjectPool<T> s_Pool = new ObjectPool<T>(
			() => new T(),
			null,
			l => l.RemoveFromHierarchy(),
			null,
			true, 10, 10000);

		public static T Get()
		{
			return s_Pool.Get();
		}

		public static PooledObject<T> Get(out T value)
		{
			return s_Pool.Get(out value);
		}

		public static void Release(T toRelease)
		{
			s_Pool.Release(toRelease);
		}
	}
}