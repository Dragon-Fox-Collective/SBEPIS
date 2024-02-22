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


Transform3D groundTransform = new() { IsGlobal = true, LocalPosition = Vector3.Down * 15, LocalScale = new Vector3(50, 50, 0.5)};
Box groundShape = new(100, 100, 1);
StaticBody groundBody = new(groundTransform, BodyShape.Of(groundShape))
{
	PhysicsMaterial = new PhysicsMaterial { Friction = 10 },
	Simulation = simulation
};
PBRMeshRenderer groundMesh = new(groundTransform) { Mesh = Mesh.Cube, Albedo = Color.LightGray, Roughness = 0.5 };
root.AddChild(groundBody);
root.AddChild(groundMesh);

Player player = new(simulation, root);
player.Camera.Size = (650, 450);
root.AddChild(player);



//IHasChildren.PrintTree(root);

PostProcessing postProcessing = new(new Shader(File.ReadAllText("Assets/post.vert"), File.ReadAllText("Assets/post.frag")), (1280, 720), player.Camera);

Window window = new(new GameWindow(
	new GameWindowSettings(),
	new NativeWindowSettings
	{
		ClientSize = (1280, 720),
		Title = "SBEPIS",
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
	position += new Vector3(Random.Shared.NextDouble(), Random.Shared.NextDouble(), Random.Shared.NextDouble());
	Consort consort = new(simulation);
	consort.MeshRenderer.Albedo = Color.FromArgb((int)color.X, (int)color.Y, (int)color.Z);
	consort.MeshRenderer.AmbientOcclusion = ao;
	root.AddChild(consort);
}