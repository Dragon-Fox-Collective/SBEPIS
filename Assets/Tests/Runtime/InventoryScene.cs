using KBCore.Refs;
using SBEPIS.Capturellection;

namespace SBEPIS.Tests.Scenes
{
	public class InventoryScene : ValidatedMonoBehaviour
	{
		[Anywhere] public Inventory inventory;
		[Anywhere] public Capturellectable item;
		[Anywhere] public InventoryStorable card;
	}
}