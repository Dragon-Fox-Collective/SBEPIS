using System.Drawing;
using BepuPhysics.Collidables;
using Echidna2.Core;
using Echidna2.Mathematics;
using Echidna2.Physics;
using Echidna2.Rendering;
using Echidna2.Rendering3D;
using OpenTK.Windowing.Desktop;
using Mesh = Echidna2.Rendering.Mesh;

Console.WriteLine("Hello, World!");


Hierarchy root = new();

WorldSimulation simulation = new(root);
root.AddChild(simulation);

Vector3 cameraPosition = new(5, -15, 3);
Transform3D cameraTransform = new() { IsGlobal = true, LocalPosition = cameraPosition, LocalRotation = Quaternion.LookAt(cameraPosition, Vector3.Down * 15, Vector3.Up)};
Camera3D camera = new(root, cameraTransform) { FieldOfView = 100 };

AddCube((0, 0, 0), (255, 255, 255), ao: 300.0);

AddCube((-10, 0, 0), (0, 255, 255));
AddCube((+10, 0, 0), (255, 0, 0));
AddCube((0, -10, 0), (255, 0, 255));
AddCube((0, +10, 0), (0, 255, 0));
AddCube((0, 0, -10), (255, 255, 0));
AddCube((0, 0, +10), (0, 0, 255));

AddCube((0, -10, 10), (127, 0, 255));
AddCube((0, -10, -10), (255, 127, 0));
AddCube((0, 10, -10), (127, 255, 0));
AddCube((0, 10, 10), (0, 127, 127));
AddCube((10, 0, 10), (127, 0, 127));
AddCube((-10, 0, -10), (127, 255, 127));
AddCube((-10, 0, 10), (0, 127, 255));
AddCube((10, 0,  -10), (255, 127, 0));
AddCube((10, 10, 0), (127, 127, 0));
AddCube((-10, 10, 0), (0, 255, 127));
AddCube((10, -10, 0), (255, 0, 127));
AddCube((-10, -10, 0), (127, 127, 255));

AddCube((10, 10, 10), (63, 63, 63));
AddCube((-10, 10, 10), (0, 191, 191));
AddCube((10, -10, 10), (191, 0, 191));
AddCube((-10, -10, 10), (63, 63, 191));
AddCube((10, 10, -10), (191, 191, 0));
AddCube((-10, 10, -10), (63, 191, 63));
AddCube((10, -10, -10), (191, 63, 63));
AddCube((-10, -10, -10), (191, 191, 191));

SkyboxRenderer skybox = new();
root.AddChild(skybox);


Transform3D groundTransform = new() { IsGlobal = true, LocalPosition = Vector3.Down * 15, LocalScale = new Vector3(100, 100, 1)};
Box groundShape = new(100, 100, 1);
StaticBody groundBody = new(simulation, groundTransform, BodyShape.Of(groundShape));
PBRMeshRenderer groundMesh = new(groundTransform) { Mesh = Mesh.Cube, Albedo = Color.LightGray, Roughness = 0.5 };
root.AddChild(groundBody);
root.AddChild(groundMesh);




//IHasChildren.PrintTree(root);

Window window = new(new GameWindow(
	new GameWindowSettings(),
	new NativeWindowSettings
	{
		ClientSize = (1280, 720),
		Title = "SBEPIS",
	}
))
{
	Camera = camera
};
window.Run();
return;

void AddCube(Vector3 position, Vector3 color, double ao = 1.0)
{
	Transform3D cubeTransform = new() { IsGlobal = true, LocalPosition = position };
	Box cubeShape = new(2, 2, 2);
	DynamicBody cubeBody = new(simulation, cubeTransform, cubeShape.ComputeInertia(1), BodyShape.Of(cubeShape));
	GravityAffector cubeGravity = new(cubeBody);
	PBRMeshRenderer cubeRenderer = new(cubeTransform) { Mesh = Mesh.Cube, Albedo = Color.FromArgb((int)color.X, (int)color.Y, (int)color.Z), AmbientOcclusion = ao };
	root.AddChild(cubeBody);
	root.AddChild(cubeGravity);
	root.AddChild(cubeRenderer);
}