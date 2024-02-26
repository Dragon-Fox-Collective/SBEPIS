using System.Drawing;
using Echidna2.Core;
using Echidna2.Mathematics;
using Echidna2.Physics;
using Echidna2.Rendering;
using Echidna2.Serialization;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SBEPIS;
using Window = Echidna2.Rendering.Window;

Console.WriteLine("Hello, World!");


Scene root = TomlSerializer.Deserialize<Scene>($"{AppContext.BaseDirectory}/Prefabs/Scene.toml");



AddConsort((0, 0, 0), (255, 255, 255), ao: 300.0);

AddConsort((-10, 0, 0), (0, 255, 255));
AddConsort((+10, 0, 0), (255, 0, 0));
AddConsort((0, -10, 0), (255, 0, 255));
AddConsort((0, +10, 0), (0, 255, 0));
AddConsort((0, 0, -10), (255, 255, 0));
AddConsort((0, 0, +10), (0, 0, 255));

AddConsort((0, -10, 10), (127, 0, 255));
AddConsort((0, -10, -10), (255, 127, 0));
AddConsort((0, 10, -10), (127, 255, 0));
AddConsort((0, 10, 10), (0, 127, 127));
AddConsort((10, 0, 10), (127, 0, 127));
AddConsort((-10, 0, -10), (127, 255, 127));
AddConsort((-10, 0, 10), (0, 127, 255));
AddConsort((10, 0,  -10), (255, 127, 0));
AddConsort((10, 10, 0), (127, 127, 0));
AddConsort((-10, 10, 0), (0, 255, 127));
AddConsort((10, -10, 0), (255, 0, 127));
AddConsort((-10, -10, 0), (127, 127, 255));

AddConsort((10, 10, 10), (63, 63, 63));
AddConsort((-10, 10, 10), (0, 191, 191));
AddConsort((10, -10, 10), (191, 0, 191));
AddConsort((-10, -10, 10), (63, 63, 191));
AddConsort((10, 10, -10), (191, 191, 0));
AddConsort((-10, 10, -10), (63, 191, 63));
AddConsort((10, -10, -10), (191, 63, 63));
AddConsort((-10, -10, -10), (191, 191, 191));


//IHasChildren.PrintTree(root);

WorldSimulation simulation = new(root);
root.AddChild(simulation);
INotificationPropagator.Notify(new IInitializeIntoSimulation.Notification(simulation), root);

PostProcessing postProcessing = new(
	new Shader(File.ReadAllText($"{AppContext.BaseDirectory}/Assets/quad.vert"), File.ReadAllText($"{AppContext.BaseDirectory}/Assets/post.frag")),
	new Shader(File.ReadAllText($"{AppContext.BaseDirectory}/Assets/quad.vert"), File.ReadAllText($"{AppContext.BaseDirectory}/Assets/blur.frag")),
	(1280, 720),
	root.Player.Camera
);

Window window = new(new GameWindow(
	new GameWindowSettings(),
	new NativeWindowSettings
	{
		ClientSize = (1280, 720),
		Title = "SBEPIS",
		Icon = Window.CreateWindowIcon($"{AppContext.BaseDirectory}/Assets/sbepis.png"),
	}
)
{
	CursorState = CursorState.Grabbed,
})
{
	Camera = root.Player.Camera,
	PostProcessing = postProcessing,
};
window.GameWindow.KeyDown += args =>
{
	if (args.Key == Keys.Escape) window.GameWindow.Close();
};
window.Resize += size =>
{
	postProcessing.Size = size;
	root.Player.Camera.Size = size;
};
window.Run();
return;

void AddConsort(Vector3 position, Vector3 color, double ao = 1.0)
{
	// position += new Vector3(Random.Shared.NextDouble(), Random.Shared.NextDouble(), Random.Shared.NextDouble());
	position += Vector3.Up * 5;
	Consort consort = TomlSerializer.Deserialize<Consort>($"{AppContext.BaseDirectory}/Prefabs/Consort.toml");
	consort.LocalPosition = position;
	consort.MeshRenderer.Albedo = Color.FromArgb((int)color.X, (int)color.Y, (int)color.Z);
	consort.MeshRenderer.AmbientOcclusion = ao;
	root.AddChild(consort);
}