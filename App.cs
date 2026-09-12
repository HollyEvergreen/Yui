using System.Diagnostics;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.Input.Sdl;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.SDL;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Sdl;
using Tomlyn;
using Yui.Serial;
using Window = Silk.NET.Windowing.Window;

namespace Yui;

public abstract class App
{
	protected App(bool Debug = false)
	{
		debug = Debug;
	}
	protected TomlSerializerOptions toml_opts = new()
	{
		Converters = [new Serial.TomlVector2DConv(), new Serial.TomlVector3DConv(), new Serial.TomlVector4DConv()],
		WriteIndented = true,
	};
	protected Filesystem fs = new();
	private double _dt;
	private Timer windowAutoSave;
	private long ASPeriod = 300_000;
	public static bool debug;

	protected TimeSpan AutoSavePeriod
    {
        get => new(ASPeriod * 10_000);
        set => ASPeriod = value.Ticks / 10_000;
    }
    public IWindow? _window { get; private set; }
	public IInputContext? Input {get; private set;}
    public ImGuiController? Imgui { get; private set; }
    //
    // public App()
    // {
    // }

    private void WinAutoSave(object? window)
    {
        
    }

    public int run()
    {
		Debug.Assert(OGL.gl != null);
	    WindowOptions windowConfig = fs.GetConfig<WindowConfig>("Window") ?? throw new("Failed to get config");
	    windowConfig.API = windowConfig.API with { Version = new APIVersion(4, 6) };
	    SdlWindowing.RegisterPlatform();
	    SdlWindowing.Use();
        _window = Window.Create(windowConfig);
        Debug.Assert(_window is not null);
        SdlInput.RegisterPlatform();
        SdlInput.Use();
		_window.Load += on_load;
		_window.Load += Start;
		_window.Update += update;
		_window.Render += render;
		_window.Resize += OGL.gl.Viewport;

		_window.Run();
		return 0;
	}
	private void on_load()
	{
		Debug.Assert(_window != null, nameof(_window) + " != null");
		OGL.gl = _window.CreateOpenGL();
		if (Environment.GetEnvironmentVariable("XDG_SESSION_TYPE") == "wayland") unsafe {
			Glfw.GetApi().SetWindowRefreshCallback((WindowHandle*)_window.Handle, _ => { });
		}
		Input = _window.CreateInput();
		Imgui = new ImGuiController(OGL.gl, _window, Input);
		windowAutoSave = new(WinAutoSave, null, ASPeriod, ASPeriod);
		OnLoad();
	}
	private void update(double dt)
	{
		Update(dt);
	}
	private void render(double dt)
	{
		Render(dt);
	}
	protected virtual void Start(){}
	protected virtual void OnLoad(){}
	protected virtual void Update(double dt){}
	protected virtual void Render(double dt){}

	protected void Log(string msg)
	{
		#if DEBUG
		if (debug) Console.Write(msg);
		#endif
	}
}