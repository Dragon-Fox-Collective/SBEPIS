using System;
using KBCore.Refs;
using SBEPIS.Capturellection;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Tests.Scenes
{
	public class CapturellectableScene : MonoBehaviour
	{
		[Anywhere] public Capturellectable capturellectable;
		[Anywhere] public CaptureContainer container;
		[Anywhere] public Capturellector capturellector;
		[FormerlySerializedAs("inventory")]
		[Anywhere] public Inventory inventory1;
		[Anywhere] public Inventory inventory2;
		
		private void OnValidate() => this.ValidateRefs();
	}
}