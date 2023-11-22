using KBCore.Refs;
using SBEPIS.Commands;
using Ping = SBEPIS.Commands.Commands.Ping;

namespace SBEPIS.Tests.Scenes
{
	public class CommandScene : ValidatedMonoBehaviour
	{
		[Anywhere] public CommandStaff staff;
		[Anywhere] public Ping ping;
	}
}