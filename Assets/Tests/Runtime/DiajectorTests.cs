using NUnit.Framework;
using SBEPIS.Utils;
using SBEPIS.Tests.Scenes;
using UnityEngine;

namespace SBEPIS.Tests
{
	public class DiajectorTests : TestSceneSuite<DiajectorScene>
	{
		[Test]
		public void OpeningDiajector_OpensDiajector()
		{
			Scene.diajector1.StartAssembly(Scene.closer, Vector3.zero, Quaternion.identity);
			Assert.That(Scene.diajector1.IsOpen);
		}
		
		[Test]
		public void OpeningSecondDiajector_ClosesFirstDiajector()
		{
			Scene.diajector1.StartAssembly(Scene.closer, Vector3.zero, Quaternion.identity);
			Scene.diajector2.StartAssembly(Scene.closer, Vector3.zero, Quaternion.identity);
			Assert.That(!Scene.diajector1.IsOpen);
		}
	}
}