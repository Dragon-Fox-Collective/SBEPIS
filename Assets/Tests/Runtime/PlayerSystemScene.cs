using KBCore.Refs;
using SBEPIS.Capturellection;
using SBEPIS.Controller;

namespace SBEPIS.Tests.Scenes
{
	public class PlayerSystemScene : ValidatedMonoBehaviour
	{
		[Anywhere] public Capturellector capturellector;
		[Anywhere] public Grabber grabber;
		[Anywhere] public CouplingSocket hipSocket;
		[Anywhere] public CouplingPlug startingDequePlug;
		[Anywhere] public Grabbable dequeBoxGrabbable;
		[Anywhere] public Inventory dequeBoxInventory;
	}
}