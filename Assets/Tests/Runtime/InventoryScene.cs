using KBCore.Refs;
using SBEPIS.Capturellection;
using UnityEngine;

namespace SBEPIS.Tests.Scenes
{
	public class InventoryScene : ValidatedMonoBehaviour
	{
		[Anywhere] public Inventory inventory;
		[Anywhere] public Capturellectable item;
	}
}