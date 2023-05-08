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
		
		protected override WobblyLayout SettingsPageLayoutData => layout;
	}
}
