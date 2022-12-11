using System;
using System.Collections.Generic;
using UnityEngine;
using SBEPIS.Utils;
using System.Collections.ObjectModel;

namespace SBEPIS.Items
{
	[CreateAssetMenu(fileName=nameof(ItemBaseManager))]
	public class ItemBaseManager : ScriptableSingleton<ItemBaseManager>
	{
		[SerializeField]
		private Item _trueBase;
		public Item trueBase => _trueBase;

		[SerializeField]
		private ItemBase[] _itemBases = Array.Empty<ItemBase>();
		public IEnumerable<ItemBase> itemBases => _itemBases;
	}
}
