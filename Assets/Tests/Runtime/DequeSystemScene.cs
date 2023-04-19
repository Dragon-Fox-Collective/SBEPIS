using System.Collections.Generic;
using KBCore.Refs;
using SBEPIS.Capturellection;
using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Tests.Scenes
{
	public class DequeSystemScene : MonoBehaviour
	{
		[Anywhere] public Grabber grabber;
		[Anywhere] public DequeBox dequeBox;
		[Anywhere] public Diajector diajector;
		[Anywhere] public Grabbable dequeBoxGrabbable;
		[Anywhere] public CollisionTrigger dequeBoxTrigger;
		[Anywhere] public Transform dropPoint;
		[Anywhere] public Inventory inventory;
		public List<CardTarget> cardTargets = new();

		private void OnValidate() => this.ValidateRefs();
	}
}