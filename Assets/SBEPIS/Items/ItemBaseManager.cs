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
		private ItemBase[] _itemBases = new ItemBase[0];
		private ReadOnlyCollection<ItemBase> readOnlyItemBases;
		public ReadOnlyCollection<ItemBase> itemBases => readOnlyItemBases ??= new(_itemBases);
	}
}
