using System.Drawing;
using Echidna2.Core;
using Echidna2.Mathematics;
using Echidna2.Rendering;
using Echidna2.Rendering3D;
using OpenTK.Windowing.Desktop;

Console.WriteLine("Hello, World!");


Hierarchy root = new();

Transform3D cameraTransform = new() { IsGlobal = true };
Camera3D camera = new(root, cameraTransform) { FieldOfView = 100 };
Spinner cameraSpinner = new(cameraTransform);
root.AddChild(cameraSpinner);

AddCube((0, 0, 0), (0, 0, 0));

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

void AddCube(Vector3 position, Vector3 color)
{
	Transform3D cubeTransform = new() { IsGlobal = true, LocalPosition = position };
	MeshRenderer cubeRenderer = new(cubeTransform) { Color = Color.FromArgb((int)color.X, (int)color.Y, (int)color.Z) };
	root.AddChild(cubeRenderer);
}

class Spinner(Transform3D transform) : IUpdate
{
	private double time = 0;
	
	public void OnUpdate(double deltaTime)
	{
		time += deltaTime;
		// transform.LocalPosition = new Vector3(0, -5, 5 * Math.Sin(time));
		transform.LocalPosition = new Vector3(5 * Math.Sin(time), 5 * Math.Cos(time), 3 * Math.Sin(time / 3));
		transform.LocalRotation = Quaternion.LookAt(transform.LocalPosition, Vector3.Zero, Vector3.Up);
	}
}