//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Arbor.ObjectPooling
{
	internal sealed class PoolQueue
	{
		private LinkedList<PoolObject> _List = new LinkedList<PoolObject>();

		public int Count
		{
			get
			{
				return _List.Count;
			}
		}

		public void Enqueue(PoolObject item)
		{
			item.node = _List.AddLast(item);
		}

		public PoolObject Dequeue()
		{
			var poolObject = _List.First.Value;

			_List.RemoveFirst();
			poolObject.node = null;

			return poolObject;
		}
	}
}