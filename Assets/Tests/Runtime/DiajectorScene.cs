using KBCore.Refs;
using SBEPIS.Capturellection;
using SBEPIS.Utils;
using UnityEngine.Serialization;

namespace SBEPIS.Tests.Scenes
{
	public class DiajectorScene : ValidatedMonoBehaviour
	{
		[FormerlySerializedAs("diajector1")]
		[Anywhere] public Diajector diajector;
		[FormerlySerializedAs("diajector1Page")]
		[Anywhere] public DiajectorPage diajectorPage;
		[Anywhere] public CardTarget cardTarget;
		[Anywhere] public LerpTarget startCardTarget;
		[Anywhere] public LerpTarget endCardTarget;
	}
}