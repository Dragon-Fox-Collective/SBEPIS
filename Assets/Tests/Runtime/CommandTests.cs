using NUnit.Framework;
using SBEPIS.Commands;
using SBEPIS.Utils;
using SBEPIS.Tests.Scenes;

namespace SBEPIS.Tests
{
	public class CommandTests : TestSceneSuite<CommandScene>
	{
		[Test]
		public void NoCommand_DoesNothing()
		{
			bool did = false;
			Scene.ping.OnPing += () => did = true;
			
			Scene.staff.AddNote(Notes.C4);
			Scene.staff.AddNote(Notes.D4);
			
			Assert.That(!did);
		}
		
		[Test]
		public void PingCommand_Pings()
		{
			bool did = false;
			Scene.ping.OnPing += () => did = true;
			
			Scene.staff.AddNote(Notes.C4);
			Scene.staff.AddNote(Notes.D4);
			Scene.staff.AddNote(Notes.E4);
			
			Assert.That(did);
		}
	}
}