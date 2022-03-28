using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SBEPIS.Items
{
	[CreateAssetMenu]
	public class ItemBaseManager : ScriptableObject, IEnumerable<ItemBase>
	{
		public ItemBase[] itemBases;

		public IEnumerator<ItemBase> GetEnumerator()
		{
			return itemBases.Cast<ItemBase>().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
