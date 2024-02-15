using Echidna2.Core;
using Echidna2.Rendering;
using OpenTK.Windowing.Desktop;

Console.WriteLine("Hello, World!");


Hierarchy root = new();


IHasChildren.PrintTree(root);

Window window = new(new GameWindow(
	new GameWindowSettings(),
	new NativeWindowSettings
	{
		ClientSize = (1280, 720),
		Title = "SBEPIS",
	}
))
{
	Camera = new Camera { World = root }
};
// window.Resize += size => root.LocalSize = size;
window.Run();
return;