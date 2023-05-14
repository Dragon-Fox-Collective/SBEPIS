//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;

namespace ArborEditor
{
	public class UniqueIDGenerator
	{
		static System.Random s_Random = new System.Random();
		private HashSet<int> _ItemIDs = new HashSet<int>();

		public bool AddID(int id)
		{
			if (id <= 0 || _ItemIDs.Contains(id))
			{
				return false;
			}

			_ItemIDs.Add(id);
			return true;
		}

		public int CreateID()
		{
			while (true)
			{
				int id = s_Random.Next(1, int.MaxValue);

				if (AddID(id))
				{
					return id;
				}
			}
		}

		public void Clear()
		{
			_ItemIDs.Clear();
		}
	}
}