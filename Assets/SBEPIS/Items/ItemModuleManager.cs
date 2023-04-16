using System;
using System.Collections.Generic;
using UnityEngine;
using SBEPIS.Utils;
using UnityEngine.Serialization;

namespace SBEPIS.Items
{
	[CreateAssetMenu(fileName=nameof(ItemModuleManager))]
	public class ItemModuleManager : ScriptableSingleton<ItemModuleManager>
	{
		[FormerlySerializedAs("_trueModule")]
		[FormerlySerializedAs("_trueBase")]
		[SerializeField]
		private Item _itemBase;
		public Item itemBase => _itemBase;
		
		[FormerlySerializedAs("_itemBases")]
		[SerializeField]
		private ItemModule[] _modules = Array.Empty<ItemModule>();
		public IEnumerable<ItemModule> modules => _modules;
	}
}
