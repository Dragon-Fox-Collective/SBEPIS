using KBCore.Refs;
using SBEPIS.Capturellection;
using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Tests.Scenes
{
	public class PlayerSystemScene : ValidatedMonoBehaviour
	{
		[Anywhere] public Capturellector capturellector;
		[Anywhere] public Grabber grabber;
		[Anywhere] public Grabbable dequeBoxGrabbable;
		[Anywhere] public Inventory dequeBoxInventory;
	}
}