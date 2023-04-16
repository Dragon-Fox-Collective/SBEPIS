using KBCore.Refs;
using SBEPIS.Capturellection;
using UnityEngine;

namespace SBEPIS.Tests.Scenes
{
	public class DiajectorScene : MonoBehaviour
	{
		[Anywhere] public DiajectorCloser closer;
		[Anywhere] public Diajector diajector1;
		[Anywhere] public Diajector diajector2;
		
		private void OnValidate() => this.ValidateRefs();
	}
}