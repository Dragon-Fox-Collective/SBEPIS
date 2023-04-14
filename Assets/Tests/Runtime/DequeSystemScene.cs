using SBEPIS.Capturellection;
using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Tests.Scenes
{
	public class DequeSystemScene : MonoBehaviour
	{
		public Grabber grabber;
		public DequeBox dequeBox;
		public Diajector diajector;
		public Grabbable dequeBoxGrabbable;
		public CollisionTrigger dequeBoxTrigger;
		public Transform dropPoint;
	}
}