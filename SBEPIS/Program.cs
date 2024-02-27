using Echidna2.Rendering;
using Echidna2.Serialization;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SBEPIS;
using Window = Echidna2.Rendering.Window;

Console.WriteLine("Hello, World!");


Scene root = TomlSerializer.Deserialize<Scene>($"{AppContext.BaseDirectory}/Prefabs/Scene.toml");



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
