using KBCore.Refs;
using SBEPIS.Capturellection;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Tests.Scenes
{
	public class DiajectorScene : ValidatedMonoBehaviour
	{
		[Anywhere] public DiajectorCloser closer;
		[Anywhere] public Diajector diajector1;
		[Anywhere] public Diajector diajector2;
		[Anywhere] public DiajectorPage diajector1Page;
		[Anywhere] public CardTarget cardTarget;
		[Anywhere] public LerpTarget startCardTarget;
		[Anywhere] public LerpTarget endCardTarget;
	}
}