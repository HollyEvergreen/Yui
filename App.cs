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
	protected TomlSerializerOptions toml_opts = new()
	{
		Converters = [new Serial.TomlVector2DConv(), new Serial.TomlVector3DConv(), new Serial.TomlVector4DConv()],
		WriteIndented = true,
	};
	protected Filesystem fs = new();
	private double _dt;
	private Timer windowAutoSave;
	private long ASPeriod = 300_000;
    protected TimeSpan AutoSavePeriod
    {
        get => new(ASPeriod * 10_000);
        set => ASPeriod = value.Ticks / 10_000;
    }
    public IWindow? _window { get; private set; }
    public GL? Gl { get; private set; }
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
	    WindowOptions windowConfig = fs.GetConfig<WindowConfig>("Window") ?? throw new("Failed to get config");
	    windowConfig.API = windowConfig.API with { Version = new APIVersion(4, 5) };
	    SdlWindowing.RegisterPlatform();
	    SdlWindowing.Use();
        _window = Window.Create(windowConfig);
        SdlInput.RegisterPlatform();
        SdlInput.Use();
		_window.Load += on_load;
		_window.Load += Start;
		_window.Update += update;
		_window.Render += render;

		_window.Run();
		return 0;
	}
	private void on_load()
	{
		Debug.Assert(_window != null, nameof(_window) + " != null");
		Gl = _window.CreateOpenGL();
		if (Environment.GetEnvironmentVariable("XDG_SESSION_TYPE") == "wayland") unsafe {
			Glfw.GetApi().SetWindowRefreshCallback((WindowHandle*)_window.Handle, _ => { });
		}
		Input = _window.CreateInput();
		Imgui = new ImGuiController(Gl, _window, Input);
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
}