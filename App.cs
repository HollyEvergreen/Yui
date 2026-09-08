using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace Yui;

public abstract class App
{
	protected Filesystem fs = new();
	private double _dt;
	private Timer windowAutoSave;
	private long ASPeriod = 300_000;
    protected TimeSpan AutoSavePeriod
    {
        get => new(ASPeriod * 10_000);
        set => ASPeriod = value.Ticks / 10_000;
    }
    public IWindow? Window { get; private set; }
    public GL? Gl { get; private set; }
	public IInputContext? Input {get; private set;}
    public ImGuiController? Imgui { get; private set; }

    private void WinAutoSave(object? window)
    {
        
    }

    public int run()
	{
        Window = Silk.NET.Windowing.Window.Create(fs.GetConfig<WindowOptions>("Window"));
		Gl = Window.CreateOpenGL();
		Input = Window.CreateInput();
        Imgui = new ImGuiController(Gl, Window, Input);
		windowAutoSave = new(WinAutoSave, null, ASPeriod, ASPeriod);
		Window.Load += on_load;
		Window.Load += Start;
		Window.Update += update;
		Window.Render += render;

		Window.Run();
		return 0;
	}
	private void on_load()
	{
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