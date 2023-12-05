using System.Collections.Generic;
using UnityEngine;
using SBEPIS.Utils;
using UnityEngine.Serialization;

namespace SBEPIS.Items
{
	[CreateAssetMenu(fileName=nameof(ItemModuleManager))]
	public class ItemModuleManager : ScriptableSingleton<ItemModuleManager>
	{
		[FormerlySerializedAs("_itemBase")]
		[FormerlySerializedAs("_trueModule")]
		[FormerlySerializedAs("_trueBase")]
		[SerializeField]
		private Item itemBase;
		public Item ItemBase => itemBase;
		
		[FormerlySerializedAs("_modules")]
		[FormerlySerializedAs("_itemBases")]
		[SerializeField]
		private List<ItemModule> modules = new();
		public IEnumerable<ItemModule> Modules => modules;
		
		[SerializeField]
		private List<ItemModule> uniqueModules = new();
		public IEnumerable<ItemModule> UniqueModules => uniqueModules;
	}
}
