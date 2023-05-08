using System.Collections.Generic;
using System.Linq;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class ArrayDeque : SingleDeque<WobblyLayout, WobblyState>
	{
		[SerializeField] private WobblyLayout layout;
		
		public override void Tick(List<Storable> inventory, WobblyState state, float deltaTime) => layout.Tick(inventory, state, deltaTime);
		
		public override bool CanFetchFrom(List<Storable> inventory, WobblyState state, InventoryStorable card) => inventory.Any(storable => storable.CanFetch(card));
		
		protected override WobblyLayout SettingsPageLayoutData => layout;
	}
}
