using KBCore.Refs;
using SBEPIS.Commands;
using SBEPIS.Commands.Commands;

namespace SBEPIS.Tests.Scenes
{
	public class CommandScene : ValidatedMonoBehaviour
	{
		[Anywhere] public CommandStaff staff;
		[Anywhere] public PingCommand ping;
		[Anywhere] public KillCommand kill;
	}
}