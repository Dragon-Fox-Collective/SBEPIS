using System.Drawing;
using BepuPhysics.Collidables;
using Echidna2.Core;
using Echidna2.Mathematics;
using Echidna2.Physics;
using Echidna2.Rendering;
using Echidna2.Rendering3D;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SBEPIS;
using Mesh = Echidna2.Rendering.Mesh;
using Window = Echidna2.Rendering.Window;

Console.WriteLine("Hello, World!");


Hierarchy root = new();

WorldSimulation simulation = new(root);
root.AddChild(simulation);


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

SkyboxRenderer skybox = new();
root.AddChild(skybox);


Transform3D groundTransform = new() { LocalPosition = Vector3.Down * 7, LocalScale = new Vector3(60, 60, 0.5)};
Box groundShape = new(120, 120, 1);
StaticBody groundBody = new(simulation, groundTransform, BodyShape.Of(groundShape))
{
	PhysicsMaterial = new PhysicsMaterial { Friction = 10 },
};
PBRMeshRenderer groundMesh = new(groundTransform) { Mesh = Mesh.Cube, Albedo = Color.LightGray, Roughness = 0.5 };
root.AddChild(groundBody);
root.AddChild(groundMesh);

Player player = new(simulation, root);
player.Camera.Size = (650, 450);
root.AddChild(player);



//IHasChildren.PrintTree(root);

PostProcessing postProcessing = new(
	new Shader(File.ReadAllText($"{AppContext.BaseDirectory}/Assets/quad.vert"), File.ReadAllText($"{AppContext.BaseDirectory}/Assets/post.frag")),
	new Shader(File.ReadAllText($"{AppContext.BaseDirectory}/Assets/quad.vert"), File.ReadAllText($"{AppContext.BaseDirectory}/Assets/blur.frag")),
	(1280, 720),
	player.Camera
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
	Camera = player.Camera,
	PostProcessing = postProcessing,
};
window.GameWindow.KeyDown += args =>
{
	if (args.Key == Keys.Escape) window.GameWindow.Close();
};
window.Resize += size =>
{
	postProcessing.Size = size;
	player.Camera.Size = size;
};
window.Run();
return;

void AddConsort(Vector3 position, Vector3 color, double ao = 1.0)
{
	// position += new Vector3(Random.Shared.NextDouble(), Random.Shared.NextDouble(), Random.Shared.NextDouble());
	position += Vector3.Up * 5;
	Consort consort = new(simulation)
	{
		LocalPosition = position,
	};
	consort.MeshRenderer.Albedo = Color.FromArgb((int)color.X, (int)color.Y, (int)color.Z);
	consort.MeshRenderer.AmbientOcclusion = ao;
	root.AddChild(consort);
}