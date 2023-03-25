using System.Collections;
using NUnit.Framework;
using SBEPIS.Tests.Scenes;
using SBEPIS.Utils;
using UnityEngine.TestTools;

namespace SBEPIS.Tests
{
	public class DiajectorTests : TestSceneSuite<DiajectorScene>
	{
		[UnityTest]
		public IEnumerator Test1()
		{
			Assert.NotNull(scene);
			yield break;
		}
		
		[UnityTest]
		public IEnumerator Test2()
		{
			Assert.NotNull(scene.obj);
			yield break;
		}
	}
}